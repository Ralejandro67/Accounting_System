window.onload = function () {
    $('.loading').hide();
    $('button').prop('disabled', false);
};

//Graficos
document.addEventListener('DOMContentLoaded', function () {
    var fechasPronosticos = pronosticos.map(function (d) { return d.FechaPronostico; });
    var cantPVentas = pronosticos.map(function (d) { return d.PCantVenta; });
    var Meses = ingresos.map(function (x) { return x.Mes; });
    var MontoIngresos = ingresos.map(function (d) { return d.TotalMonto; });
    var MontoEgresos = egresos.map(function (d) { return d.TotalMonto; });
    var platillos = ventas.map(function (d) { return d.NombrePlatillo; });
    var cantVentas = ventas.map(function (d) { return d.TotalVentas; });
    var materiasPrimas = compras.map(function (d) { return d.NombreMateriaPrima; });
    var cantCompras = compras.map(function (d) { return d.TotalCompras; });

    var coloresPredefinidos = [
        'rgba(150, 239, 255, 1.0)',
        'rgba(95, 189, 255, 1.0)',
        'rgba(114, 221, 217, 1)',
        'rgba(23, 107, 135, 1)',
        'rgba(166, 246, 241, 1)'
    ];

    //Grafico Lineal Ingresos y Egresos
    var cta = document.getElementById('TransaccionesChart').getContext('2d');

    var transaccionesChart = new Chart(cta, {
        type: 'bar',
        data: {
            labels: Meses,
            datasets: [{
                label: 'Ingresos',
                data: MontoIngresos,
                backgroundColor: 'rgba(150, 239, 255, 1)',
                borderWidth: 1
            },
            {
                label: 'Egresos',
                data: MontoEgresos,
                backgroundColor: 'rgba(95, 189, 255, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        fontColor: 'black',
                        fontSize: 25,
                        boxWidth: 13
                    }
                }
            }
        }
    });

    //Grafico Circular Ventas

    var coloresVentas = platillos.map(function (_, index) {
        return coloresPredefinidos[index % coloresPredefinidos.length];
    });

    var ctb = document.getElementById('VentasChart').getContext('2d');

    var ventasChart = new Chart(ctb, {
        type: 'doughnut',
        data: {
            labels: platillos,
            datasets: [{
                label: 'Ventas',
                data: cantVentas,
                backgroundColor: coloresVentas,
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'right',
                    labels: {
                        fontColor: 'black',
                        fontSize: 25,
                        boxWidth: 13
                    }
                }
            }
        }
    });

    //Grafico Barras Pronosticos

    var ctx = document.getElementById('PronosticosChart').getContext('2d');

    var pronosticosChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: fechasPronosticos,
            datasets: [{
                label: 'Ventas',
                data: cantPVentas,
                backgroundColor: 'rgba(95, 189, 255, 1)',
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
                    position: 'bottom',
                    labels: {
                        fontColor: 'black',
                        fontSize: 25,
                        boxWidth: 13
                    }
                }
            }
        }
    });

    //Grafico Circular Compras

    var coloresCompras = materiasPrimas.map(function (_, index) {
        return coloresPredefinidos[index % coloresPredefinidos.length];
    });

    var ctc = document.getElementById('ComprasChart').getContext('2d');

    var comprasChart = new Chart(ctc, {
        type: 'doughnut',
        data: {
            labels: materiasPrimas,
            datasets: [{
                label: 'Compras',
                data: cantCompras,
                backgroundColor: coloresCompras,
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'right',
                    labels: {
                        fontColor: 'black',
                        fontSize: 25,
                        boxWidth: 13
                    }
                }
            }
        }
    });
}); 