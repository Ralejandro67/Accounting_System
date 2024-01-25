using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class DetalleCuentasController : Controller
    {
        private readonly IDetallesCuentaRep _detallesCuentaRep;
        private readonly ICuentaPagarRep _cuentasPagarRep;

        public DetalleCuentasController(IDetallesCuentaRep context, ICuentaPagarRep cuentasPagarRep)
        {
            _detallesCuentaRep = context;
            _cuentasPagarRep = cuentasPagarRep;
        }

        // GET: DetalleCuentas
        public async Task<IActionResult> Index()
        {
            var detalleCuentas = await _detallesCuentaRep.MostrarDetallesCuenta();
            return View(detalleCuentas);
        }

        // GET: DetalleCuentas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCuenta = await _detallesCuentaRep.ConsultarDetallesCuentas(id);
            if (detalleCuenta == null)
            {
                return NotFound();
            }

            return View(detalleCuenta);
        }

        // GET: DetalleCuentas/Create
        public async Task<IActionResult> Create()
        {
            var cuentasPagar = await _cuentasPagarRep.MostrarCuentasPagar();
            ViewBag.CuentasPagar = new SelectList(cuentasPagar, "IdCuentaPagar", "IdCuentaPagar");
            return PartialView("_newDetalleCuentaPartial", new DetalleCuenta());
        }

        // POST: DetalleCuentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetallesCuenta,Pago,FechaIngreso,IdCuentaPagar")] DetalleCuenta detalleCuenta)
        {
            if (ModelState.IsValid)
            {
                await _detallesCuentaRep.CrearDetallesCuenta(detalleCuenta.Pago, detalleCuenta.FechaIngreso, detalleCuenta.IdCuentaPagar);
                return Json(new { success = true, message = "Pago agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la factura de compra." });
        }

        // GET: DetalleCuentas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCuenta = await _detallesCuentaRep.ConsultarDetallesCuentas(id);
            if (detalleCuenta == null)
            {
                return NotFound();
            }
            return View(detalleCuenta);
        }

        // POST: DetalleCuentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDetallesCuenta,Pago,FechaIngreso,IdCuentaPagar")] DetalleCuenta detalleCuenta)
        {
            if (id != detalleCuenta.IdDetallesCuenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _detallesCuentaRep.ActualizarDetallesCuentas(detalleCuenta.IdDetallesCuenta, detalleCuenta.Pago, detalleCuenta.FechaIngreso);
                return RedirectToAction(nameof(Index));
            }
            return View(detalleCuenta);
        }

        // GET: DetalleCuentas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCuenta = await _detallesCuentaRep.ConsultarDetallesCuentas(id);
            if (detalleCuenta == null)
            {
                return NotFound();
            }

            return View(detalleCuenta);
        }

        // POST: DetalleCuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _detallesCuentaRep.EliminarDetallesCuenta(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
