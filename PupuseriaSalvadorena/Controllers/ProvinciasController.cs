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
    public class ProvinciasController : Controller
    {
        private readonly IProvinciasRep _provinciasRep;

        public ProvinciasController(IProvinciasRep context)
        {
            _provinciasRep = context;
        }

        // GET: Provincias
        public async Task<IActionResult> Index()
        {
            var provincias = await _provinciasRep.MostrarProvincias();
            return View(provincias);
        }

        // GET: Provincias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincia = await _provinciasRep.ConsultarProvincias(id.Value);
            if (provincia == null)
            {
                return NotFound();
            }

            return View(provincia);
        }

        // GET: Provincias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Provincias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProvincia,NombreProvincia")] Provincia provincia)
        {
            if (ModelState.IsValid)
            {
                await _provinciasRep.CrearProvincia(provincia.NombreProvincia);
                return Json(new { success = true, message = "Provincia agregada correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la provincia." });
        }

        // GET: Provincias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincia = await _provinciasRep.ConsultarProvincias(id.Value);
            if (provincia == null)
            {
                return NotFound();
            }
            return View(provincia);
        }

        // POST: Provincias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProvincia,NombreProvincia")] Provincia provincia)
        {
            if (id != provincia.IdProvincia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _provinciasRep.ActualizarProvincia(provincia.IdProvincia, provincia.NombreProvincia);
                return RedirectToAction(nameof(Index));
            }
            return View(provincia);
        }

        // GET: Provincias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincia = await _provinciasRep.ConsultarProvincias(id.Value);
            if (provincia == null)
            {
                return NotFound();
            }

            return View(provincia);
        }

        // POST: Provincias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _provinciasRep.EliminarProvincia(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
