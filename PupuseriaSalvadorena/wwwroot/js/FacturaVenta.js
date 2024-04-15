
//Grafico Barras
document.addEventListener('DOMContentLoaded', function () {
    var ctx = document.getElementById('FacturasChart').getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: Meses,
            datasets: [{
                label: 'Facturas de Venta',
                data: ventasPorMes,
                backgroundColor: ['rgba(114, 221, 217, 1)'],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 500000 
                    }
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
}); 

// Reporte de Facturas
$(document).off('click', '#NewReporte').on('click', '#NewReporte', function () {
    var idReporte = $(this).data('idreporte');
    $('input[name="TipoReporte"]').val(idReporte); 
    $('#ReporteModal').modal('show');
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitReporteForm') {

        var formData = new FormData(document.getElementById('ReporteForm'));

        fetch('/FacturaVentas/ReporteFacturas', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
        .then(response => {
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.indexOf("application/json") !== -1) {
                return response.json().then(data => {
                    Swal.fire({
                        title: 'Error',
                        text: data.message,
                        icon: 'error'
                    });
                });
            } else if (contentType && contentType.indexOf("application/pdf") !== -1 || contentType.indexOf("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") !== -1) {
                response.blob().then(blob => {
                    var url = window.URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = response.headers.get("content-disposition").split('filename=')[1].replaceAll('"', '');
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                });
            } else {
                Swal.fire({
                    title: 'Error',
                    text: 'Formato de respuesta desconocido.',
                    icon: 'error'
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

// Agregar Factura de Venta
$(document).ready(function () {
    $("#AddFacturaVenta").on("click", function () {
        $.ajax({
            url: '/FacturaVentas/Create',
            type: 'GET',
            success: function (result) {
                $('#newFacturaVentaModal .modal-body').html(result);
                $('#newFacturaVentaModal').modal('show');
                rebindEvents();
                actualizarEstadoFacturaE();
            },
            error: function (error) {
                console.error("Error al cargar la vista parcial", error);
            }
        });
    });
});

function rebindEvents() {
    $(document).on('change', '[name="TipoFactura"]', function () {
        actualizarEstadoFacturaE();
    });

    $(document).off('click', '#agregarPlatillo').on('click', '#agregarPlatillo', function () {
        agregarNuevoPlatillo();
        actualizarOpcionesPlatillos();
    });

    $(document).off('click', '.eliminarPlatillo').on('click', '.eliminarPlatillo', function () {
        $(this).closest('.platilloRow').remove();
        actualizarCalculos();
        actualizarOpcionesPlatillos();
    });

    $(document).off('change', '.platilloSelect, .platilloRow input[type="number"]').on('change', '.platilloSelect, .platilloRow input[type="number"]', function () {
        actualizarCalculos();
        actualizarOpcionesPlatillos();
    });
}

function actualizarEstadoFacturaE() {
    var isElectronicaSelected = $('#FacturaElectronica').is(':checked');
    $('#CamposFacturasE').toggle(isElectronicaSelected);
    $('#FacturaE').val(isElectronicaSelected ? 'true' : 'false');

    if (isElectronicaSelected) {
        $('#idCedula').val('CF');
        $('#Identificacion').val('');
        $('#Telefono').val('');
        $('#Cliente').val('');
        $('#Correo').val('');
    } else {
        $('#idCedula').val('');
        $('#Identificacion').val('0');
        $('#Telefono').val('00000000');
        $('#Cliente').val('Consumidor final');
        $('#Correo').val('example@gmail.com');
    }
}

function agregarNuevoPlatillo() {
    var selectHtml = '<select class="form-control platilloSelect" name="IdPlatillo[]">' +
        '<option value="">Seleccione un platillo</option>';
    platillosData.forEach(function (platillo) {
        selectHtml += '<option value="' + platillo.idPlatillo +
            '" data-precioventa="' + platillo.precioVenta + '">' +
            platillo.nombrePlatillo + '</option>';
    });
    selectHtml += '</select>';

    var nuevoPlatilloHtml = `
        <div class="row platilloRow">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Platillo</label>
                    ${selectHtml}
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label>Cantidad</label>
                    <input type="number" class="form-control" name="CantVenta[]" min="1" value="1"/>
                </div>
            </div>
            <div class="col-md-2">
                <button type="button" class="btn btn-danger eliminarPlatillo">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">
                        <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5M8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5m3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0" />
                    </svg>
                </button>
            </div>
        </div>`;

    $('#platillosContenedor').append(nuevoPlatilloHtml);
    actualizarOpcionesPlatillos();
}

function actualizarCalculos() {
    var subtotal = 0;
    $('.platilloRow').each(function () {
        var precio = $(this).find('.platilloSelect option:selected').data('precioventa');
        var cantidad = $(this).find('input[name="CantVenta[]"]').val();
        subtotal += (precio * cantidad) || 0;
    });

    var iva = subtotal * 0.13;
    var total = subtotal + iva;

    $('#montoDisplay').text(`₡${subtotal.toFixed(2)}`);
    $('#inputMontoImpuesto').text(`₡${iva.toFixed(2)}`);
    $('#inputMontoTotal').text(`₡${total.toFixed(2)}`);
    $('#MontoSubTotal').val(subtotal.toFixed(2));
    $('#MontoTotal').val(total.toFixed(2));
}

function actualizarOpcionesPlatillos() {
    var seleccionados = $('.platilloSelect').map(function () { return $(this).val(); }).get();
    $('.platilloSelect').each(function () {
        var selectActual = this;
        var valorActual = $(this).val();
        $(this).find('option').each(function () {
            if (seleccionados.includes($(this).val()) && $(this).val() !== valorActual) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });
}

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitFacturaVentaForm') {
        e.preventDefault();


        var platillos = document.getElementById('platillosContenedor').querySelectorAll('.platilloRow');
        var tipoCedula = document.getElementById('idCedula').value;
        var cedula = document.getElementById('Identificacion').value;
        var telefono = document.getElementById('Telefono').value;

        var regexCedulaCF = /^[0-9]{9}$/;
        var regexCedulaDIMEX = /^[0-9]{12}$/;
        var regexCedulaNITE = /^[0-9]{10}$/;
        var regexTel = /^[0-9]{8}$/;

        if (tipoCedula === 'CF') {
            if (!regexCedulaCF.test(cedula)) {
                Swal.fire({
                    title: 'Error',
                    text: 'La cédula debe contener 9 dígitos.',
                    icon: 'warning',
                    confirmButtonColor: '#0DBCB5'
                })
                return;
            }
        } else if (tipoCedula === 'DIMEX') {
            if (!regexCedulaDIMEX.test(cedula)) {
                Swal.fire({
                    title: 'Error',
                    text: 'La cédula debe contener 12 dígitos.',
                    icon: 'warning',
                    confirmButtonColor: '#0DBCB5'
                })
                return;
            }
        } else if (tipoCedula === 'NITE' || tipoCedula === 'CJ') {
            if (!regexCedulaNITE.test(cedula)) {
                Swal.fire({
                    title: 'Error',
                    text: 'La cédula debe contener 10 dígitos.',
                    icon: 'warning',
                    confirmButtonColor: '#0DBCB5'
                })
                return;
            }
        }

        if (!regexTel.test(telefono)) {
            Swal.fire({
                title: 'Error',
                text: 'El número de teléfono debe contener 8 dígitos.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            })
            return;
        }

        if (platillos.length === 0) {
            Swal.fire({
                title: '¡Error!',
                text: 'Debe agregar al menos un platillo a la factura',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        $('.loading').show();
        $('button').prop('disabled', true);

        var formData = new FormData(document.getElementById('FacturaVentaForm'));

        fetch('/FacturaVentas/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
        .then(response => response.json())
        .then(data => {
            $('.loading').hide();
            $('button').prop('disabled', false);

            if (data.success) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: data.message,
                    icon: 'success',
                    confirmButtonColor: '#0DBCB5'
                }).then((result) => {
                    if (result.isConfirmed || result.isDismissed) {
                        $('#newFacturaVentaModal').modal('hide');
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
            $('.loading').hide();
            $('button').prop('disabled', false);
            Swal.fire({
                title: 'Error',
                text: 'Hubo un problema con la solicitud.',
                icon: 'error',
                confirmButtonColor: '#0DBCB5'
            });
        });
    }
});

// Imprimir factura

document.querySelectorAll('.print-FacturaVenta').forEach(button => {
    button.addEventListener('click', function () {
        $('.loading').show();
        $('button').prop('disabled', true);

        var id = this.getAttribute('data-id');
        fetch(`/FacturaVentas/ImprimirFactura/${id}`, {
            method: 'GET'
        })
        .then(response => response.json())
            .then(data => {
                $('.loading').hide();
                $('button').prop('disabled', false);
            if (data.success) {
                window.open(data.url, '_blank');
            } else {
                let errorMessage = "Error al imprimir la factura.";
                if (data.message) {
                    errorMessage = data.message;
                }

                Swal.fire({
                    title: 'Error',
                    text: errorMessage,
                    icon: 'warning',
                    confirmButtonColor: '#0DBCB5'
                });
            }
        })
        .catch(error => console.error('Error:', error));
    });
});

// Anular factura
document.querySelectorAll('.delete-FacturaVenta').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡Si anulas la factura no podrás revertir este cambio!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#0DBCB5',
            cancelButtonColor: '#9DB2BF',
            confirmButtonText: 'Sí, proceder!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $('.loading').show();
                $('button').prop('disabled', true);
                fetch(`/FacturaVentas/Delete/${Id}`, {
                    method: 'POST'
                })
                    .then(response => response.json())
                    .then(data => {
                        $('.loading').hide();
                        $('button').prop('disabled', false);
                        if (data.success) {
                            Swal.fire({
                                title: '¡Anulada!',
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
                        $('.loading').hide();
                        $('button').prop('disabled', false);
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