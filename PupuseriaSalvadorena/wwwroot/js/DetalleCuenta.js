// Realizar Pago
document.querySelectorAll('.add-Pago').forEach(button => {
    button.addEventListener('click', function () {
        var IdCuentaPagar = this.getAttribute('data-idCuenta');
        fetch(`/DetalleCuentas/Create/${IdCuentaPagar}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#newDetalleCuentaModal .modal-body').innerHTML = html;
                $('#newDetalleCuentaModal').modal('show');
                document.querySelector('#newDetalleCuentaModal #DetalleCuentaForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/DetalleCuentas/Create`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#newDetalleCuentaModal').modal('hide');
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
                            $('#newDetalleCuentaModal').modal('hide');
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