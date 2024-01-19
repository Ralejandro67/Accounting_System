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
    public class CategoriaPresupuestoesController : Controller
    {
        private readonly ICatPresupuestoRep _categoriaPresupuestoRep;

        public CategoriaPresupuestoesController(ICatPresupuestoRep context)
        {
            _categoriaPresupuestoRep = context;
        }

        // GET: CategoriaPresupuestoes
        public async Task<IActionResult> Index()
        {
            var categoriaPresupuesto = await _categoriaPresupuestoRep.MostrarCatPresupuestos();
            return View(categoriaPresupuesto);
        }

        // GET: CategoriaPresupuestoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaPresupuesto = await _categoriaPresupuestoRep.ConsultarCatPresupuestos(id.Value);
            if (categoriaPresupuesto == null)
            {
                return NotFound();
            }

            return View(categoriaPresupuesto);
        }

        // GET: CategoriaPresupuestoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaPresupuestoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoriaP,NombreCategoriaP,Estado")] CategoriaPresupuesto categoriaPresupuesto)
        {
            if (ModelState.IsValid)
            {
                await _categoriaPresupuestoRep.CrearCatPresupuesto(categoriaPresupuesto.NombreCategoriaP, categoriaPresupuesto.Estado);
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaPresupuesto);
        }

        // GET: CategoriaPresupuestoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaPresupuesto = await _categoriaPresupuestoRep.ConsultarCatPresupuestos(id.Value);
            if (categoriaPresupuesto == null)
            {
                return NotFound();
            }
            return View(categoriaPresupuesto);
        }

        // POST: CategoriaPresupuestoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoriaP,NombreCategoriaP,Estado")] CategoriaPresupuesto categoriaPresupuesto)
        {
            if (id != categoriaPresupuesto.IdCategoriaP)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _categoriaPresupuestoRep.ActualizarCatPresupuestos(categoriaPresupuesto.IdCategoriaP, categoriaPresupuesto.NombreCategoriaP, categoriaPresupuesto.Estado);
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaPresupuesto);
        }

        // GET: CategoriaPresupuestoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaPresupuesto = await _categoriaPresupuestoRep.ConsultarCatPresupuestos(id.Value);
            if (categoriaPresupuesto == null)
            {
                return NotFound();
            }

            return View(categoriaPresupuesto);
        }

        // POST: CategoriaPresupuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoriaPresupuestoRep.EliminarCatPresupuestos(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
