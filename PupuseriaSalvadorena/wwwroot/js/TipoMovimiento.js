// Agregar Tipo de Movimiento
document.getElementById("AddTipoM").addEventListener("click", function () {
    $("#newTipoMovimientoModal").modal("show");
});

document.getElementById('submitTipoMovimiento').addEventListener('click', function () {
    var formData = new FormData(document.getElementById('TipoMovimientoForm'));

    fetch('/TipoMovimientos/Create', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                $('#newTipoMovimientoModal').modal('hide');
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
});

// Editar Tipo de Movimiento
document.querySelectorAll('.edit-TipoM').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/TipoMovimientos/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#newTipoMovimientoModal .modal-body').innerHTML = html;
                $('#newTipoMovimientoModal').modal('show');

                document.querySelector('#newTipoMovimientoModal #editTipoMovimientoForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/TipoMovimientos/Edit/${Id}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#newTipoMovimientoModal').modal('hide');
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
                            $('#newTipoMovimientoModal').modal('hide');
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

// Eliminar Tipo de Movimiento
document.querySelectorAll('.delete-TipoM').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');

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
                fetch(`/TipoMovimientos/Delete/${Id}`, {
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