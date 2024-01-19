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
    public class PresupuestoesController : Controller
    {
        private readonly IPresupuestoRep _presupuestoRep;

        public PresupuestoesController(IPresupuestoRep context)
        {
            _presupuestoRep = context;
        }

        // GET: Presupuestoes
        public async Task<IActionResult> Index()
        {
            var presupuestos = await _presupuestoRep.MostrarPresupuestos();
            return View(presupuestos); 
        }

        // GET: Presupuestoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(id);
            if (presupuesto == null)
            {
                return NotFound();
            }

            return View(presupuesto);
        }

        // GET: Presupuestoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Presupuestoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPresupuesto,NombreP,FechaInicio,FechaFin,Descripcion,SaldoUsado,SaldoPresupuesto,Estado,IdCategoriaP")] Presupuesto presupuesto)
        {
            if (ModelState.IsValid)
            {
                await _presupuestoRep.CrearPresupuesto(presupuesto.NombreP, presupuesto.FechaInicio, presupuesto.FechaFin, presupuesto.Descripcion, presupuesto.SaldoUsado, presupuesto.SaldoPresupuesto, presupuesto.Estado, presupuesto.IdCategoriaP);
                return RedirectToAction(nameof(Index));
            }
            return View(presupuesto);
        }

        // GET: Presupuestoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(id);
            if (presupuesto == null)
            {
                return NotFound();
            }
            return View(presupuesto);
        }

        // POST: Presupuestoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdPresupuesto,NombreP,FechaInicio,FechaFin,Descripcion,SaldoUsado,SaldoPresupuesto,Estado,IdCategoriaP")] Presupuesto presupuesto)
        {
            if (id != presupuesto.IdPresupuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _presupuestoRep.ActualizarPresupuesto(presupuesto.IdPresupuesto, presupuesto.NombreP, presupuesto.FechaInicio, presupuesto.FechaFin, presupuesto.Descripcion, presupuesto.SaldoUsado, presupuesto.SaldoPresupuesto, presupuesto.Estado, presupuesto.IdCategoriaP);
                return RedirectToAction(nameof(Index));
            }
            return View(presupuesto);
        }

        // GET: Presupuestoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(id);
            if (presupuesto == null)
            {
                return NotFound();
            }

            return View(presupuesto);
        }

        // POST: Presupuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _presupuestoRep.EliminarPresupuesto(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
