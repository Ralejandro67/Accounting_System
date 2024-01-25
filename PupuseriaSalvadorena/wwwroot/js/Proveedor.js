// Agregar proveedor
document.getElementById("AddProveedor").addEventListener("click", function () {
    $("#newProveedorModal").modal("show");
});

document.getElementById('submitProveedor').addEventListener('click', function () {
    var formData = new FormData(document.getElementById('ProveedorForm'));

    fetch('/Proveedores/Create', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                $('#newProveedorModal').modal('hide');
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