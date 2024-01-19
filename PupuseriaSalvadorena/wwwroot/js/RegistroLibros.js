// Agregar Libro
document.getElementById("AddLibro").addEventListener("click", function () {
    $("#newLibroModal").modal("show");
});

document.getElementById('submitLibroForm').addEventListener('click', function () {
    var formData = new FormData(document.getElementById('LibroForm'));

    fetch('/RegistroLibroes/Create', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            $('#newLibroModal').modal('hide');
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

// Editar Libro
document.querySelectorAll('.edit-libro').forEach(button => {
    button.addEventListener('click', function () {
        var IdRegistroLibros = this.getAttribute('data-id');
        fetch(`/RegistroLibroes/Edit/${IdRegistroLibros}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editLibroModal .modal-body').innerHTML = html;
                $('#editLibroModal').modal('show');
                document.querySelector('#editLibroModal #editLibroForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/RegistroLibroes/Edit/${IdRegistroLibros}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                    .then(response => response.json())
                    .then(data => {
                        $('#editLibroModal').modal('hide');
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
                        $('#editLibroModal').modal('hide');
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

// Eliminar Libro
document.querySelectorAll('.delete-libro').forEach(button => {
    button.addEventListener('click', function () {
        var IdRegistroLibros = this.getAttribute('data-id');

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
                fetch(`/RegistroLibroes/Delete/${IdRegistroLibros}`, {
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