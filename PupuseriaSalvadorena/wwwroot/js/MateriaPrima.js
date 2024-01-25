// Agregar Materia Prima
document.getElementById("AddMateriaPrima").addEventListener("click", function () {
    $.ajax({
        url: '/MateriaPrimas/Create',
        type: 'GET',
        success: function (result) {
            $('#newMateriaPrimaModal .modal-body').html(result);
            $('#newMateriaPrimaModal').modal('show');
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitMateriaPrima') {
        var formData = new FormData(document.getElementById('MateriaPrimaForm'));

        fetch('/MateriaPrimas/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                $('#newMateriaPrimaModal').modal('hide');
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