// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaProveedores tbody');
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
    var rows = document.querySelectorAll('#tablaProveedores tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaProveedores th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaProveedores');
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

            document.querySelectorAll('#tablaProveedores th').forEach(th => {
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

// Agregar proveedor
document.getElementById("AddProveedor").addEventListener("click", function () {
    $("#newProveedorModal").modal("show");
});

document.getElementById('submitProveedor').addEventListener('click', function () {

    var telefono = document.getElementById('Telefono').value;

    var regex = /^[0-9]{8}$/;
    if (!regex.test(telefono)) {
        Swal.fire({
            title: 'Error',
            text: 'El número de teléfono debe contener 8 dígitos.',
            icon: 'warning', 
            confirmButtonColor: '#0DBCB5'
        })
        return;
    }

    var formData = new FormData(document.getElementById('ProveedorForm'));

    fetch('/Proveedores/Create', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: data.message,
                    icon: 'success',
                    confirmButtonColor: '#0DBCB5'
                }).then((result) => {
                    if (result.isConfirmed || result.isDismissed) {
                        $('#newProveedorModal').modal('hide');
                        window.location.reload();
                    }
                });
            } else {
                let errorMessage = "";
                if (data.message) {
                    errorMessage = data.message;
                } else if (data.errors && data.errors.length > 0) {
                    errorMessage = data.errors.join("\n");
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

                    var telefono = document.getElementById('TelefonoEdit').value;

                    var regex = /^[0-9]{8}$/;
                    if (!regex.test(telefono)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El número de teléfono debe contener 8 dígitos.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        })
                        return;
                    }

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
                            if (data.success) {
                                Swal.fire({
                                    title: '¡Éxito!',
                                    text: data.message,
                                    icon: 'success',
                                    confirmButtonColor: '#0DBCB5'
                                }).then(() => {
                                    $('#editProveedorModal').modal('hide');
                                    window.location.reload();
                                });
                            } else {
                                let errorMessage = "";
                                if (data.message) {
                                    errorMessage = data.message;
                                } else if (data.errors && data.errors.length > 0) {
                                    errorMessage = data.errors.join("\n");
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

// Eliminar proveedor
document.querySelectorAll('.delete-Proveedor').forEach(button => {
    button.addEventListener('click', function () {
        var proveedoresId = this.getAttribute('data-id');

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#0DBCB5',
            cancelButtonColor: '#9DB2BF',
            confirmButtonText: 'Sí, elimínalo!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/Proveedores/Delete/${proveedoresId}`, {
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