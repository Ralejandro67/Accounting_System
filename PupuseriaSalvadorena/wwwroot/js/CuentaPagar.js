// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaCuentasPagar tbody');
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
    var rows = document.querySelectorAll('#tablaCuentasPagar tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaCuentasPagar th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaCuentasPagar');
            var tableBody = table.querySelector('tbody');
            var rowsArray = Array.from(tableBody.rows);
            var column = this.cellIndex;
            var isAscending = header.classList.contains('asc');
            var isDateColumn = this.id === 'FechaInicio';
            var isDateColumnFinal = this.id === 'FechaFinal';

            rowsArray.sort(function (rowA, rowB) {
                var textA = rowA.cells[column].textContent.trim();
                var textB = rowB.cells[column].textContent.trim();

                if (isDateColumn || isDateColumnFinal) {
                    var dateA = new Date(textA.split('/').reverse().join('-'));
                    var dateB = new Date(textB.split('/').reverse().join('-'));
                    return (dateA - dateB) * (isAscending ? -1 : 1);
                } else {
                    return textA.localeCompare(textB, undefined, { numeric: true }) * (isAscending ? -1 : 1);
                }
            });

            document.querySelectorAll('#tablaCuentasPagar th').forEach(th => {
                th.innerHTML = th.textContent;
                th.classList.remove('asc', 'desc');
            });

            var newIconHtml = isAscending ? '<i class="fa fa-arrow-down"></i>' : '<i class="fa fa-arrow-up"></i>';
            this.innerHTML += ' ' + newIconHtml;
            this.classList.toggle('asc', !isAscending);
            this.classList.toggle('desc', isAscending);

            rowsArray.forEach(row => tableBody.appendChild(row));
        });
    }
});

// Agregar Factura Compra
document.getElementById("AddFacturaC").addEventListener("click", function () {
    $.ajax({
        url: '/FacturaCompras/Create',
        type: 'GET',
        success: function (result) {
            $('#newFacturaCModal .modal-body').html(result);
            toggleCamposActivos();
            $('#newFacturaCModal').modal('show');

            var tooltipTriggerList = [].slice.call(document.querySelectorAll('#newFacturaCModal [data-bs-toggle="tooltip"]'));
            var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

$(document).on('change', '[name="EstadoFactura"]', function () {
    toggleCamposActivos();
});

function toggleCamposActivos() {
    var isPorPagarSelected = $('#FacturaPorPagar').is(':checked');
    $('#campoFechaVencimiento').toggle(isPorPagarSelected);
    $('#cuentaPorPagar').val(isPorPagarSelected ? 'true' : 'false');
}

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitFacturaC') {

        var ValueCant = document.getElementById('Cant').value;
        var ValueTotal = document.getElementById('TotalCompra').value;
        var ValuePeso = document.getElementById('Peso').value;

        var regex = /^[0-9]+(\.[0-9]+)?$/;
        if (!regex.test(ValueCant) || !regex.test(ValueTotal) || !regex.test(ValuePeso)) {
            Swal.fire({
                title: 'Error',
                text: 'El total de la compra, la cantidad del producto o el peso debe ser numerico.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (parseFloat(ValueCant) < 1 || parseFloat(ValueTotal) < 1 || parseFloat(ValuePeso) < 1) {
            Swal.fire({
                title: 'Error',
                text: 'Debes agregar un monto a el valor del producto, la cantidad, el peso.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        var formData = new FormData(document.getElementById('FacturaCForm'));

        fetch('/FacturaCompras/Create', {
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
                            $('#newFacturaCModal').modal('hide');
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

$(document).on('change', '#IdProveedor', function () {
    var idProveedor = $(this).val();
    $.ajax({
        url: '/FacturaCompras/GetMateriasPrimas',
        type: 'GET',
        dataType: 'json',
        data: { idProveedor: idProveedor },
        success: function (data) {
            var materiasPrimaSelect = $('#IdMateriaPrima');
            materiasPrimaSelect.empty();
            materiasPrimaSelect.append($('<option></option>').val('').text('Materias Primas'));
            $.each(data, function (index, item) {
                materiasPrimaSelect.append($('<option></option>').val(item.value).text(item.text));
            });
        },
        error: function () {
            alert('Error al cargar las materias primas');
        }
    });
});

// Editar Cuenta por Pagar
document.querySelectorAll('.edit-CuentaPagar').forEach(button => {
    button.addEventListener('click', function () {
        var IdCuentaPagar = this.getAttribute('data-id');
        fetch(`/CuentaPagars/Edit/${IdCuentaPagar}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editCuentaPagarModal .modal-body').innerHTML = html;
                $('#editCuentaPagarModal').modal('show');
                document.querySelector('#editCuentaPagarModal #editCuentaPagarForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/CuentaPagars/Edit/${IdCuentaPagar}`, {
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
                                    $('#editCuentaPagarModal').modal('hide');
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
                            $('#editCuentaPagarModal').modal('hide');
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

// Eliminar Cuenta por Pagar
document.querySelectorAll('.delete-CuentaPagar').forEach(button => {
    button.addEventListener('click', function () {
        var IdCuentaPagar = this.getAttribute('data-id');

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
                fetch(`/CuentaPagars/Delete/${IdCuentaPagar}`, {
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