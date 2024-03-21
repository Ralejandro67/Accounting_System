// Imprimir factura

document.querySelectorAll('.print-FacturaVenta').forEach(button => {
    button.addEventListener('click', function () {
        $('.loading').show();
        $('button').prop('disabled', true);

        var id = this.getAttribute('data-id');
        fetch(`/FacturaVentas/ImprimirFactura/${id}`, {
            method: 'GET'
        })
            .then(response => response.json())
            .then(data => {
                $('.loading').hide();
                $('button').prop('disabled', false);
                if (data.success) {
                    window.open(data.url, '_blank');
                } else {
                    let errorMessage = "Error al imprimir la factura.";
                    if (data.message) {
                        errorMessage = data.message;
                    }

                    Swal.fire({
                        title: 'Error',
                        text: errorMessage,
                        icon: 'warning',
                        confirmButtonColor: '#0DBCB5'
                    });
                }
            })
            .catch(error => console.error('Error:', error));
    });
});

// Anular factura
document.querySelectorAll('.delete-FacturaVenta').forEach(button => {
    button.addEventListener('click', function () {
        var Id = this.getAttribute('data-id');

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡Si anulas la factura no podrás revertir este cambio!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#0DBCB5',
            cancelButtonColor: '#9DB2BF',
            confirmButtonText: 'Sí, proceder!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $('.loading').show();
                $('button').prop('disabled', true);
                fetch(`/FacturaVentas/Delete/${Id}`, {
                    method: 'POST'
                })
                    .then(response => response.json())
                    .then(data => {
                        $('.loading').hide();
                        $('button').prop('disabled', false);
                        if (data.success) {
                            Swal.fire({
                                title: '¡Anulada!',
                                text: data.message,
                                icon: 'success',
                                confirmButtonColor: '#0DBCB5'
                            }).then(() => {
                                window.location.reload();
                            });
                        } else {
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