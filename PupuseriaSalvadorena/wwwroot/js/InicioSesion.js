//Validacion de Inicio de Sesion
document.getElementById('InicioSesionForm').addEventListener('submit', function (event) {
    event.preventDefault();

    $('.loading').show();
    $('button').prop('disabled', true);

    var formData = new FormData(this);

    fetch('/Acceso/Login', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                window.location.href = data.url;
            } else {
                $('.loading').hide();
                $('button').prop('disabled', false);
                Swal.fire({
                    title: 'Error',
                    text: data.message,
                    icon: 'error',
                    confirmButtonColor: '#0DBCB5'
                });
            }
        })
        .catch(error => {
            $('.loading').hide();
            $('button').prop('disabled', false);
            console.error('Error capturado:', error);
            Swal.fire({
                title: 'Error',
                text: error,
                icon: 'error',
                confirmButtonColor: '#0DBCB5'
            });
        });
});

