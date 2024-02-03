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
    public class TipoFacturasController : Controller
    {
        private readonly ITipoFacturaRep _tipoFacturaRep;   

        public TipoFacturasController(ITipoFacturaRep context)
        {
            _tipoFacturaRep = context;
        }

        // GET: TipoFacturas
        public async Task<IActionResult> Index()
        {
            var tipoFacturas = await _tipoFacturaRep.MostrarTipoFacturas();
            return View(tipoFacturas);
        }

        // GET: TipoFacturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoFactura = await _tipoFacturaRep.ConsultarTipoFacturas(id.Value);
            if (tipoFactura == null)
            {
                return NotFound();
            }

            return View(tipoFactura);
        }

        // GET: TipoFacturas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoFacturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoFactura,NombreFactura,Estado")] TipoFactura tipoFactura)
        {
            if (ModelState.IsValid)
            {
                await _tipoFacturaRep.CrearTipoFactura(tipoFactura.NombreFactura, tipoFactura.Estado);
                return Json(new { success = true, message = "Tipo de factura agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el tipo de factura." });
        }

        // GET: TipoFacturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoFactura = await _tipoFacturaRep.ConsultarTipoFacturas(id.Value);
            if (tipoFactura == null)
            {
                return NotFound();
            }
            return PartialView("_editTipoFPartial", tipoFactura);
        }

        // POST: TipoFacturas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoFactura,NombreFactura,Estado")] TipoFactura tipoFactura)
        {
            if (id != tipoFactura.IdTipoFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tipoFacturaRep.ActualizarTipoFactura(tipoFactura.IdTipoFactura, tipoFactura.NombreFactura, tipoFactura.Estado);
                return Json(new { success = true, message = "Tipo de factura actualizada correctamente." });
            }
            return Json(new { success = false, message = "Error al actualizar el tipo de factura." });
        }

        // GET: TipoFacturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var tipoFactura = await _tipoFacturaRep.ConsultarTipoFacturas(id.Value);
            return Json(new { exists = tipoFactura != null });
        }

        // POST: TipoFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tipoFacturaRep.EliminarTipoFactura(id);
                return Json(new { success = true, message = "Tipo de factura eliminada correctamente." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al eliminar el tipo de factura." });
            }
        }
    }
}
