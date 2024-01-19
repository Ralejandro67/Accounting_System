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
    public class HistorialComprasController : Controller
    {
        private readonly IHistorialCompraRep _historialCompraRep;

        public HistorialComprasController(IHistorialCompraRep context)
        {
            _historialCompraRep = context;
        }

        // GET: HistorialCompras
        public async Task<IActionResult> Index()
        {
            var historialCompras = await _historialCompraRep.MostrarHistorialCompras();
            return View(historialCompras);
        }

        // GET: HistorialCompras/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
            if (historialCompra == null)
            {
                return NotFound();
            }

            return View(historialCompra);
        }

        // GET: HistorialCompras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistorialCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdMateriaPrima,Cantidad,Precio,Peso,FechaCompra,IdFacturaCompra")] HistorialCompra historialCompra)
        {
            if (ModelState.IsValid)
            {
                await _historialCompraRep.CrearHistorialCompra(historialCompra.IdMateriaPrima, historialCompra.Cantidad, historialCompra.Precio, historialCompra.Peso, historialCompra.FechaCompra, historialCompra.IdFacturaCompra);
                return RedirectToAction(nameof(Index));
            }
            return View(historialCompra);
        }

        // GET: HistorialCompras/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
            if (historialCompra == null)
            {
                return NotFound();
            }
            return View(historialCompra);
        }

        // POST: HistorialCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdCompra,IdMateriaPrima,Cantidad,Precio,Peso,FechaCompra,IdFacturaCompra")] HistorialCompra historialCompra)
        {
            if (id != historialCompra.IdFacturaCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _historialCompraRep.ActualizarHistorialCompra(historialCompra.IdCompra, historialCompra.IdMateriaPrima, historialCompra.Cantidad, historialCompra.Precio, historialCompra.Peso, historialCompra.FechaCompra, historialCompra.IdFacturaCompra);
                return RedirectToAction(nameof(Index));
            }
            return View(historialCompra);
        }

        // GET: HistorialCompras/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
            if (historialCompra == null)
            {
                return NotFound();
            }

            return View(historialCompra);
        }

        // POST: HistorialCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _historialCompraRep.EliminarHistorialCompra(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
