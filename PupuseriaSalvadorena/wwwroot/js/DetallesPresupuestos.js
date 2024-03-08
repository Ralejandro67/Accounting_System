function BarraProgreso() {
    const progressBar = document.getElementById('progressBar');
    const fechaInicio = progressBar.getAttribute('data-start-date');
    const fechaFin = progressBar.getAttribute('data-end-date');

    var fechaInicioDate = new Date(fechaInicio);
    fechaInicioDate.setDate(fechaInicioDate.getDate() + 1);

    var fechaFinDate = new Date(fechaFin);
    fechaFinDate.setDate(fechaFinDate.getDate() + 1);

    var fechaActual = new Date();

    fechaInicioDate.setHours(0, 0, 0, 0);
    fechaFinDate.setHours(0, 0, 0, 0);
    fechaActual.setHours(0, 0, 0, 0);

    const fechaActualFormateada = fechaActual.toLocaleDateString('es-CR', {
        year: 'numeric', month: 'long', day: 'numeric'
    });

    var diffInicioActual = Math.floor((fechaActual - fechaInicioDate) / (1000 * 60 * 60 * 24));
    var diffInicioFin = Math.floor((fechaFinDate - fechaInicioDate) / (1000 * 60 * 60 * 24));

    let progress;

    if (fechaActual >= fechaFinDate)
    {
        progress = 100;
    }
    else if (fechaActual == fechaInicioDate)
    {
        progress = 1;
    }
    else
    {
        progress = Math.min(Math.max((diffInicioActual / diffInicioFin) * 100, 0), 100);
    }

    progressBar.style.width = progress + '%';
    progressBar.title = 'Fecha actual: ' + fechaActualFormateada;
}

//Grafico Doughnut
document.addEventListener('DOMContentLoaded', function () {
    BarraProgreso();
    var ctx = document.getElementById('DoughnutChart').getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Saldo Usado', 'Saldo Restante'],
            datasets: [{
                label: 'Presupuesto',
                data: [saldoUsado, saldoRestante],
                backgroundColor: [
                    'rgba(150, 239, 255, 1.0)',
                    'rgba(95, 189, 255, 1.0)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        fontColor: 'black',
                        fontSize: 20,
                        boxWidth: 13
                    }
                }
            }
        }
    });
}); 

// Agregar transacciones seleccionadas
document.getElementById('DetallesPresupuestoForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const transaccionesSeleccionadas = document.querySelectorAll('input[name="TransaccionesSeleccionadas"]:checked');
    if (transaccionesSeleccionadas.length === 0) {
        Swal.fire({
            title: 'Error',
            text: 'Debes seleccionar al menos una transacción.',
            icon: 'warning',
            confirmButtonColor: '#0DBCB5'
        });
        return;
    }

    var formData = new FormData(this);

    fetch('/DetallePresupuestos/Create', {
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
                        window.location.reload();
                    }
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
            Swal.fire({
                title: 'Error',
                text: 'Hubo un problema con la solicitud.',
                icon: 'error',
                confirmButtonColor: '#0DBCB5'
            });
        });
});

