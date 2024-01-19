// Agregar impuesto
document.getElementById("AddImpuesto").addEventListener("click", function () {
    $("#newImpuestoModal").modal("show");
});

document.getElementById('submitCreateForm').addEventListener('click', function () {
    var formData = new FormData(document.getElementById('impuestoForm'));

    fetch('/Impuestos/Create', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            $('#newImpuestoModal').modal('hide');
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

// Editar impuesto
document.querySelectorAll('.edit-impuesto').forEach(button => {
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

// Eliminar impuesto
document.querySelectorAll('.delete-impuesto').forEach(button => {
    button.addEventListener('click', function () {
        var impuestoId = this.getAttribute('data-id');

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
                fetch(`/Impuestos/Delete/${impuestoId}`, {
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