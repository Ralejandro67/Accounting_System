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
    public class CantonesController : Controller
    {
        private readonly ICantonesRep _cantonesRep;
        private readonly IProvinciasRep _provinciasRep;

        public CantonesController(ICantonesRep cantonesRep, IProvinciasRep provinciasRep)
        {
            _cantonesRep = cantonesRep;
            _provinciasRep = provinciasRep;
        }

        // GET: Cantones
        public async Task<IActionResult> Index()
        {
            var cantones = await _cantonesRep.MostrarCantones();
            return View(cantones);
        }

        // GET: Cantones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var canton = await _cantonesRep.ConsultarCantones(id.Value);
            if (canton == null)
            {
                return NotFound();
            }

            return View(canton);
        }

        // GET: Cantones/Create
        public async Task<IActionResult> Create()
        {
            var provincias = await _provinciasRep.MostrarProvincias();
            ViewBag.Provincias = new SelectList(provincias, "IdProvincia", "NombreProvincia");
            return PartialView("_newCantonPartial", new Canton());
        }

        // POST: Cantones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCanton,NombreCanton,IdProvincia")] Canton canton)
        {
            if (ModelState.IsValid)
            {
                await _cantonesRep.CrearCanton(canton.NombreCanton, canton.IdProvincia);
                return Json(new { success = true, message = "Canton agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el canton." });
        }

        // GET: Cantones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var canton = await _cantonesRep.ConsultarCantones(id.Value);
            if (canton == null)
            {
                return NotFound();
            }
            return View(canton);
        }

        // POST: Cantones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCanton,NombreCanton,IdProvincia")] Canton canton)
        {
            if (id != canton.IdCanton)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _cantonesRep.ActualizarCanton(canton.IdCanton, canton.NombreCanton, canton.IdProvincia);
                return RedirectToAction(nameof(Index));
            }
            return View(canton);
        }

        // GET: Cantones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var canton = await _cantonesRep.ConsultarCantones(id.Value);
            if (canton == null)
            {
                return NotFound();
            }

            return View(canton);
        }

        // POST: Cantones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cantonesRep.EliminarCanton(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
