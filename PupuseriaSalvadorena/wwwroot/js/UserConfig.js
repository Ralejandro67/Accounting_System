// Editar usuario
document.querySelectorAll('.edit-Usuario').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/Usuarios/Editar/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editUsuarioModal .modal-body').innerHTML = html;
                $('#editUsuarioModal').modal('show');

                document.querySelector('#editUsuarioModal #editUsuarioForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var telefono = document.getElementById('TelefonoEdit').value;

                    var regexTel = /^[0-9]{8}$/;
                    if (!regexTel.test(telefono)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'El número de teléfono debe contener 8 dígitos.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        })
                        return;
                    }

                    var formData = new FormData(this);

                    fetch(`/Usuarios/Edit/${Id}`, {
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
                                    $('#editUsuarioModal').modal('hide');
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
                            $('#editUsuarioModal').modal('hide');
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

$(document).on('change', '#IdProvinciaEdit', function () {
    var idProvincia = $(this).val();
    $.ajax({
        url: '/Usuarios/GetCantones',
        type: 'GET',
        dataType: 'json',
        data: { IdProvincia: idProvincia },
        success: function (data) {
            var cantonesSelect = $('#IdCantonEdit');
            cantonesSelect.empty();
            cantonesSelect.append($('<option></option>').val('').text('Cantones'));
            $.each(data, function (index, item) {
                cantonesSelect.append($('<option></option>').val(item.value).text(item.text));
            });
        },
        error: function () {
            alert('Error al cargar las materias primas');
        }
    });
});

$(document).on('change', '#IdCantonEdit', function () {
    var idCanton = $(this).val();
    $.ajax({
        url: '/Usuarios/GetDistritos',
        type: 'GET',
        dataType: 'json',
        data: { IdCanton: idCanton },
        success: function (data) {
            var distritosSelect = $('#IdDistritoEdit');
            distritosSelect.empty();
            distritosSelect.append($('<option></option>').val('').text('Distritos'));
            $.each(data, function (index, item) {
                distritosSelect.append($('<option></option>').val(item.value).text(item.text));
            });
        },
        error: function () {
            alert('Error al cargar las materias primas');
        }
    });
});

// Editar Contrasena
document.querySelectorAll('.edit-Contrasena').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/Usuarios/Password/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editPasswordModal .modal-body').innerHTML = html;
                $('#editPasswordModal').modal('show');

                document.querySelector('#editPasswordModal #PasswordForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var contrasena = document.getElementById('ContrasenaEdit').value;
                    var contrasenaConfirm = document.getElementById('ContrasenaConfirm').value;
                    var regex = /^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{10,}$/;

                    if (!regex.test(contrasena)) {
                        Swal.fire({
                            title: 'Error',
                            text: 'La contraseña debe ser alfanumérica con al menos una letra mayúscula, un símbolo especial y al menos 10 caracteres de longitud.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    if (contrasena !== contrasenaConfirm) {
                        Swal.fire({
                            title: 'Error',
                            text: 'La nueva contraseña no coincide con la confirmacion.',
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
                        return;
                    }

                    var formData = new FormData(this);

                    fetch(`/Usuarios/EditPassword/${Id}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                $('#editPasswordModal').modal('hide');
                                Swal.fire({
                                    title: '¡Éxito!',
                                    text: data.message,
                                    icon: 'success',
                                    confirmButtonColor: '#0DBCB5'
                                }).then(() => {
                                    window.location.reload();
                                });
                            } else {
                                let errorMessage = data.message;
                                if (data.errors && data.errors.length > 0) {
                                    errorMessage += "\n" + data.errors.join("\n");
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
                            $('#editPasswordModal').modal('hide');
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