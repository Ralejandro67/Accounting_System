// Recurrencia Transacciones
function toggleRecurrenceFields() {
    var isChecked = $('#Recurrencia').is(':checked');
    if (isChecked) {
        $('.recurrence-fields').show();
    } else {
        $('.recurrence-fields').hide();
        $('#FechaRecurrencia').val($('#FechaTrans').val());
        $('#Frecuencia').val('No Recurrente');
    }
}

$(document).on('change', '#Recurrencia', function () {
    toggleRecurrenceFields();
});

function inicializarInputMonto() {
    $('#inputMonto').on('input', function () {
        var monto = $(this).val();
        if (monto === '') {
            $('#montoDisplay').text('₡0.00');
        } else {
            $('#montoDisplay').text('₡' + monto);
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
            toggleRecurrenceFields();
            inicializarInputMonto();
            $('#newDetallesTModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
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
                $('#inputMontoImpuesto').text('₡' + data.montoImpuesto.toFixed(2));
                $('#inputMontoTotal').text('₡' + data.montoTotal.toFixed(2));
                $('#MontoTotal').val(data.montoTotal.toFixed(2));
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
                                icon: 'success'
                            }).then(() => {
                                window.location.reload();
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
                        $('#editDetallesTModal').modal('hide');
                        Swal.fire({
                            title: 'Error',
                            text: 'Hubo un problema con la solicitud.',
                            icon: 'error'
                        });
                    });
                });
            })
            .catch(error => console.error('Error:', error));
    });
});

// Eliminar Transacciones
document.querySelectorAll('.delete-transaccion').forEach(button => {
    button.addEventListener('click', function () {
        var transaccionId = this.getAttribute('data-id');

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
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
                            Swal.fire(
                                '¡Eliminado!',
                                'El impuesto ha sido eliminado.',
                                'success'
                            ).then(() => {
                                window.location.reload();
                            });
                        } else {
                            Swal.fire(
                                'Error',
                                'Hubo un problema al eliminar el impuesto.',
                                'error'
                            );
                        }
                    })
                    .catch(error => {
                        Swal.fire(
                            'Error',
                            'Hubo un problema con la solicitud.',
                            'error'
                        );
                    });
            }
        })
    });
});