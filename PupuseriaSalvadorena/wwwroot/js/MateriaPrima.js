// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaMateriasPrimas tbody');
    var rows = table.rows;
    var totalRows = rows.length;
    var totalPages = Math.ceil(totalRows / rowsPerPage);

    function showPage(page) {
        for (let i = 0; i < totalRows; i++) {
            rows[i].style.display = (i >= (page - 1) * rowsPerPage && i < page * rowsPerPage) ? "" : "none";
        }

        document.getElementById('pageIndicator').textContent = `${page} / ${totalPages}`;
    }

    window.changePage = function (direction) {
        currentPage += direction;

        if (currentPage < 1) currentPage = 1;
        if (currentPage > totalPages) currentPage = totalPages;

        showPage(currentPage);
    };

    showPage(currentPage);
}

document.addEventListener('DOMContentLoaded', setupPagination);

// Busqueda
document.getElementById('busqueda').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var rows = document.querySelectorAll('#tablaMateriasPrimas tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaMateriasPrimas th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaMateriasPrimas');
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

            document.querySelectorAll('#tablaMateriasPrimas th').forEach(th => {
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

// Agregar Materia Prima
document.getElementById("AddMateriaPrima").addEventListener("click", function () {
    $.ajax({
        url: '/MateriaPrimas/Create',
        type: 'GET',
        success: function (result) {
            $('#newMateriaPrimaModal .modal-body').html(result);
            $('#newMateriaPrimaModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitMateriaPrima') {

        var proveedor = document.getElementById('ProveedorId').value;

        if (proveedor === "") {
            Swal.fire({
                title: 'Error',
                text: 'Debe seleccionar un proveedor.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        var formData = new FormData(document.getElementById('MateriaPrimaForm'));

        fetch('/MateriaPrimas/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                $('#newMateriaPrimaModal').modal('hide');
                Swal.fire({
                    title: '¡Éxito!',
                    text: data.message,
                    icon: 'success'
                }).then((result) => {
                    if (result.isConfirmed || result.isDismissed) {
                        window.location.reload();
                    }
                });
            } else {
                let errorMessage = "";
                if (data.errors && data.errors.length > 0) {
                    errorMessage += "\n" + data.errors.join("\n");
                }

                Swal.fire({
                    title: 'Error',
                    text: errorMessage,
                    icon: 'warning',
                    confirmButtonColor: '#0DBCB5'
                });
            }
        })
        .catch(error => {
            Swal.fire({
                title: 'Error',
                text: 'Hubo un problema con la solicitud.',
                icon: 'error'
            });
        });
    }
});

// Editar materia prima
document.querySelectorAll('.edit-Materia').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/MateriaPrimas/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editMateriaPrimaModal .modal-body').innerHTML = html;
                $('#editMateriaPrimaModal').modal('show');

                document.querySelector('#editMateriaPrimaModal #editMateriaPrimaForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/MateriaPrimas/Edit/${Id}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editMateriaPrimaModal').modal('hide');
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
                                    icon: 'warning',
                                    confirmButtonColor: '#0DBCB5'
                                });
                            }
                        })
                        .catch(error => {
                            $('#editMateriaPrimaModal').modal('hide');
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

// Eliminar materia prima
document.querySelectorAll('.delete-Materia').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡No podrás revertir este cambio!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#0DBCB5',
            cancelButtonColor: '#9DB2BF',
            confirmButtonText: 'Sí, elimínalo!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/MateriaPrimas/Delete/${Id}`, {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                    }
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            Swal.fire({
                                title: '¡Eliminado!',
                                text: 'La materia prima ha sido eliminada.',
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
                        Swal.fire({
                            title: 'Error',
                            text: 'Hubo un problema con la solicitud.',
                            icon: 'error',
                            confirmButtonColor: '#0DBCB5'
                        });
                    });
            }
        })
    });
});