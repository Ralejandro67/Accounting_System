using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
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
        public async Task<IActionResult> Index()
        {
            var alertasCuentaPagar = await _alertaCuentaPagarRep.MostrarAlertaCuentaPagar();
            return View(alertasCuentaPagar);
        }

        // GET: AlertasCuentaPagar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alertasCuentaPagar = await _alertaCuentaPagarRep.ConsultarAlertaCuentaPagar(id.Value);
            if (alertasCuentaPagar == null)
            {
                return NotFound();
            }

            return View(alertasCuentaPagar);
        }

        // GET: AlertasCuentaPagar/Create
        public async Task<IActionResult> Create()
        {
            var cuentaPagar = await _cuentaPagarRep.MostrarCuentasPagar();
            ViewBag.CuentaPagar = new SelectList(cuentaPagar, "IdCuentaPagar", "IdCuentaPagar");
            return PartialView("_newAlertaCuentaPagarPartial", new AlertaCuentaPagar());
        }

        // POST: AlertasCuentaPagar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAlerta,Mensaje,FechaMensaje,IdCuentaPagar,Leido")] AlertaCuentaPagar alertaCuentaPagar)
        {
            if (ModelState.IsValid)
            {
                await _alertaCuentaPagarRep.CrearAlertaCuentaPagar(alertaCuentaPagar.Mensaje, alertaCuentaPagar.FechaMensaje, alertaCuentaPagar.IdCuentaPagar, alertaCuentaPagar.Leido);
                return Json(new { success = true, message = "Alerta agregada correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la alerta." });
        }

        // GET: AlertasCuentaPagar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alertaCuentaPagar = await _alertaCuentaPagarRep.ConsultarAlertaCuentaPagar(id.Value);
            if (alertaCuentaPagar == null)
            {
                return NotFound();
            }
            return PartialView("_editAlertaCuentaPagarPartial", alertaCuentaPagar);
        }

        // POST: AlertasCuentaPagar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAlerta,Mensaje,FechaMensaje,IdCuentaPagar,Leido")] AlertaCuentaPagar alertaCuentaPagar)
        {
            if (id != alertaCuentaPagar.IdAlerta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _alertaCuentaPagarRep.ActualizarAlertaCuentaPagar(alertaCuentaPagar.IdAlerta, alertaCuentaPagar.Mensaje, alertaCuentaPagar.FechaMensaje, alertaCuentaPagar.IdCuentaPagar, alertaCuentaPagar.Leido);
                return Json(new { success = true, message = "Alerta actualizada correctamente." });
            }
            return Json(new { success = false, message = "Datos inválidos." });
        }

        // GET: AlertasCuentaPagar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var alertaCuentaPagar = await _alertaCuentaPagarRep.ConsultarAlertaCuentaPagar(id.Value);
            return Json(new { exists = alertaCuentaPagar != null });
        }

        // POST: AlertasCuentaPagar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
