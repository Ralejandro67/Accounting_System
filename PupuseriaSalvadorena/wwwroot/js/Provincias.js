// Agregar provincia
document.getElementById("AddProvincia").addEventListener("click", function () {
    $("#newProvinciaModal").modal("show");
});

document.getElementById('submitProvinciaForm').addEventListener('click', function () {
    var formData = new FormData(document.getElementById('provinciaForm'));

    fetch('/Provincias/Create', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                $('#newProvinciaModal').modal('hide');
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
});