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
    public class DireccionesController : Controller
    {
        private readonly IDireccionesRep _direccionesRep;
        private readonly IDistritosRep _distritosRep;

        public DireccionesController(IDireccionesRep context, IDistritosRep distritosRep)
        {
            _direccionesRep = context;
            _distritosRep = distritosRep;
        }

        // GET: Direcciones
        public async Task<IActionResult> Index()
        {
            var direcciones = await _direccionesRep.MostrarDirecciones();
            return View(direcciones);
        }

        // GET: Direcciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _direccionesRep.ConsultarDirecciones(id.Value);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // GET: Direcciones/Create
        public async Task<IActionResult> Create()
        {
            var distritos = await _distritosRep.MostrarDistritos();
            ViewBag.Direcciones = new SelectList(distritos, "IdDistrito", "NombreDistrito");
            return PartialView("_newDireccionPartial", new Direccion());
        }

        // POST: Direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDireccion,Estado,Detalles,IdDistrito")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                await _direccionesRep.CrearDireccion(direccion.Estado, direccion.Detalles, direccion.IdDistrito);
                return Json(new { success = true, message = "Direccion agregada correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la direccion." });
        }

        // GET: Direcciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _direccionesRep.ConsultarDirecciones(id.Value);
            if (direccion == null)
            {
                return NotFound();
            }
            return View(direccion);
        }

        // POST: Direcciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDireccion,Estado,Detalles,IdDistrito")] Direccion direccion)
        {
            if (id != direccion.IdDireccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _direccionesRep.ActualizarDireccion(direccion.IdDireccion, direccion.Estado, direccion.Detalles, direccion.IdDistrito);
                return RedirectToAction(nameof(Index));
            }
            return View(direccion);
        }

        // GET: Direcciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _direccionesRep.ConsultarDirecciones(id.Value);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // POST: Direcciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _direccionesRep.EliminarDireccion(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
