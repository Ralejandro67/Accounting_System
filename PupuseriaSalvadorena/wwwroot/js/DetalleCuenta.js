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

                    var Value = document.getElementById('Pago').value;

                    var regex = /^[0-9]+(\.[0-9]+)?$/;
                    if (!regex.test(Value)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El pago tiene que ser un valor numerico.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    if (parseFloat(Value) < 1) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El pago tiene que ser mayor a 0.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

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
                            if (data.success) {
                                Swal.fire({
                                    title: '¡Éxito!',
                                    text: data.message,
                                    icon: 'success',
                                    confirmButtonColor: '#0DBCB5'
                                }).then(() => {
                                    $('#newDetalleCuentaModal').modal('hide');
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
                            $('#newDetalleCuentaModal').modal('hide');
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

// Editar pago
document.querySelectorAll('.edit-DetalleCuenta').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/DetalleCuentas/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editDetalleCuentaModal .modal-body').innerHTML = html;
                $('#editDetalleCuentaModal').modal('show');

                document.querySelector('#editDetalleCuentaModal #editDetalleCuentaForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var Value = document.getElementById('PagoC').value;

                    var regex = /^[0-9]+(\.[0-9]+)?$/;
                    if (!regex.test(Value)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El pago tiene que ser un valor numerico.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    if (parseFloat(Value) < 1) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El pago tiene que ser mayor a 0.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    var formData = new FormData(this);

                    fetch(`/DetalleCuentas/Edit/${Id}`, {
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
                                    $('#editDetalleCuentaModal').modal('hide');
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
                            $('#editDetalleCuentaModal').modal('hide');
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

// Eliminar pago
document.querySelectorAll('.delete-DetalleCuenta').forEach(button => {
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
                fetch(`/DetalleCuentas/Delete/${Id}`, {
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