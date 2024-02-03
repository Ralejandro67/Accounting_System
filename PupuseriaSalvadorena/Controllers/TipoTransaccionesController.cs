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
    public class TipoTransaccionesController : Controller
    {
        private readonly ITipoTransacRep _tipoTransacRep;  
        private readonly ITipoMovimientoRep _movimientosRep;
        private readonly IImpuestosRep _impuestosRep;

        public TipoTransaccionesController(ITipoTransacRep context, ITipoMovimientoRep movimientosRep, IImpuestosRep impuestosRep)
        {
            _tipoTransacRep = context;
            _movimientosRep = movimientosRep;
            _impuestosRep = impuestosRep;
        }

        // GET: TipoTransacciones
        public async Task<IActionResult> Index()
        {
            var tipoTransacciones = await _tipoTransacRep.MostrarTipoTransaccion();
            return View(tipoTransacciones);
        }

        // GET: TipoTransacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTransacciones = await _tipoTransacRep.ConsultarTipoTransaccion(id.Value);
            if (tipoTransacciones == null)
            {
                return NotFound();
            }

            return View(tipoTransacciones);
        }

        // GET: TipoTransacciones/Create
        public async Task<IActionResult> Create()
        {
            var movimientos = await _movimientosRep.MostrarTipoMovimiento();
            var impuestos = await _impuestosRep.MostrarImpuestos();

            ViewBag.Movimientos = new SelectList(movimientos, "IdMovimiento", "NombreMov");
            ViewBag.Impuestos = new SelectList(impuestos, "IdImpuesto", "NombreImpuesto");

            return PartialView("_newTipoTPartial", new TipoTransacciones());
        }

        // POST: TipoTransacciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipo,TipoTransac,IdMovimiento,IdImpuesto")] TipoTransacciones tipoTransacciones)
        {
            if (ModelState.IsValid)
            {
                await _tipoTransacRep.CrearTipoTransac(tipoTransacciones.TipoTransac, tipoTransacciones.IdMovimiento, tipoTransacciones.IdImpuesto);
                return Json(new { success = true, message = "Tipo de transaccion agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el tipo de transaccion." });
        }

        // GET: TipoTransacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTransacciones = await _tipoTransacRep.ConsultarTipoTransaccion(id.Value);
            var movimientos = await _movimientosRep.MostrarTipoMovimiento();
            var impuestos = await _impuestosRep.MostrarImpuestos();
            ViewBag.Movimientos = new SelectList(movimientos, "IdMovimiento", "NombreMov");
            ViewBag.Impuestos = new SelectList(impuestos, "IdImpuesto", "NombreImpuesto");

            if (tipoTransacciones == null)
            {
                return NotFound();
            }
            return PartialView("_editTipoTPartial", tipoTransacciones);
        }

        // POST: TipoTransacciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipo,TipoTransac,IdMovimiento,IdImpuesto")] TipoTransacciones tipoTransacciones)
        {
            if (id != tipoTransacciones.IdTipo)
            {
                return Json(new { success = false, message = "Tipo de transaccion no encontrada." });
            }

            if (ModelState.IsValid)
            {
                await _tipoTransacRep.ActualizarTipoTransac(tipoTransacciones.IdTipo, tipoTransacciones.TipoTransac, tipoTransacciones.IdMovimiento, tipoTransacciones.IdImpuesto);
                return Json(new { success = true, message = "Tipo de transaccion actualizada correctamente." });
            }
            return Json(new { success = false, message = "Datos inválidos." });
        }

        // GET: TipoTransacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var tipoTransacciones = await _tipoTransacRep.ConsultarTipoTransaccion(id.Value);
            return Json(new { exists = tipoTransacciones != null });
        }

        // POST: TipoTransacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tipoTransacRep.EliminarTipoTransac(id);
                return Json(new { success = true, message = "Tipo de transaccion eliminado correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el tipo de transaccion." });
            }
        }
    }
}