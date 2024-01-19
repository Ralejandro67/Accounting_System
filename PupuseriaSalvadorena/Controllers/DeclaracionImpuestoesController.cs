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
    public class DeclaracionImpuestoesController : Controller
    {
        private readonly IDeclaracionTaxRep _declaracionTaxRep;

        public DeclaracionImpuestoesController(IDeclaracionTaxRep context)
        {
            _declaracionTaxRep = context;
        }

        // GET: DeclaracionImpuestoes
        public async Task<IActionResult> Index()
        {
            var declaracionImpuesto = await _declaracionTaxRep.MostrarDeclaracionesImpuestos();
            return View(declaracionImpuesto);   
        }

        // GET: DeclaracionImpuestoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var declaracionImpuesto = await _declaracionTaxRep.ConsultarDeclaracionesImpuestos(id);
            if (declaracionImpuesto == null)
            {
                return NotFound();
            }

            return View(declaracionImpuesto);
        }

        // GET: DeclaracionImpuestoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeclaracionImpuestoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDeclaracionImpuesto,CedulaJuridica,FechaInicio,FechaFinal,MontoTotalIngresos,MontoTotalEgresos,MontoTotalImpuestos,Observaciones")] DeclaracionImpuesto declaracionImpuesto)
        {
            if (ModelState.IsValid)
            {
                await _declaracionTaxRep.CrearDeclaracionTax(declaracionImpuesto.CedulaJuridica, declaracionImpuesto.FechaInicio, declaracionImpuesto.FechaFinal, declaracionImpuesto.MontoTotalIngresos, declaracionImpuesto.MontoTotalEgresos, declaracionImpuesto.MontoTotalImpuestos, declaracionImpuesto.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(declaracionImpuesto);
        }

        // GET: DeclaracionImpuestoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var declaracionImpuesto = await _declaracionTaxRep.ConsultarDeclaracionesImpuestos(id);
            if (declaracionImpuesto == null)
            {
                return NotFound();
            }
            return View(declaracionImpuesto);
        }

        // POST: DeclaracionImpuestoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDeclaracionImpuesto,CedulaJuridica,FechaInicio,FechaFinal,MontoTotalIngresos,MontoTotalEgresos,MontoTotalImpuestos,Observaciones")] DeclaracionImpuesto declaracionImpuesto)
        {
            if (id != declaracionImpuesto.IdDeclaracionImpuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _declaracionTaxRep.ActualizarDeclaracionTax(declaracionImpuesto.IdDeclaracionImpuesto, declaracionImpuesto.FechaInicio, declaracionImpuesto.FechaFinal, declaracionImpuesto.MontoTotalIngresos, declaracionImpuesto.MontoTotalEgresos, declaracionImpuesto.MontoTotalImpuestos, declaracionImpuesto.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(declaracionImpuesto);
        }

        // GET: DeclaracionImpuestoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var declaracionImpuesto = await _declaracionTaxRep.ConsultarDeclaracionesImpuestos(id);
            if (declaracionImpuesto == null)
            {
                return NotFound();
            }

            return View(declaracionImpuesto);
        }

        // POST: DeclaracionImpuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _declaracionTaxRep.EliminarDeclaracionTax(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
