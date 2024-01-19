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
    public class DistritosController : Controller
    {
        private readonly IDistritosRep _distritosRep;

        public DistritosController(IDistritosRep context)
        {
            _distritosRep = context;
        }

        // GET: Distritos
        public async Task<IActionResult> Index()
        {
            var distritos = await _distritosRep.MostrarDistritos();
            return View(distritos);
        }

        // GET: Distritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _distritosRep.ConsultarDistritos(id.Value);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }

        // GET: Distritos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distritos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDistrito,NombreDistrito,IdCanton")] Distrito distrito)
        {
            if (ModelState.IsValid)
            {
                await _distritosRep.CrearDistrito(distrito.NombreDistrito, distrito.IdCanton);
                return RedirectToAction(nameof(Index));
            }
            return View(distrito);
        }

        // GET: Distritos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _distritosRep.ConsultarDistritos(id.Value);
            if (distrito == null)
            {
                return NotFound();
            }
            return View(distrito);
        }

        // POST: Distritos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDistrito,NombreDistrito,IdCanton")] Distrito distrito)
        {
            if (id != distrito.IdDistrito)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _distritosRep.ActualizarDistrito(distrito.IdDistrito, distrito.NombreDistrito, distrito.IdCanton);
                return RedirectToAction(nameof(Index));
            }
            return View(distrito);
        }

        // GET: Distritos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _distritosRep.ConsultarDistritos(id.Value);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }

        // POST: Distritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _distritosRep.EliminarDistrito(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
