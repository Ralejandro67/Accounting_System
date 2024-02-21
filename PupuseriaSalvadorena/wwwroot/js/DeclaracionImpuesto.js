// Agregar Tipo de Movimiento
document.getElementById("AddDeclaracion").addEventListener("click", function () {
    $("#newPronosticoModal").modal("show");
});

document.getElementById('submitDeclaracionImpuesto').addEventListener('click', function () {
    var formData = new FormData(document.getElementById('DeclaracionImpuestoForm'));

    fetch('/DeclaracionImpuestoes/Create', {
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