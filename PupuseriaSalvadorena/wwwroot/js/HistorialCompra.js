// Paginacion
var currentPage = 1;
var rowsPerPage = 10;

function setupPagination() {
    var table = document.querySelector('#tablaHistorial tbody');
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
    var rows = document.querySelectorAll('#tablaHistorial tbody tr');

    rows.forEach(row => {
        var text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchTerm) ? '' : 'none';
    });
});

// Ordernar tabla
document.querySelectorAll('#tablaHistorial th').forEach(header => {
    if (!header.id.includes("acciones")) {
        header.addEventListener('click', function () {
            var table = document.querySelector('#tablaHistorial');
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

            document.querySelectorAll('#tablaHistorial th').forEach(th => {
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

// Eliminar Factura Compra Historial
document.querySelectorAll('.delete-Compra').forEach(button => {
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
                fetch(`/HistorialCompras/DeleteConfirmation/${IdFactura}`)
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
                                fetch(`/HistorialCompras/Delete/${IdFactura}`, {
                                    method: 'POST',
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