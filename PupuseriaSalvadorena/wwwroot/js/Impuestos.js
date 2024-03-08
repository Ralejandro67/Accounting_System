// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaImpuestos tbody');
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
document.getElementById('busquedaImpuestos').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var rows = document.querySelectorAll('#tablaImpuestos tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaImpuestos th').forEach(header => {
    if (!header.id.includes("acciones")) { 
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaImpuestos');
            var tableBody = table.querySelector('tbody');
            var rowsArray = Array.from(tableBody.rows);
            var column = this.cellIndex;
            var isAscending = header.classList.contains('asc');

            rowsArray.sort(function (rowA, rowB) {
                var textA = rowA.cells[column].textContent.trim();
                var textB = rowB.cells[column].textContent.trim();
                return textA.localeCompare(textB, undefined, { numeric: true }) * (isAscending ? -1 : 1);
            });

            document.querySelectorAll('#tablaImpuestos th').forEach(th => {
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

// Agregar impuesto
document.getElementById("AddImpuesto").addEventListener("click", function () {
    $.ajax({
        url: '/Impuestos/Create',
        type: 'GET',
        success: function (result) {
            $('#newImpuestoModal .modal-body').html(result);
            $('#newImpuestoModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitCreateForm') {

        var tasaValue = document.getElementById('Tasa').value;

        var regex = /^[0-9]+(\.[0-9]+)?$/;
        if (!regex.test(tasaValue)) {
            Swal.fire({
                title: 'Error',
                text: 'La tasa del impuesto debe ser un número.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (parseFloat(tasaValue) < 0) {
            Swal.fire({
                title: 'Error',
                text: 'La tasa del impuesto no puede ser un número negativo.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        var formData = new FormData(document.getElementById('impuestoForm'));

        fetch('/Impuestos/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#newImpuestoModal').modal('hide');
                    Swal.fire({
                        title: '¡Éxito!',
                        text: data.message,
                        icon: 'success',
                        confirmButtonColor: '#0DBCB5'
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
                    icon: 'error',
                    confirmButtonColor: '#0DBCB5'
                });
            });
    }
});

// Editar impuesto
document.querySelectorAll('.edit-impuesto').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/Impuestos/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editImpuestoModal .modal-body').innerHTML = html;
                $('#editImpuestoModal').modal('show');

                document.querySelector('#editImpuestoModal #editImpuestoForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var tasaValue = document.getElementById('Value').value;

                    var regex = /^[0-9]+(\.[0-9]+)?$/;
                    if (!regex.test(tasaValue) || parseFloat(tasaValue) < 0) {
                        Swal.fire({
                            title: 'Error',
                            text: 'La tasa del impuesto debe ser un número.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    var formData = new FormData(this);

                    fetch(`/Impuestos/Edit/${Id}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editImpuestoModal').modal('hide');
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
                            $('#editImpuestoModal').modal('hide');
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

// Eliminar impuesto
document.querySelectorAll('.delete-impuesto').forEach(button => {
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
                fetch(`/Impuestos/Delete/${Id}`, {
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
                                text: 'El impuesto ha sido eliminado.',
                                icon: 'success',
                                confirmButtonColor: '#0DBCB5'
                            }).then(() => {
                                window.location.reload();
                            });
                        } else {
                            Swal.fire({
                                title: 'Error',
                                text: 'Hubo un problema al eliminar el impuesto.',
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