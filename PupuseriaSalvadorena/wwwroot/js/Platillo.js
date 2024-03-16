// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaPlatillos tbody');
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
    var rows = document.querySelectorAll('#tablaPlatillos tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaPlatillos th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaPlatillos');
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

            document.querySelectorAll('#tablaPlatillos th').forEach(th => {
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

// Agregar platillo
document.getElementById("AddPlatillo").addEventListener("click", function () {
    $.ajax({
        url: '/Platillos/Create',
        type: 'GET',
        success: function (result) {
            $('#newPlatilloModal .modal-body').html(result);
            $('#newPlatilloModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitPlatilloForm') {

        var ValueProd = document.getElementById('CostoProduccion').value;
        var ValuePrice = document.getElementById('PrecioVenta').value;

        var regex = /^[0-9]+(\.[0-9]+)?$/;
        if (!regex.test(ValueProd) || !regex.test(ValuePrice)) {
            Swal.fire({
                title: 'Error',
                text: 'El costo de produccion o el valor de venta del platillo es obligatorio.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (parseFloat(ValueProd) > parseFloat(ValuePrice)) {
            Swal.fire({
                title: 'Error',
                text: 'El costo de produccion no puede ser mayor al valor de venta.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (parseFloat(ValueProd) < 1 || parseFloat(ValuePrice) < 1) {
            Swal.fire({
                title: 'Error',
                text: 'El costo de produccion o el valor de venta no puede ser menor a 1.', 
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        var formData = new FormData(document.getElementById('PlatilloForm'));

        fetch('/Platillos/Create', {
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
                            $('#newPlatilloModal').modal('hide');
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
    }
});


// Editar platillo
document.querySelectorAll('.edit-platillo').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/Platillos/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editPlatilloModal .modal-body').innerHTML = html;
                $('#editPlatilloModal').modal('show');

                document.querySelector('#editPlatilloModal #editPlatilloForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var ValueProd = document.getElementById('produccion').value;
                    var ValuePrice = document.getElementById('precio').value;

                    var regex = /^[0-9]+(\.[0-9]+)?$/;
                    if (!regex.test(ValueProd) || !regex.test(ValuePrice)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El valor de produccion o el valor de venta tiene que ser un numero.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    if (parseFloat(ValueProd) > parseFloat(ValuePrice)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El valor de produccion no puede ser mayor al valor de venta.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    if (parseFloat(ValueProd) < 1 || parseFloat(ValuePrice) < 1) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El valor de produccion o el valor de venta no puede ser menor a 1.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    var formData = new FormData(this);

                    fetch(`/Platillos/Edit/${Id}`, {
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
                                    $('#editPlatilloModal').modal('hide');
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
                            $('#editPlatilloModal').modal('hide');
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

// Eliminar Tipo de Movimiento
document.querySelectorAll('.delete-platillo').forEach(button => {
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
                fetch(`/Platillos/Delete/${Id}`, {
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