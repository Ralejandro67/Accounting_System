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
            var today = new Date();
            today.setDate(today.getDate());
            var minDate = today.toISOString().split('T')[0];
            document.getElementById('fechaFinal').min = minDate;
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
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        var regex = /^[0-9]+(\.[0-9]+)?$/;
        if (!regex.test(saldoPresupuesto)) {
            Swal.fire({
                title: 'Error',
                text: 'El saldo del prespuesto debe ser un número.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (parseFloat(saldoPresupuesto) < 1) {
            Swal.fire({
                title: 'Error',
                text: 'El saldo del prespuesto no puede ser un número negativo.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
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
                    Swal.fire({
                        title: '¡Éxito!',
                        text: data.message,
                        icon: 'success',
                        confirmButtonColor: '#0DBCB5'
                    }).then((result) => {
                        if (result.isConfirmed || result.isDismissed) {
                            $('#newPresupuestoModal').modal('hide');
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

                    var saldoPresupuesto = parseFloat(document.getElementById('SaldoIncialE').value);
                    var saldoDisponible = parseFloat(document.getElementById('SaldoDisponibleE').dataset.saldo);

                    if (saldoPresupuesto > saldoDisponible) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El saldo del prespuesto no puede ser mayor al saldo disponible.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    var regex = /^[0-9]+(\.[0-9]+)?$/;
                    if (!regex.test(saldoPresupuesto)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El saldo del prespuesto debe ser un número.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    if (parseFloat(saldoPresupuesto) < 1) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El saldo del prespuesto no puede ser un número negativo.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

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
                            if (data.success) {
                                Swal.fire({
                                    title: '¡Éxito!',
                                    text: data.message,
                                    icon: 'success',
                                    confirmButtonColor: '#0DBCB5'
                                }).then(() => {
                                    $('#editPresupuestoModal').modal('hide');
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
                            $('#editPresupuestoModal').modal('hide');
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

// Eliminar Presupuesto
document.querySelectorAll('.delete-Presupuesto').forEach(button => {
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
                fetch(`/Presupuestoes/Delete/${Id}`, {
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