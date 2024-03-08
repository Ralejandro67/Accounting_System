//Grafico Linear
document.addEventListener('DOMContentLoaded', function () {
    var Meses = ingresos.map(function (x) { return x.Mes; });
    var MontoIngresos = ingresos.map(function (d) { return d.TotalMonto; });
    var MontoEgresos = egresos.map(function (d) { return d.TotalMonto; });

    var ctx = document.getElementById('IngresosEgresosChart').getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: Meses,
            datasets: [{
                label: 'Ingresos',
                data: MontoIngresos,
                backgroundColor: 'rgba(150, 239, 255, 0.5)',
                borderColor: 'rgba(150, 239, 255, 1)', 
                borderWidth: 2,
                tension: 0.4,
                fill: false 
            },
            {
                label: 'Egresos',
                data: MontoEgresos,
                backgroundColor: 'rgba(95, 189, 255, 0.5)', 
                borderColor: 'rgba(95, 189, 255, 1)', 
                borderWidth: 2,
                tension: 0.4,
                fill: false 
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
}); 

// Recurrencia Transacciones
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

// Agregar Transacciones
document.getElementById("AddTransaccion").addEventListener("click", function () {
    $.ajax({
        url: '/DetallesTransacciones/GetDetalleTransaccionPartial', 
        type: 'GET',
        success: function (result) {
            $('#newDetallesTModal .modal-body').html(result);
            inicializarInputMonto();
            toggleRecurrenceFields();
            $('#newDetallesTModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitDetallesForm') {

        var ValueMonto = document.getElementById('inputMonto').value;
        var ValueCant = document.getElementById('Cantidad').value;

        var regex = /^[0-9]+(\.[0-9]+)?$/;
        if (!regex.test(ValueMonto) || !regex.test(ValueCant)) {
            Swal.fire({
                title: 'Error',
                text: 'La valor de la transacción o la cantidad debe ser un número.',
                icon: 'error',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (parseFloat(ValueMonto) < 1 || parseFloat(ValueMonto) < 1) {
            Swal.fire({
                title: 'Error',
                text: 'La valor de la transacción o la cantidad debe ser mayor a 0.',
                icon: 'error',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

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
});

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
            tipotransaccionSelect.append($('<option></option>').val('').text('Tipo de transacción'));
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

// Editar Transacciones
document.querySelectorAll('.edit-transaccion').forEach(button => {
    button.addEventListener('click', function () {
        var transaccionId = this.getAttribute('data-id');
        fetch(`/DetallesTransacciones/Edit/${transaccionId}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editDetallesTModal .modal-body').innerHTML = html;
                inicializarInputMontoEditar();
                $('#editDetallesTModal').modal('show');
                document.querySelector('#editDetallesTModal #editdetallesTForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/DetallesTransacciones/Edit/${transaccionId}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                    .then(response => response.json())
                    .then(data => {
                        $('#editDetallesTModal').modal('hide');
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
                                icon: 'error',
                                confirmButtonColor: '#0DBCB5'
                            });
                        }
                    })
                    .catch(error => {
                        $('#editDetallesTModal').modal('hide');
                        Swal.fire({
                            title: 'Error',
                            text: 'Hubo un problema con la solicitud.',
                            icon: 'error',
                            confirmButtonColor: '#0DBCB5'
                        });
                    });
                });
            })
    });
});

function inicializarInputMontoEditar() {
    $('#inputMontoE').on('input', function () {
        var monto = $(this).val();
        var impuestoId = $('#IdImpuestoE').val();
        if (monto === '') {
            $('#montoDisplayE').text('₡0.00');
        } else {
            $.ajax({
                url: '/DetallesTransacciones/GetImpuesto/',
                type: 'GET',
                dataType: 'json',
                data: {
                    IdImpuesto: impuestoId,
                    Monto: monto
                },
                success: function (data) {
                    $('#montoDisplayE').text('₡' + monto);
                    $('#inputMontoImpuestoE').text('₡' + data.montoImpuesto.toFixed(2));
                    $('#inputMontoTotalE').text('₡' + data.montoTotal.toFixed(2));
                    $('#MontoTotalE').val(data.montoTotal.toFixed(2));
                },
                error: function () {
                    alert('Error al cargar la tasa del impuesto');
                }
            });
        }
    });
}

$(document).on('change', '#IdImpuestoE', function () {
    var impuestoId = $(this).val();
    var monto = $('#montoDisplayE').text();
    var valor = monto.replace('₡', '');
    $.ajax({
        url: '/DetallesTransacciones/GetTipoTransaccion/',
        type: 'GET',
        dataType: 'json',
        data: { IdImpuesto: impuestoId },
        success: function (data) {
            var tipotransaccionSelect = $('#IdTipoE');
            tipotransaccionSelect.empty();
            tipotransaccionSelect.append($('<option></option>').val('').text('Tipo de transacción'));
            $.each(data, function (index, item) {
                tipotransaccionSelect.append($('<option></option>').val(item.value).text(item.text));
            });
        },
        error: function () {
            alert('Error al cargar los tipos de transacción');
        }
    });
    if (impuestoId && valor) {
        $.ajax({
            url: '/DetallesTransacciones/GetImpuesto/',
            type: 'GET',
            dataType: 'json',
            data: {
                IdImpuesto: impuestoId,
                Monto: valor
            },
            success: function (data) {
                $('#inputMontoImpuestoE').text('₡' + data.montoImpuesto.toFixed(2));
                $('#inputMontoTotalE').text('₡' + data.montoTotal.toFixed(2));
                $('#MontoTotalE').val(data.montoTotal.toFixed(2));
            },
            error: function () {
                alert('Error al cargar la tasa del impuesto');
            }
        });
    }
});

// Eliminar Transacciones
document.querySelectorAll('.delete-transaccion').forEach(button => {
    button.addEventListener('click', function () {
        var transaccionId = this.getAttribute('data-id');

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
                fetch(`/DetallesTransacciones/Delete/${transaccionId}`, {
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
                                text: 'La transacción ha sido eliminada.',
                                icon: 'success',
                                confirmButtonColor: '#0DBCB5'
                            }).then(() => {
                                window.location.reload();
                            });
                        } else {
                            Swal.fire({
                                title: 'Error',
                                text: 'Hubo un problema al eliminar la transacción.',
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