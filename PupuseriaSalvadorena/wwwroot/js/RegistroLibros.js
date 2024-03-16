//Event Handler Propagacion
document.addEventListener('DOMContentLoaded', function () {
    var buttons = document.querySelectorAll('.botoneslibros button');
    buttons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    });
});

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
            Swal.fire({
                title: '¡Éxito!',
                text: data.message,
                icon: 'success',
                confirmButtonColor: '#0DBCB5'
            }).then((result) => {
                if (result.isConfirmed || result.isDismissed) {
                    $('#newLibroModal').modal('hide');
                    window.location.reload();
                }
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
        Swal.fire({
            title: 'Error',
            text: 'Hubo un problema con la solicitud.',
            icon: 'error',
            confirmButtonColor: '#0DBCB5'
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
                        if (data.success) {
                            Swal.fire({
                                title: '¡Éxito!',
                                text: data.message,
                                icon: 'success',
                                confirmButtonColor: '#0DBCB5'
                            }).then(() => {
                                $('#editLibroModal').modal('hide');
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
                        $('#editLibroModal').modal('hide');
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

// Eliminar Libro
document.querySelectorAll('.delete-libro').forEach(button => {
    button.addEventListener('click', function () {
        var IdRegistroLibros = this.getAttribute('data-id');

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#0DBCB5',
            cancelButtonColor: '#9DB2BF',
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