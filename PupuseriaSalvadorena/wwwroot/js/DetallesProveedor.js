// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPaginationM() {
    var table = document.querySelector('#tablaMaterias tbody');
    var rows = table.rows;
    var totalRows = rows.length;
    var totalPages = Math.ceil(totalRows / rowsPerPage);

    function showPage(page) {
        for (let i = 0; i < totalRows; i++) {
            rows[i].style.display = (i >= (page - 1) * rowsPerPage && i < page * rowsPerPage) ? "" : "none";
        }

        document.getElementById('pageIndicatorM').textContent = `${page} / ${totalPages}`;
    }

    window.changePage = function (direction) {
        currentPage += direction;

        if (currentPage < 1) currentPage = 1;
        if (currentPage > totalPages) currentPage = totalPages;

        showPage(currentPage);
    };

    showPage(currentPage);
}

document.addEventListener('DOMContentLoaded', setupPaginationM);

// Busqueda
document.getElementById('busquedaMaterias').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var rows = document.querySelectorAll('#tablaMaterias tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaMaterias th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaMaterias');
            var tableBody = table.querySelector('tbody');
            var rowsArray = Array.from(tableBody.rows);
            var column = this.cellIndex;
            var isAscending = header.classList.contains('asc');
            var isDateColumn = this.id === 'Fecha';

            rowsArray.sort(function (rowA, rowB) {
                var textA = rowA.cells[column].textContent.trim();
                var textB = rowB.cells[column].textContent.trim();

                if (isDateColumn) {
                    var dateA = new Date(textA.split('/').reverse().join('-'));
                    var dateB = new Date(textB.split('/').reverse().join('-'));
                    return (dateA - dateB) * (isAscending ? -1 : 1);
                } else {
                    return textA.localeCompare(textB, undefined, { numeric: true }) * (isAscending ? -1 : 1);
                }
            });

            document.querySelectorAll('#tablaMaterias th').forEach(th => {
                th.innerHTML = th.textContent;
                th.classList.remove('asc', 'desc');
            });

            var newIconHtml = isAscending ? '<i class="fas fa-arrow-down"></i>' : '<i class="fas fa-arrow-up"></i>';
            this.innerHTML += ' ' + newIconHtml;
            this.classList.toggle('asc', !isAscending);
            this.classList.toggle('desc', isAscending);

            rowsArray.forEach(row => tableBody.appendChild(row));
        });
    }
});


// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPaginationC() {
    var table = document.querySelector('#tablaCuentas tbody');
    var rows = table.rows;
    var totalRows = rows.length;
    var totalPages = Math.ceil(totalRows / rowsPerPage);

    function showPage(page) {
        for (let i = 0; i < totalRows; i++) {
            rows[i].style.display = (i >= (page - 1) * rowsPerPage && i < page * rowsPerPage) ? "" : "none";
        }

        document.getElementById('pageIndicatorC').textContent = `${page} / ${totalPages}`;
    }

    window.changePage = function (direction) {
        currentPage += direction;

        if (currentPage < 1) currentPage = 1;
        if (currentPage > totalPages) currentPage = totalPages;

        showPage(currentPage);
    };

    showPage(currentPage);
}

document.addEventListener('DOMContentLoaded', setupPaginationC);

// Busqueda
document.getElementById('busquedaCuentas').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var rows = document.querySelectorAll('#tablaCuentas tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaCuentas th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaCuentas');
            var tableBody = table.querySelector('tbody');
            var rowsArray = Array.from(tableBody.rows);
            var column = this.cellIndex;
            var isAscending = header.classList.contains('asc');
            var isDateColumn = this.id === 'Fecha';

            rowsArray.sort(function (rowA, rowB) {
                var textA = rowA.cells[column].textContent.trim();
                var textB = rowB.cells[column].textContent.trim();

                if (isDateColumn) {
                    var dateA = new Date(textA.split('/').reverse().join('-'));
                    var dateB = new Date(textB.split('/').reverse().join('-'));
                    return (dateA - dateB) * (isAscending ? -1 : 1);
                } else {
                    return textA.localeCompare(textB, undefined, { numeric: true }) * (isAscending ? -1 : 1);
                }
            });

            document.querySelectorAll('#tablaCuentas th').forEach(th => {
                th.innerHTML = th.textContent;
                th.classList.remove('asc', 'desc');
            });

            var newIconHtml = isAscending ? '<i class="fas fa-arrow-down"></i>' : '<i class="fas fa-arrow-up"></i>';
            this.innerHTML += ' ' + newIconHtml;
            this.classList.toggle('asc', !isAscending);
            this.classList.toggle('desc', isAscending);

            rowsArray.forEach(row => tableBody.appendChild(row));
        });
    }
});

//Grafico Circular
document.addEventListener('DOMContentLoaded', function () {
    var materiasPrimas = detalles.map(function (d) { return d.NombreMateriaPrima; });
    var conteoMaterias = detalles.map(function (d) { return d.ConteoCompras; });
    var ComprasCero = conteoMaterias.every(function (conteo) { return conteo === 0; });

    if (ComprasCero)
    {
        document.getElementById('ComprasMatChart').style.display = 'none';
        document.getElementById('noDatos').style.display = 'block';
    }
    else
    {
        var coloresPredefinidos = [
            'rgba(150, 239, 255, 1.0)',
            'rgba(95, 189, 255, 1.0)',
            'rgba(114, 221, 217, 1)',
            'rgba(23, 107, 135, 1)',
            'rgba(166, 246, 241, 1)'
        ];

        var colores = materiasPrimas.map(function (_, index) {
            return coloresPredefinidos[index % coloresPredefinidos.length];
        });

        var ctx = document.getElementById('ComprasMatChart').getContext('2d');
        document.getElementById('ComprasMatChart').style.display = 'block';
        document.getElementById('noDatos').style.display = 'none';
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: materiasPrimas,
                datasets: [{
                    label: 'Compras',
                    data: conteoMaterias,
                    backgroundColor: colores,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                responsive: true,
                plugins: {
                    legend: {
                        display: false
                    }
                }
            }
        });
    }
});

// Editar proveedor
document.querySelectorAll('.edit-Proveedor').forEach(button => {
    button.addEventListener('click', function () {
        var proveedoresId = this.getAttribute('data-id');
        fetch(`/Proveedores/Edit/${proveedoresId}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editProveedorModal .modal-body').innerHTML = html;
                $('#editProveedorModal').modal('show');

                document.querySelector('#editProveedorModal #editProveedorForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/Proveedores/Edit/${proveedoresId}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editProveedorModal').modal('hide');
                            if (data.success) {
                                Swal.fire({
                                    title: '¡Éxito!',
                                    text: data.message,
                                    icon: 'success',
                                    confirmButtonColor: '#0DBCB5'
                                }).then(() => {
                                    window.location.reload();
                                });
                            } else {
                                Swal.fire({
                                    title: 'Error',
                                    text: data.message,
                                    icon: 'error',
                                    confirmButtonColor: '#0DBCB5'
                                });
                            }
                        })
                        .catch(error => {
                            $('#editProveedorModal').modal('hide');
                            Swal.fire({
                                title: 'Error',
                                text: 'Hubo un problema con la solicitud.',
                                icon: 'error',
                                confirmButtonColor: '#0DBCB5'
                            });
                        });
                });
            })
            .catch(error => console.error('Error:', error));
    });
});