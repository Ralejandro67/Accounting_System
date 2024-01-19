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

        public CantonesController(ICantonesRep cantonesRep)
        {
            _cantonesRep = cantonesRep;
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cantones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCanton,NombreCanton,IdProvincia")] Canton canton)
        {
            if (ModelState.IsValid)
            {
                await _cantonesRep.CrearCanton(canton.NombreCanton, canton.IdProvincia);
                return RedirectToAction(nameof(Index));
            }
            return View(canton);
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
