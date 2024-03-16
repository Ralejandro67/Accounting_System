// Editar usuario
document.querySelectorAll('.edit-Negocio').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/Negocios/Edit/${Id}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editNegocioModal .modal-body').innerHTML = html;
                $('#editNegocioModal').modal('show');

                document.querySelector('#editNegocioModal #editNegocioForm').addEventListener('submit', function (e) {
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

                    fetch(`/Negocios/Edit/${Id}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editNegocioModal').modal('hide');
                            if (data.success) {
                                Swal.fire({
                                    title: '¡Éxito!',
                                    text: data.message,
                                    icon: 'success',
                                    confirmButtonColor: '#0DBCB5'
                                }).then(() => {
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
                            $('#editNegocioModal').modal('hide');
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