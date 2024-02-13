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
    public class HistorialVentasController : Controller
    {
        private readonly IHistorialVentaRep _historialVentaRep;

        public HistorialVentasController(IHistorialVentaRep context)
        {
            _historialVentaRep = context;
        }

        // GET: HistorialVentas
        public async Task<IActionResult> Index()
        {
            var historialVentas = await _historialVentaRep.MostrarHistorialVenta();
            return View(historialVentas);   
        }

        // GET: HistorialVentas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialVenta = await _historialVentaRep.ConsultarHistorialVentas(id);
            if (historialVenta == null)
            {
                return NotFound();
            }

            return View(historialVenta);
        }

        // GET: HistorialVentas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistorialVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,CantVenta,FechaVenta,IdPlatillo,IdFacturaVenta,IdTipoVenta")] HistorialVenta historialVenta)
        {
            if (ModelState.IsValid)
            {
                await _historialVentaRep.CrearHistorialVenta(historialVenta.IdVenta, historialVenta.CantVenta, historialVenta.FechaVenta, historialVenta.IdPlatillo, historialVenta.IdFacturaVenta, historialVenta.IdTipoVenta);
                return RedirectToAction(nameof(Index));
            }
            return View(historialVenta);
        }

        // GET: HistorialVentas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialVenta = await _historialVentaRep.ConsultarHistorialVentas(id);
            if (historialVenta == null)
            {
                return NotFound();
            }
            return View(historialVenta);
        }

        // POST: HistorialVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,CantVenta,FechaVenta,IdPlatillo,IdFacturaVenta,IdTipoVenta")] HistorialVenta historialVenta)
        {
            if (id != historialVenta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _historialVentaRep.ActualizarHistorialVenta(historialVenta.IdVenta, historialVenta.CantVenta, historialVenta.IdPlatillo, historialVenta.IdFacturaVenta, historialVenta.IdTipoVenta);
                return RedirectToAction(nameof(Index));
            }
            return View(historialVenta);
        }

        // GET: HistorialVentas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialVenta = await _historialVentaRep.ConsultarHistorialVentas(id); 
            if (historialVenta == null)
            {
                return NotFound();
            }

            return View(historialVenta);
        }

        // POST: HistorialVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _historialVentaRep.EliminarHistorialVenta(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
