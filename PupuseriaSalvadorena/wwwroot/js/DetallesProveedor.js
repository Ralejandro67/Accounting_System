//Grafico Circular
document.addEventListener('DOMContentLoaded', function () {
    var materiasPrimas = detalles.map(function (d) { return d.NombreMateriaPrima; });
    var conteoMaterias = detalles.map(function (d) { return d.ConteoCompras; });

    var coloresPredefinidos = [
        'rgba(150, 239, 255, 1.0)',
        'rgba(95, 189, 255, 1.0)',
        'rgba(114, 221, 217, 1)',
        'rgba(23, 107, 135, 1)',
        'rgba(166, 246, 241, 1)'
    ];

    var colores = materiasPrimas.map(function (_, index) {
        return coloresPredefinidos[index % coloresPredefinidos.length];
    });


    var ctx = document.getElementById('ComprasMatChart').getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: materiasPrimas,
            datasets: [{
                label: 'Compras',
                data: conteoMaterias,
                backgroundColor: colores,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 10
                    }
                }
            },
            responsive: true,
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });
}); 

// Editar proveedor
document.querySelectorAll('.edit-Proveedor').forEach(button => {
    button.addEventListener('click', function () {
        var proveedoresId = this.getAttribute('data-id');
        fetch(`/Proveedores/Edit/${proveedoresId}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#editProveedorModal .modal-body').innerHTML = html;
                $('#editProveedorModal').modal('show');

                document.querySelector('#editProveedorModal #editProveedorForm').addEventListener('submit', function (e) {
                    e.preventDefault();

                    var formData = new FormData(this);

                    fetch(`/Proveedores/Edit/${proveedoresId}`, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            $('#editProveedorModal').modal('hide');
                            if (data.success) {
                                Swal.fire({
                                    title: '¡Éxito!',
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
                            $('#editProveedorModal').modal('hide');
                            Swal.fire({
                                title: 'Error',
                                text: 'Hubo un problema con la solicitud.',
                                icon: 'error',
                                confirmButtonColor: '#0DBCB5'
                            });
                        });
                });
            })
            .catch(error => console.error('Error:', error));
    });
});