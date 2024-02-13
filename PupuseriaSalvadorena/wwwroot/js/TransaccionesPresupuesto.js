// Eliminar transaccion
document.querySelectorAll('.delete-detPresupuesto').forEach(button => {
    button.addEventListener('click', function () {
        var IdPresupuesto = this.getAttribute('data-idPresupuesto');
        var Id = this.getAttribute('data-id');

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, elimínalo!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                var formData = new FormData();
                formData.append('IdPresupuesto', IdPresupuesto);
                formData.append('Id', Id);

                fetch(`/DetallePresupuestos/Delete`, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                    }
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire(
                            '¡Eliminado!',
                            'La transaccion ha sido eliminada.',
                            'success'
                        ).then(() => {
                            window.location.reload();
                        });
                    } else {
                        Swal.fire(
                            'Error',
                            'Hubo un problema al eliminar la transaccion.',
                            'error'
                        );
                    }
                })
                .catch(error => {
                    Swal.fire(
                        'Error',
                        'Hubo un problema con la solicitud.',
                        'error'
                    );
                });
            }
        })
    });
});