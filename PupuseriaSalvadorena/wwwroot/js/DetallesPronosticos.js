// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaPronosticos tbody');
    var rows = table.rows;
    var totalRows = rows.length;
    var totalPages = Math.ceil(totalRows / rowsPerPage);

    function showPage(page) {
        for (let i = 0; i < totalRows; i++) {
            rows[i].style.display = (i >= (page - 1) * rowsPerPage && i < page * rowsPerPage) ? "" : "none";
        }

        document.getElementById('pageIndicator').textContent = `${page} / ${totalPages}`;
    }

    window.changePage = function (direction) {
        currentPage += direction;

        if (currentPage < 1) currentPage = 1;
        if (currentPage > totalPages) currentPage = totalPages;

        showPage(currentPage);
    };

    showPage(currentPage);
}

document.addEventListener('DOMContentLoaded', setupPagination);

// Busqueda
document.getElementById('busqueda').addEventListener('keyup', function () {
    var searchTerm = this.value.toLowerCase();
    var rows = document.querySelectorAll('#tablaPronosticos tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaPronosticos th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaPronosticos');
            var tableBody = table.querySelector('tbody');
            var rowsArray = Array.from(tableBody.rows);
            var column = this.cellIndex;
            var isAscending = header.classList.contains('asc');
            var isDateColumn = this.id === 'Fecha';

            rowsArray.sort(function (rowA, rowB) {
                var textA = rowA.cells[column].textContent.trim();
                var textB = rowB.cells[column].textContent.trim();

                if (isDateColumn) {
                    var dateA = new Date(textA.split('/').reverse().join('-'));
                    var dateB = new Date(textB.split('/').reverse().join('-'));
                    return (dateA - dateB) * (isAscending ? -1 : 1);
                } else {
                    return textA.localeCompare(textB, undefined, { numeric: true }) * (isAscending ? -1 : 1);
                }
            });

            document.querySelectorAll('#tablaPronosticos th').forEach(th => {
                th.innerHTML = th.textContent;
                th.classList.remove('asc', 'desc');
            });

            var newIconHtml = isAscending ? '<i class="fas fa-arrow-down"></i>' : '<i class="fas fa-arrow-up"></i>';
            this.innerHTML += ' ' + newIconHtml;
            this.classList.toggle('asc', !isAscending);
            this.classList.toggle('desc', isAscending);

            rowsArray.forEach(row => tableBody.appendChild(row));
        });
    }
});

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