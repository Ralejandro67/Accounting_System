using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Filtros;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class AlertasCuentaPagarController : Controller
    {
        private readonly IAlertaCuentaPagarRep _alertaCuentaPagarRep;
        private readonly ICuentaPagarRep _cuentaPagarRep;

        public AlertasCuentaPagarController(IAlertaCuentaPagarRep alertaCuentaPagarRep, ICuentaPagarRep cuentaPagarRep)
        {
            _alertaCuentaPagarRep = alertaCuentaPagarRep;
            _cuentaPagarRep = cuentaPagarRep;
        }

        // GET: AlertasCuentaPagar
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var alertasCuentaPagar = await _alertaCuentaPagarRep.MostrarAlertaCuentaPagar();
            return View(alertasCuentaPagar);
        }

        // GET: AlertasCuentaPagar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var alertaCuentaPagar = await _alertaCuentaPagarRep.ConsultarAlertaCuentaPagar(id.Value);
            return Json(new { exists = alertaCuentaPagar != null });
        }

        // POST: AlertasCuentaPagar/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _alertaCuentaPagarRep.EliminarAlertaCuentaPagar(id);
                return Json(new { success = true, message = "Alerta eliminada correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar la alerta." });
            }
        }

        [HttpPost]
        public async Task <IActionResult> DeleteAll()
        {
            try
            {
                var alertas = await _alertaCuentaPagarRep.MostrarAlertasLeidas();

                if (alertas.Count == 0)
                {
                    return Json(new { success = false, message = "No hay alertas para eliminar." });
                }

                await _alertaCuentaPagarRep.EliminarAlertasLeidas();
                return Json(new { success = true, message = "Alertas eliminadas correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar las alertas leidas." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarcarLeido()
        {
            try
            {
                var alertas = await _alertaCuentaPagarRep.MostrarAlertasNoLeidas();

                if (alertas.Count == 0)
                {
                    return Json(new { success = false, message = "No hay alertas para marcar como leidas." });
                }

                await _alertaCuentaPagarRep.ActualizarAlertasNoLeidas();
                return Json(new { success = true, message = "Alertas marcadas como leidas." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al marcar las alertas como leidas." });
            }
        }

        // GET: AlertasCuentaPagar/GetNotificaciones
        public async Task<IActionResult> GetNotificaciones()
        {
            var notificaciones = await _alertaCuentaPagarRep.MostrarAlertasNoLeidas();
            return PartialView("_notificaciones", notificaciones);
        }

        // GET: AlertasCuentaPagar/GetNotificacionesCount
        public async Task<IActionResult> GetNotificacionesCount()
        {
            var notificaciones = await _alertaCuentaPagarRep.MostrarAlertasNoLeidas();
            return Json(new { count = notificaciones.Count() });
        }
    }
}
