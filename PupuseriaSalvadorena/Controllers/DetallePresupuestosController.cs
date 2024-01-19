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
    public class DetallePresupuestosController : Controller
    {
        private readonly IDetallesPresupuestoRep _detallesPresupuestoRep;

        public DetallePresupuestosController(IDetallesPresupuestoRep context)
        {
            _detallesPresupuestoRep = context;
        }

        // GET: DetallePresupuestos
        public async Task<IActionResult> Index()
        {
            var detallePresupuesto = await _detallesPresupuestoRep.MostrarDetallesPresupuesto();
            return View(detallePresupuesto);    
        }

        // GET: DetallePresupuestos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePresupuesto = await _detallesPresupuestoRep.ConsultarDetallesPresupuesto(id.Value);
            if (detallePresupuesto == null)
            {
                return NotFound();
            }

            return View(detallePresupuesto);
        }

        // GET: DetallePresupuestos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DetallePresupuestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPresupuesto,IdRegistroLibros,IdTransaccion,FechaIngreso,Observaciones")] DetallePresupuesto detallePresupuesto)
        {
            if (ModelState.IsValid)
            {
                await _detallesPresupuestoRep.CrearDetallesPresupuesto(detallePresupuesto.IdPresupuesto, detallePresupuesto.IdRegistroLibros, detallePresupuesto.IdTransaccion, detallePresupuesto.FechaIngreso, detallePresupuesto.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(detallePresupuesto);
        }

        // GET: DetallePresupuestos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePresupuesto = await _detallesPresupuestoRep.ConsultarDetallesPresupuesto(id.Value);
            if (detallePresupuesto == null)
            {
                return NotFound();
            }
            return View(detallePresupuesto);
        }

        // POST: DetallePresupuestos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPresupuesto,IdRegistroLibros,IdTransaccion,FechaIngreso,Observaciones")] DetallePresupuesto detallePresupuesto)
        {
            if (id != detallePresupuesto.IdTransaccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _detallesPresupuestoRep.ActualizarDetallesPresupuesto(detallePresupuesto.IdPresupuesto, detallePresupuesto.IdTransaccion, detallePresupuesto.FechaIngreso, detallePresupuesto.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(detallePresupuesto);
        }

        // GET: DetallePresupuestos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePresupuesto = await _detallesPresupuestoRep.ConsultarDetallesPresupuesto(id.Value);
            if (detallePresupuesto == null)
            {
                return NotFound();
            }

            return View(detallePresupuesto);
        }

        // POST: DetallePresupuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            await _detallesPresupuestoRep.EliminarDetallesPresupuesto(id.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
