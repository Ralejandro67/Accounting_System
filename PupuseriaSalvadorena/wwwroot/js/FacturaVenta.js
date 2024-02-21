
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
                        stepSize: 50000 
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
                Swal.fire({
                    title: 'Error',
                    text: data.message,
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