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
        $('#campoProveedor').show();
    } else {
        $('#campoFechaVencimiento').hide();
        $('#campoProveedor').hide();
    }
}

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitFacturaC') {

        console.log("Valor de IdProveedor:", document.getElementById('IdProveedor').value);
        console.log("Valor de FechaVencimiento:", document.getElementById('FechaVencimiento').value);

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

// Editar Factura Compra
document.querySelectorAll('.edit-Factura').forEach(button => {
    button.addEventListener('click', function () {
        var impuestoId = this.getAttribute('data-id');
        fetch(`/Impuestos/Edit/${impuestoId}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editImpuestoModal .modal-body').innerHTML = html;
                $('#editImpuestoModal').modal('show');
                document.querySelector('#editImpuestoModal #editImpuestoForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/Impuestos/Edit/${impuestoId}`, {
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
                            $('#editImpuestoModal').modal('hide');
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