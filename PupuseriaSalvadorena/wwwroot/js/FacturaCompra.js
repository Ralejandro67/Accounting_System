// Reporte de Facturas
$(document).off('click', '#NewReporte').on('click', '#NewReporte', function () {
    var idReporte = $(this).data('idreporte');
    $('input[name="TipoReporte"]').val(idReporte);
    $('#ReporteModal').modal('show');
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitReporteForm') {
        var formData = new FormData(document.getElementById('ReporteForm'));

        fetch('/FacturaCompras/ReporteFacturas', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
        .then(response => {
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.indexOf("application/json") !== -1) {
                return response.json().then(data => {
                    Swal.fire({
                        title: 'Error',
                        text: data.message,
                        icon: 'error',
                        confirmButtonColor: '#0DBCB5'
                    });
                });
            } else if (contentType && contentType.indexOf("application/pdf") !== -1 || contentType.indexOf("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") !== -1) {
                response.blob().then(blob => {
                    var url = window.URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = response.headers.get("content-disposition").split('filename=')[1].replaceAll('"', '');
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                });
            } else {
                Swal.fire({
                    title: 'Error',
                    text: 'Formato de respuesta desconocido.',
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
});

// Agregar Factura Compra
document.getElementById("AddFacturaC").addEventListener("click", function () {
    $.ajax({
        url: '/FacturaCompras/Create',
        type: 'GET',
        success: function (result) {
            $('#newFacturaCModal .modal-body').html(result);
            toggleCamposActivos();
            $('#newFacturaCModal').modal('show');

            var tooltipTriggerList = [].slice.call(document.querySelectorAll('#newFacturaCModal [data-bs-toggle="tooltip"]'));
            var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });
        },
        error: function (error) {
            console.error("Error al cargar la vista parcial", error);
        }
    });
});

$(document).on('change', '[name="EstadoFactura"]', function () {
    toggleCamposActivos();
});

function toggleCamposActivos() {
    var isPorPagarSelected = $('#FacturaPorPagar').is(':checked');
    $('#campoFechaVencimiento').toggle(isPorPagarSelected);
    $('#cuentaPorPagar').val(isPorPagarSelected ? 'true' : 'false');
}

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'submitFacturaC') {

        var ValueCant = document.getElementById('Cant').value;
        var ValueTotal = document.getElementById('TotalCompra').value;
        var ValuePeso = document.getElementById('Peso').value;

        var regex = /^[0-9]+(\.[0-9]+)?$/;
        if (!regex.test(ValueCant) || !regex.test(ValueTotal) || !regex.test(ValuePeso)) {
            Swal.fire({
                title: 'Error',
                text: 'El total de la compra, la cantidad del producto o el peso debe ser numerico.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        if (parseFloat(ValueCant) < 1 || parseFloat(ValueTotal) < 1 || parseFloat(ValuePeso) < 1) {
            Swal.fire({
                title: 'Error',
                text: 'Debes agregar un monto a el valor del producto, la cantidad, el peso.',
                icon: 'warning',
                confirmButtonColor: '#0DBCB5'
            });
            return;
        }

        var formData = new FormData(document.getElementById('FacturaCForm'));

        fetch('/FacturaCompras/Create', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        title: '¡Éxito!',
                        text: data.message,
                        icon: 'success',
                        confirmButtonColor: '#0DBCB5'
                    }).then((result) => {
                        if (result.isConfirmed || result.isDismissed) {
                            $('#newFacturaCModal').modal('hide');
                            window.location.reload();
                        }
                    });
                } else {
                    let errorMessage = "";
                    if (data.message) {
                        errorMessage = data.message;
                    } else if (data.errors && data.errors.length > 0) {
                        errorMessage = data.errors.join("\n");
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

$(document).on('change', '#IdProveedor', function () {
    var idProveedor = $(this).val();
    $.ajax({
        url: '/FacturaCompras/GetMateriasPrimas',
        type: 'GET',
        dataType: 'json',
        data: { idProveedor: idProveedor },
        success: function (data) {
            var materiasPrimaSelect = $('#IdMateriaPrima');
            materiasPrimaSelect.empty();
            materiasPrimaSelect.append($('<option></option>').val('').text('Materias Primas'));
            $.each(data, function (index, item) {
                materiasPrimaSelect.append($('<option></option>').val(item.value).text(item.text));
            });
        },
        error: function () {
            alert('Error al cargar las materias primas');
        }
    });
});

// Dercargar Factura Compra
document.querySelectorAll('.download-Factura').forEach(button => {
    button.addEventListener('click', function () {
        var IdFactura = this.getAttribute('data-id');
        fetch(`/FacturaCompras/DescargarFactura/${IdFactura}`)
            .then(response => {
                if (response.ok) {
                    const contentType = response.headers.get("content-type");
                    if (contentType && contentType.includes("application/pdf")) {
                        return response.blob();
                    } else {
                        return response.json();
                    }
                } else {
                    throw new Error('Respuesta de red no fue ok.');
                }
            })
            .then(data => {
                if (data instanceof Blob) {
                    const url = window.URL.createObjectURL(data);
                    const a = document.createElement("a");
                    a.href = url;
                    a.download = `Factura_${IdFactura}.pdf`;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                } else if (data && data.message) {
                    Swal.fire({
                        title: 'Error',
                        text: data.message,
                        icon: 'warning',
                        confirmButtonColor: '#0DBCB5'
                    });
                }
            })
            .catch(error => {
                Swal.fire({
                    title: 'Error',
                    text: 'Hubo un problema al descargar la factura.',
                    icon: 'error',
                    confirmButtonColor: '#0DBCB5'
                });
            });
    });
});

// Eliminar Factura Compra
document.querySelectorAll('.delete-Factura').forEach(button => {
    button.addEventListener('click', function () {
        var IdFactura = this.getAttribute('data-id');

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
                fetch(`/FacturaCompras/DeleteConfirmation/${IdFactura}`)
                    .then(response => response.json())
                    .then(data => {
                        let promise;
                        if (data.success) {
                            promise = Swal.fire({
                                title: 'Cuidado',
                                text: "Esta factura tiene una cuenta por pagar asociada, para continuar se debe eliminar la cuenta. ¿Deseas continuar?",
                                icon: 'warning',
                                showCancelButton: true,
                                confirmButtonColor: '#0DBCB5',
                                cancelButtonColor: '#9DB2BF',
                                confirmButtonText: 'Eliminar',
                                cancelButtonText: 'Cancelar'
                            });
                        } else {
                            promise = Promise.resolve({ isConfirmed: true });
                        }
                        promise.then((result) => {
                            if (result.isConfirmed) {
                                fetch(`/FacturaCompras/Delete/${IdFactura}`, {
                                    method: 'POST',
                                    headers: {
                                        'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                                    }
                                })
                                    .then(response => response.json())
                                    .then(data => {
                                        if (data.success) {
                                            Swal.fire({
                                                title: '¡Eliminado!',
                                                text: 'La factura ha sido eliminada.',
                                                icon: 'success',
                                                confirmButtonColor: '#0DBCB5'
                                            }).then(() => window.location.reload());
                                        } else {
                                            Swal.fire({
                                                title: 'Error',
                                                text: 'Hubo un problema al eliminar la factura.',
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
                        });
                    });
            }
        });
    });
});