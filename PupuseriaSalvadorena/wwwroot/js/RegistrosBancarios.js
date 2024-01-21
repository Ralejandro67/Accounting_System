// Agregar Estado Bancario
document.getElementById("AddEstado").addEventListener("click", function () {
    $.ajax({
        url: '/RegistroBancarios/Create',
        type: 'GET',
        success: function (result) {
            $('#newEstadoBModal .modal-body').html(result);
            $('#newEstadoBModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitEstadoBacForm') {
        var formData = new FormData(document.getElementById('estadoBacForm'));

        fetch('/RegistroBancarios/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#newEstadoBModal').modal('hide');
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
                console.error('Error en la solicitud:', error);
                Swal.fire({
                    title: 'Error',
                    text: 'Hubo un problema con la solicitud. ' + error.toString(),
                    icon: 'error'
                });
            });
    }
});