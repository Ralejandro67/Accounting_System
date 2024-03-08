// Agregar Transacciones
$(document).ready(function () {
    $("#AddTransaccion").on("click", function () {
        $.ajax({
            url: '/DetallesTransacciones/GetDetalleTransaccionPartial',
            type: 'GET',
            success: function (result) {
                $('#newDetallesTModal .modal-body').html(result);
                $('#newDetallesTModal').modal('show');
                inicializarInputMonto();
                toggleRecurrenceFields();
            },
            error: function (error) {
                console.error("Error al cargar la vista parcial", error);
            }
        });
    });
});

function toggleRecurrenceFields() {
    var isChecked = $('#check').is(':checked');
    if (isChecked) {
        $('.recurrence-fields').show();
    } else {
        $('.recurrence-fields').hide();
        $('#FechaRecurrencia').val($('#FechaTrans').val());
        $('#Frecuencia').val('No Recurrente');
    }
}

$(document).on('change', '#check', function () {
    toggleRecurrenceFields();
});

function inicializarInputMonto() {
    $('#inputMonto').on('input', function () {
        var monto = $(this).val();
        if (monto === '') {
            $('#montoDisplayT').text('₡0.00');
        } else {
            $('#montoDisplayT').text('₡' + monto);
            $('#inputMontoTotalT').text('₡' + monto);
        }
    });
}

$(document).on('change', '#IdImpuesto', function () {
    var impuestoId = $(this).val();
    var monto = $('#inputMonto').val();
    $.ajax({
        url: '/DetallesTransacciones/GetTipoTransaccion/',
        type: 'GET',
        dataType: 'json',
        data: { IdImpuesto: impuestoId },
        success: function (data) {
            var tipotransaccionSelect = $('#IdTipo');
            tipotransaccionSelect.empty();
            tipotransaccionSelect.append($('<option></option>').val('').text('Seleccione un tipo de transacción'));
            $.each(data, function (index, item) {
                tipotransaccionSelect.append($('<option></option>').val(item.value).text(item.text));
            });
        },
        error: function () {
            alert('Error al cargar los tipos de transacción');
        }
    });
    if (impuestoId && monto) {
        $.ajax({
            url: '/DetallesTransacciones/GetImpuesto/',
            type: 'GET',
            dataType: 'json',
            data: {
                IdImpuesto: impuestoId,
                Monto: monto
            },
            success: function (data) {
                $('#inputMontoImpuestoT').text('₡' + data.montoImpuesto.toFixed(2));
                $('#inputMontoTotalT').text('₡' + data.montoTotal.toFixed(2));
                $('#MontoTotalT').val(data.montoTotal.toFixed(2));
            },
            error: function () {
                alert('Error al cargar la tasa del impuesto');
            }
        });
    }
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitDetallesForm') {
        var formData = new FormData(document.getElementById('detallesTForm'));

        fetch('/DetallesTransacciones/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#newDetallesTModal').modal('hide');
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
                    $('#newFacturaCModal').modal('hide');
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
            <div class="col-md-5">
                <div class="form-group">
                    <label>Platillo</label>
                    ${selectHtml}
                </div>
            </div>
            <div class="col-md-5">
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
                if (data.success) {
                    $('#newFacturaVentaModal').modal('hide');
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