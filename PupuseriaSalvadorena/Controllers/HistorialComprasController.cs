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
    public class HistorialComprasController : Controller
    {
        private readonly IHistorialCompraRep _historialCompraRep;
        private readonly IMateriaPrimaRep _materiaPrimaRep;
        private readonly IFacturaCompraRep _facturaCompraRep;

        public HistorialComprasController(IHistorialCompraRep context, IMateriaPrimaRep materiaPrimaRep, IFacturaCompraRep facturaCompraRep)
        {
            _historialCompraRep = context;
            _materiaPrimaRep = materiaPrimaRep;
            _facturaCompraRep = facturaCompraRep;
        }

        // GET: HistorialCompras
        public async Task<IActionResult> Index()
        {
            var historialCompras = await _historialCompraRep.MostrarHistorialCompras();
            return View(historialCompras);
        }

        // GET: HistorialCompras/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
            if (historialCompra == null)
            {
                return NotFound();
            }

            return View(historialCompra);
        }

        // GET: HistorialCompras/Create
        public async Task<IActionResult> Create()
        {
            var materiasPrimas = await _materiaPrimaRep.MostrarMateriaPrima();
            var facturasCompras = await _facturaCompraRep.MostrarFacturasCompras();

            ViewBag.MateriasPrimas = new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima");
            ViewBag.FacturasCompras = new SelectList(facturasCompras, "IdFacturaCompra", "IdFacturaCompra");
            return PartialView("_newHistorialCPartial", new HistorialCompra());
        }

        // POST: HistorialCompras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdMateriaPrima,CantCompra,Precio,Peso,FechaCompra,IdFacturaCompra")] HistorialCompra historialCompra)
        {
            if (ModelState.IsValid)
            {
                await _historialCompraRep.CrearHistorialCompra(historialCompra.IdMateriaPrima, historialCompra.CantCompra, historialCompra.Precio, historialCompra.Peso, historialCompra.FechaCompra, historialCompra.IdFacturaCompra);
                return Json(new { success = true, message = "Compra agregada correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la compra." });
        }

        // GET: HistorialCompras/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);

            var materiasPrimas = await _materiaPrimaRep.MostrarMateriaPrima();
            var facturasCompras = await _facturaCompraRep.MostrarFacturasCompras();

            ViewBag.MateriasPrimas = new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima");
            ViewBag.FacturasCompras = new SelectList(facturasCompras, "IdFacturaCompra", "IdFacturaCompra");

            if (historialCompra == null)
            {
                return NotFound();
            }
            return PartialView("_editHistorialCPartial", historialCompra);
        }

        // POST: HistorialCompras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdCompra,IdMateriaPrima,CantCompra,Precio,Peso,FechaCompra,IdFacturaCompra")] HistorialCompra historialCompra)
        {
            if (id != historialCompra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _historialCompraRep.ActualizarHistorialCompra(historialCompra.IdCompra, historialCompra.IdMateriaPrima, historialCompra.CantCompra, historialCompra.Precio, historialCompra.Peso, historialCompra.FechaCompra, historialCompra.IdFacturaCompra);
                return Json(new { success = true, message = "Compra actualizada correctamente." });
            }
            return Json(new { success = false, message = "Error al actualizar la compra." });
        }

        // GET: HistorialCompras/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
            return Json(new { exists = historialCompra != null });
        }

        // POST: HistorialCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _historialCompraRep.EliminarHistorialCompra(id);
                return Json(new { success = true, message = "Compra eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la compra." });
            }
        }
    }
}
