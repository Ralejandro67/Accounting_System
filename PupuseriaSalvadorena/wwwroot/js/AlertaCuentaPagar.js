// Mostrar Notificaciones
document.addEventListener('DOMContentLoaded', function () {
    cargarNotificaciones();
    countNotificaciones();

    setInterval(cargarNotificaciones, 1800000);
    setInterval(countNotificaciones, 1800000);
});

function cargarNotificaciones() {
    fetch('/AlertasCuentaPagar/GetNotificaciones')
        .then(response => response.text())
        .then(html => {
            document.getElementById('notificacionesContainer').innerHTML = html;
        })
        .catch(error => console.error('Error al cargar las notificaciones'));
}

function countNotificaciones() {
    fetch('/AlertasCuentaPagar/GetNotificacionesCount')
        .then(response => response.json())
        .then(data => {
            document.getElementById('notificationCount').textContent = data.count;
        })
        .catch(error => console.error('Error al cargar las notificaciones'));
}

// Agregar Alerta
document.getElementById("AddAlerta").addEventListener("click", function () {
    $.ajax({
        url: '/AlertasCuentaPagar/Create',
        type: 'GET',
        success: function (result) {
            $('#newAlertaModal .modal-body').html(result);
            $('#newAlertaModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitAlerta') {
        var formData = new FormData(document.getElementById('AlertaForm'));

        fetch('/AlertasCuentaPagar/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#newAlertaModal').modal('hide');
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

// Editar Alerta
document.querySelectorAll('.edit-Alerta').forEach(button => {
    button.addEventListener('click', function () {
        var IdAlerta = this.getAttribute('data-id');
        fetch(`/AlertasCuentaPagar/Edit/${IdAlerta}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editAlertaModal .modal-body').innerHTML = html;
                $('#editAlertaModal').modal('show');
                document.querySelector('#editAlertaModal #editAlertaForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/AlertasCuentaPagar/Edit/${IdAlerta}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editAlertaModal').modal('hide');
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
                            $('#editAlertaModal').modal('hide');
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