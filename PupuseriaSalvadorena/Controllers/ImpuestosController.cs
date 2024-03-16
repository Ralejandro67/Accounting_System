using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
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
    public class ImpuestosController : Controller
    {
        private readonly IImpuestosRep _impuestosRep;

        public ImpuestosController(IImpuestosRep context)
        {
            _impuestosRep = context;
        }

        // GET: Impuestos
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var impuestos = await _impuestosRep.MostrarImpuestos();
            return View(impuestos);
        }

        // GET: Impuestos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impuesto = await _impuestosRep.ConsultarImpuestos(id);
            if (impuesto == null)
            {
                return NotFound();
            }

            return View(impuesto);
        }

        // GET: Impuestos/Create
        public async Task<IActionResult> Create()
        {
            return PartialView("_newImpuestoPartial", new Impuesto());
        }

        // POST: Impuestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdImpuesto,NombreImpuesto,Tasa,Estado,Descripcion,IdTipo")] Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _impuestosRep.CrearImpuesto(impuesto.NombreImpuesto, impuesto.Tasa.Value, impuesto.Estado, impuesto.Descripcion);
                    return Json(new { success = true, message = "Impuesto agregado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Ya existe un impuesto llamado {impuesto.NombreImpuesto}." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: Impuestos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Impuesto no encontrado." });
            }

            var impuesto = await _impuestosRep.ConsultarImpuestos(id);

            if (impuesto == null)
            {
               return Json(new { success = false, message = "Impuesto no encontrado." });
            }
            return PartialView("_EditImpuestoPartial", impuesto);
        }

        // POST: Impuestos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdImpuesto,NombreImpuesto,Tasa,Estado,Descripcion,IdTipo")] Impuesto impuesto)
        {
            if (id != impuesto.IdImpuesto)
            {
                return Json(new { success = false, message = "Impuesto no encontrado." });
            }

            if (ModelState.IsValid)
            {
                var tax = await _impuestosRep.ConsultarImpuestos(id);

                if (tax.NombreImpuesto != impuesto.NombreImpuesto)
                {
                    var exist = await _impuestosRep.MostrarImpuestos();
                    var existImpuesto = exist.Where(i => i.NombreImpuesto == impuesto.NombreImpuesto).ToList();

                    if (existImpuesto.Count > 0)
                    {
                        return Json(new { success = false, message = $"Ya existe un impuesto llamado {impuesto.NombreImpuesto}." });
                    }
                }

                await _impuestosRep.ActualizarImpuesto(impuesto.IdImpuesto, impuesto.NombreImpuesto, impuesto.Tasa.Value, impuesto.Estado, impuesto.Descripcion);
                return Json(new { success = true, message = "Impuesto actualizado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: Impuestos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var impuesto = await _impuestosRep.ConsultarImpuestos(id);
            return Json(new { exists = impuesto != null });
        }

        // POST: Impuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _impuestosRep.EliminarImpuesto(id);
                return Json(new { success = true, message = "Impuesto eliminado correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el impuesto, hay transaciones asociadas a este impuesto." });
            }
        }
    }
}
