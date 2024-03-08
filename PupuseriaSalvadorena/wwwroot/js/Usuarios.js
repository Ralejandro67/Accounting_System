// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaUsuarios tbody');
    var rows = table.rows;
    var totalRows = rows.length;
    var totalPages = Math.ceil(totalRows / rowsPerPage);

    function showPage(page) {
        for (let i = 0; i < totalRows; i++) {
            rows[i].style.display = (i >= (page - 1) * rowsPerPage && i < page * rowsPerPage) ? "" : "none";
        }

        document.getElementById('pageIndicator').textContent = `${page} / ${totalPages}`;
    }

    window.changePage = function (direction) {
        currentPage += direction;

        if (currentPage < 1) currentPage = 1;
        if (currentPage > totalPages) currentPage = totalPages;

        showPage(currentPage);
    };

    showPage(currentPage);
}

document.addEventListener('DOMContentLoaded', setupPagination);

// Busqueda
document.getElementById('busqueda').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var rows = document.querySelectorAll('#tablaUsuarios tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaUsuarios th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaUsuarios');
            var tableBody = table.querySelector('tbody');
            var rowsArray = Array.from(tableBody.rows);
            var column = this.cellIndex;
            var isAscending = header.classList.contains('asc');
            var isDateColumn = this.id === 'Fecha';

            rowsArray.sort(function (rowA, rowB) {
                var textA = rowA.cells[column].textContent.trim();
                var textB = rowB.cells[column].textContent.trim();

                if (isDateColumn) {
                    var dateA = new Date(textA.split('/').reverse().join('-'));
                    var dateB = new Date(textB.split('/').reverse().join('-'));
                    return (dateA - dateB) * (isAscending ? -1 : 1);
                } else {
                    return textA.localeCompare(textB, undefined, { numeric: true }) * (isAscending ? -1 : 1);
                }
            });

            document.querySelectorAll('#tablaUsuarios th').forEach(th => {
                th.innerHTML = th.textContent;
                th.classList.remove('asc', 'desc');
            });

            var newIconHtml = isAscending ? '<i class="fas fa-arrow-down"></i>' : '<i class="fas fa-arrow-up"></i>';
            this.innerHTML += ' ' + newIconHtml;
            this.classList.toggle('asc', !isAscending);
            this.classList.toggle('desc', isAscending);

            rowsArray.forEach(row => tableBody.appendChild(row));
        });
    }
});

// Agregar Usuario
document.getElementById("AddUsuario").addEventListener("click", function () {
    $.ajax({
        url: '/Usuarios/Create',
        type: 'GET',
        success: function (result) {
            $('#newUsuarioModal .modal-body').html(result);
            $('#newUsuarioModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitUsuarioForm') {

        var contrasena = document.getElementById('Contrasena').value;
        var telefono = document.getElementById('Telefono').value;
        var regex = /^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{10,}$/;
        var regexTel = /^[0-9]{8}$/;

        if (!regex.test(contrasena)) {
            Swal.fire({
                title: 'Error',
                text: 'La contraseña debe ser alfanumérica con al menos una letra mayúscula, un símbolo especial y al menos 10 caracteres de longitud.',
                icon: 'error',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (!regexTel.test(telefono)) {
            Swal.fire({
                title: 'Error',
                text: 'El número de teléfono debe contener 8 dígitos.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            })
            return;
        }

        var formData = new FormData(document.getElementById('UsuarioForm'));

        fetch('/Usuarios/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#newUsuarioModal').modal('hide');
                    Swal.fire({
                        title: '¡Éxito!',
                        text: data.message,
                        icon: 'success',
                        confirmButtonColor: '#0DBCB5'
                    }).then((result) => {
                        if (result.isConfirmed || result.isDismissed) {
                            window.location.reload();
                        }
                    });
                } else {
                    let errorMessage = data.message;
                    if (data.errors && data.errors.length > 0) {
                        errorMessage += "\n" + data.errors.join("\n");
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            text: errorMessage,
                            icon: 'warning',
                            confirmButtonColor: '#0DBCB5'
                        });
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

$(document).on('change', '#IdProvincia', function () {
    var idProvincia = $(this).val();
    $.ajax({
        url: '/Usuarios/GetCantones',
        type: 'GET',
        dataType: 'json',
        data: { IdProvincia: idProvincia },
        success: function (data) {
            var cantonesSelect = $('#IdCanton');
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

$(document).on('change', '#IdCanton', function () {
    var idCanton = $(this).val();
    $.ajax({
        url: '/Usuarios/GetDistritos',
        type: 'GET',
        dataType: 'json',
        data: { IdCanton: idCanton },
        success: function (data) {
            var distritosSelect = $('#IdDistrito');
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

// Editar usuario
document.querySelectorAll('.edit-Usuario').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');
        fetch(`/Usuarios/Edit/${Id}`)
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
                            $('#editUsuarioModal').modal('hide');
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

// Eliminar usuario
document.querySelectorAll('.delete-Usuario').forEach(button => {
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
                fetch(`/Usuarios/Delete/${Id}`, {
                    method: 'POST'
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            Swal.fire({
                                title: '¡Eliminado!',
                                text: 'El usuario ha sido eliminado.',
                                icon: 'success',
                                confirmButtonColor: '#0DBCB5'
                            }).then(() => {
                                window.location.reload();
                            });
                        } else {
                            Swal.fire({
                                title: 'Error',
                                text: 'Hubo un problema al eliminar el usuario.',
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