document.addEventListener('DOMContentLoaded', function () {
    var buttons = document.querySelectorAll('.botones button');
    buttons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    });
});

// Agregar Presupuesto
document.getElementById("AddPresupuesto").addEventListener("click", function () {
    $.ajax({
        url: '/Presupuestoes/Create',
        type: 'GET',
        success: function (result) {
            $('#newPresupuestoModal .modal-body').html(result);
            $('#newPresupuestoModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitPresupuesto') {
        var saldoPresupuesto = parseFloat(document.getElementById('SaldoIncial').value);
        var saldoDisponible = parseFloat(document.getElementById('SaldoDisponible').dataset.saldo);

        if (saldoPresupuesto > saldoDisponible) {
            Swal.fire({
                title: 'Error',
                text: 'El saldo del prespuesto no puede ser mayor al saldo disponible.',
                icon: 'warning'
            });
            return;
        }

        var formData = new FormData(document.getElementById('PresupuestoForm'));

        fetch('/Presupuestoes/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#newPresupuestoModal').modal('hide');
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

// Editar Presupuesto
document.querySelectorAll('.edit-Presupuesto').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/Presupuestoes/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editPresupuestoModal .modal-body').innerHTML = html;
                $('#editPresupuestoModal').modal('show');

                document.querySelector('#editPresupuestoModal #editPresupuestoForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/Presupuestoes/Edit/${Id}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editPresupuestoModal').modal('hide');
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
                            $('#editPresupuestoModal').modal('hide');
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

// Eliminar Presupuesto
document.querySelectorAll('.delete-Presupuesto').forEach(button => {
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
                fetch(`/Presupuestoes/Delete/${Id}`, {
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