//Grafico Barras
document.addEventListener('DOMContentLoaded', function () {
    var fechasPronosticos = detalles.map(function (d) { return d.FechaPronostico; });
    var cantPVentas = detalles.map(function (d) { return d.PCantVenta; });

    var coloresPredefinidos = [
        'rgba(150, 239, 255, 1.0)',
        'rgba(95, 189, 255, 1.0)',
        'rgba(114, 221, 217, 1)',
        'rgba(23, 107, 135, 1)',
        'rgba(166, 246, 241, 1)'
    ];

    var colores = fechasPronosticos.map(function (_, index) {
        return coloresPredefinidos[index % coloresPredefinidos.length];
    });

    var ctx = document.getElementById('PVentasChart').getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: fechasPronosticos,
            datasets: [{
                label: 'Pronosticos',
                data: cantPVentas,
                backgroundColor: colores,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 20
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