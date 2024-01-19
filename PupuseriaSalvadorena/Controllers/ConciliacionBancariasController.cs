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
    public class ConciliacionBancariasController : Controller
    {
        private readonly IConciliacionRep _conciliacionRep;

        public ConciliacionBancariasController(IConciliacionRep context)
        {
            _conciliacionRep = context;
        }

        // GET: ConciliacionBancarias
        public async Task<IActionResult> Index()
        {
            var conciliacionBancarias = await _conciliacionRep.MostrarConciliacionesBancarias();
            return View(conciliacionBancarias);
        }

        // GET: ConciliacionBancarias/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }

            return View(conciliacionBancaria);
        }

        // GET: ConciliacionBancarias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConciliacionBancarias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConciliacion,FechaConciliacion,SaldoBancario,SaldoLibro,Diferencia,Observaciones,IdRegistro,IdRegistroLibros")] ConciliacionBancaria conciliacionBancaria)
        {
            if (ModelState.IsValid)
            {
                await _conciliacionRep.CrearConciliacion(conciliacionBancaria.FechaConciliacion, conciliacionBancaria.SaldoBancario, conciliacionBancaria.SaldoLibro, conciliacionBancaria.Diferencia, conciliacionBancaria.Observaciones, conciliacionBancaria.IdRegistro, conciliacionBancaria.IdRegistroLibros);
                return RedirectToAction(nameof(Index));
            }
            return View(conciliacionBancaria);
        }

        // GET: ConciliacionBancarias/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }
            return View(conciliacionBancaria);
        }

        // POST: ConciliacionBancarias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdConciliacion,FechaConciliacion,SaldoBancario,SaldoLibro,Diferencia,Observaciones,IdRegistro,IdRegistroLibros")] ConciliacionBancaria conciliacionBancaria)
        {
            if (id != conciliacionBancaria.IdConciliacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _conciliacionRep.ActualizarConciliacion(conciliacionBancaria.IdConciliacion, conciliacionBancaria.SaldoBancario, conciliacionBancaria.SaldoLibro, conciliacionBancaria.Diferencia, conciliacionBancaria.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(conciliacionBancaria);
        }

        // GET: ConciliacionBancarias/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }

            return View(conciliacionBancaria);
        }

        // POST: ConciliacionBancarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _conciliacionRep.EliminarConciliacion(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
