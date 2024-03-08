// Mostrar Notificaciones
document.addEventListener('DOMContentLoaded', function () {
    cargarNotificaciones();
    countNotificaciones();

    setInterval(cargarNotificaciones, 1800000);
    setInterval(countNotificaciones, 1800000);
});

function cargarNotificaciones() {
    fetch('/AlertasCuentaPagar/GetNotificaciones')
        .then(response => response.text())
        .then(html => {
            document.getElementById('notificacionesContainer').innerHTML = html;
        })
        .catch(error => console.error('Error al cargar las notificaciones'));
}

function countNotificaciones() {
    fetch('/AlertasCuentaPagar/GetNotificacionesCount')
        .then(response => response.json())
        .then(data => {
            document.getElementById('notificationCount').textContent = data.count;
        })
        .catch(error => console.error('Error al cargar las notificaciones'));
}