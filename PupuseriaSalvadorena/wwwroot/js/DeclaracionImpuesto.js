//Event Handler Propagacion
document.addEventListener('DOMContentLoaded', function () {
    var buttons = document.querySelectorAll('.botones button');
    buttons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    });
});

// Declaracion de Impuestos
$(document).off('click', '#NewReporte').on('click', '#NewReporte', function () {
    var idReporte = $(this).data('idreporte');
    $('input[name="TipoReporte"]').val(idReporte);
    $('#ReporteModal').modal('show');
});

// Agregar Declaracion
document.getElementById("AddDeclaracion").addEventListener("click", function () {
    $("#newDeclaracionModal").modal("show");
});

document.getElementById('submitDeclaracionImpuesto').addEventListener('click', function () {
    var formData = new FormData(document.getElementById('DeclaracionImpuestoForm'));

    fetch('/DeclaracionImpuestoes/Create', {
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
                    text: 'Declaracion creada correctamente.',
                    icon: 'success',
                    confirmButtonColor: '#0DBCB5'
                }).then((result) => {
                    if (result.isConfirmed || result.isDismissed) {
                        $('#newDeclaracionModal').modal('hide');
                        window.location.href = data.url;
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
                icon: 'error'
            });
        });
});

// Editar Declaracion
document.querySelectorAll('.edit-Declaracion').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/DeclaracionImpuestoes/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editDeclaracionModal .modal-body').innerHTML = html;
                $('#editDeclaracionModal').modal('show');

                document.querySelector('#editDeclaracionModal #editDeclaracionImpuestoForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/DeclaracionImpuestoes/Edit/${Id}`, {
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
                                    $('#editDeclaracionModal').modal('hide');
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
                            $('#editDeclaracionModal').modal('hide');
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

// Eliminar Declaracion
document.querySelectorAll('.delete-Declaracion').forEach(button => {
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
                fetch(`/DeclaracionImpuestoes/Delete/${Id}`, {
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
                            }).then((result) => {
                                if (result.isConfirmed || result.isDismissed) {
                                    window.location.href = data.url;
                                }
                            });
                        } else {
                            Swal.fire({
                                title: 'Error',
                                text: 'Hubo un problema al eliminar la declaracion.',
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