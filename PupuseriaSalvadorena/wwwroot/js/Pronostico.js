// Agregar Cuenta por Pagar
document.getElementById("AddPronostico").addEventListener("click", function () {
    $.ajax({
        url: '/Pronosticos/Create',
        type: 'GET',
        success: function (result) {
            $('#newPronosticoModal .modal-body').html(result);
            var today = new Date();
            today.setDate(today.getDate() + 1);
            var minDate = today.toISOString().split('T')[0];
            document.getElementById('fechaFinal').min = minDate;
            $('#newPronosticoModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitPronostico') {
        var formData = new FormData(document.getElementById('PronosticoForm'));

        fetch('/Pronosticos/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#newPronosticoModal').modal('hide');
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