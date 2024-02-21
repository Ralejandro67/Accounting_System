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
    public class TipoVentasController : Controller
    {
        private readonly ITipoVentaRep _tipoVentaRep;

        public TipoVentasController(ITipoVentaRep context)
        {
            _tipoVentaRep = context;
        }

        // GET: TipoVentas
        public async Task<IActionResult> Index()
        {
            var tipoVentas = await _tipoVentaRep.MostrarTipoVentas();
            return View(tipoVentas);    
        }

        // GET: TipoVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            if (tipoVenta == null)
            {
                return NotFound();
            }

            return View(tipoVenta);
        }

        // GET: TipoVentas/Create
        public IActionResult Create()
        {
            return PartialView("_newTipoVPartial", new TipoVenta());
        }

        // POST: TipoVentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoVenta,NombreVenta,Estado")] TipoVenta tipoVenta)
        {
            if (ModelState.IsValid)
            {
                await _tipoVentaRep.CrearTipoVenta(tipoVenta.NombreVenta, tipoVenta.Estado);
                return Json(new { success = true, message = "Tipo de venta agregada correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el tipo de venta." });
        }

        // GET: TipoVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Error al editar el tipo de venta." });
            }

            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            if (tipoVenta == null)
            {
                return Json(new { success = false, message = "No se encontro el tipo de venta." });
            }
            return PartialView("_editTipoVPartial", tipoVenta);
        }

        // POST: TipoVentas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoVenta,NombreVenta,Estado")] TipoVenta tipoVenta)
        {
            if (id != tipoVenta.IdTipoVenta)
            {
                return Json(new { success = false, message = "Error al editar el tipo de venta." });
            }

            if (ModelState.IsValid)
            {
                await _tipoVentaRep.ActualizarTipoVentas(tipoVenta.IdTipoVenta, tipoVenta.NombreVenta, tipoVenta.Estado);
                return Json(new { success = true, message = "Tipo de venta actualizada correctamente." });
            }
            return Json(new { success = false, message = "Error al editar el tipo de venta." });
        }

        // GET: TipoVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            return Json(new { exists = tipoVenta != null });
        }

        // POST: TipoVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try 
            {                 
                await _tipoVentaRep.EliminarTipoVenta(id);
                return Json(new { success = true, message = "Tipo de venta eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el tipo de venta." });
            }
        }
    }
}
