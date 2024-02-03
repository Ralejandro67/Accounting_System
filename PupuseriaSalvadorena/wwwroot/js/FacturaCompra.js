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

$(document).on('change', '#checkbox', function () {
    toggleCamposActivos();
});

function toggleCamposActivos() {
    if ($('#checkbox').is(':checked')) {
        $('#campoFechaVencimiento').show();
    } else {
        $('#campoFechaVencimiento').hide();
    }
}

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitFacturaC') {

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
            materiasPrimaSelect.append($('<option></option>').val('').text('Seleccione una materia prima'));
            $.each(data, function (index, item) {
                materiasPrimaSelect.append($('<option></option>').val(item.value).text(item.text));
            });
        },
        error: function () {
            alert('Error al cargar las materias primas');
        }
    });
});

// Editar Factura Compra
document.querySelectorAll('.edit-Factura').forEach(button => {
    button.addEventListener('click', function () {
        var facturaId = this.getAttribute('data-id');
        fetch(`/FacturaCompras/Edit/${facturaId}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editFacturaCModal .modal-body').innerHTML = html;
                $('#editFacturaCModal').modal('show');

                document.querySelector('#editFacturaCModal #editFacturaCompraForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/FacturaCompras/Edit/${facturaId}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editFacturaCModal').modal('hide');
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
                            $('#editFacturaCModal').modal('hide');
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

// Eliminar Factura Compra
document.querySelectorAll('.delete-Factura').forEach(button => {
    button.addEventListener('click', function () {
        var IdFactura = this.getAttribute('data-id');

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
                fetch(`/FacturaCompras/Delete/${IdFactura}`, {
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