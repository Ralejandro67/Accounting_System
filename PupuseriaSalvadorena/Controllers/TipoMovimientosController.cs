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
    public class TipoMovimientosController : Controller
    {
        private readonly ITipoMovimientoRep _tipoMovimientoRep;

        public TipoMovimientosController(ITipoMovimientoRep tipoMovimientoRep)
        {
            _tipoMovimientoRep = tipoMovimientoRep;
        }

        // GET: TipoMovimientos
        public async Task<IActionResult> Index()
        {
            var tipoMovimientos = await _tipoMovimientoRep.MostrarTipoMovimiento();
            return View(tipoMovimientos);
        }

        // GET: TipoMovimientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMovimiento = await _tipoMovimientoRep.ConsultarTipoMovimiento(id.Value);
            if (tipoMovimiento == null)
            {
                return NotFound();
            }

            return View(tipoMovimiento);
        }

        // GET: TipoMovimientos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoMovimientos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMovimiento,NombreMov")] TipoMovimiento tipoMovimiento)
        {
            if (ModelState.IsValid)
            {
                await _tipoMovimientoRep.CrearTipoMovimiento(tipoMovimiento.NombreMov);
                return Json(new { success = true, message = "Tipo de movimiento agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el tipo de movimiento." });
        }

        // GET: TipoMovimientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMovimiento = await _tipoMovimientoRep.ConsultarTipoMovimiento(id.Value);    
            if (tipoMovimiento == null)
            {
                return NotFound();
            }
            return PartialView("_editTipoMovimientoPartial", tipoMovimiento);
        }

        // POST: TipoMovimientos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMovimiento,NombreMov")] TipoMovimiento tipoMovimiento)
        {
            if (id != tipoMovimiento.IdMovimiento)
            {
                return Json(new { success = false, message = "Tipo de movimiento no encontrado." });
            }

            if (ModelState.IsValid)
            {
                await _tipoMovimientoRep.ActualizarTipoMovimiento(tipoMovimiento.IdMovimiento, tipoMovimiento.NombreMov);
                return Json(new { success = true, message = "Tipo de movimiento actualizado correctamente." });
            }
            return Json(new { success = false, message = "Error al actualizar el tipo de movimiento." });
        }

        // GET: TipoMovimientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var tipoMovimiento = await _tipoMovimientoRep.ConsultarTipoMovimiento(id.Value);
            return Json(new { exists = tipoMovimiento != null });
        }

        // POST: TipoMovimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tipoMovimientoRep.EliminarTipoMovimiento(id);
                return Json(new { success = true, message = "Tipo de movimiento eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el tipo de movimiento." });
            }
        }
    }
}
