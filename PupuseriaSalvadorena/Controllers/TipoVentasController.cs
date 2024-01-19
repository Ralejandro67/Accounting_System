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
    public class TipoVentasController : Controller
    {
        private readonly ITipoVentaRep _tipoVentaRep;

        public TipoVentasController(ITipoVentaRep context)
        {
            _tipoVentaRep = context;
        }

        // GET: TipoVentas
        public async Task<IActionResult> Index()
        {
            var tipoVentas = await _tipoVentaRep.MostrarTipoVentas();
            return View(tipoVentas);    
        }

        // GET: TipoVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            if (tipoVenta == null)
            {
                return NotFound();
            }

            return View(tipoVenta);
        }

        // GET: TipoVentas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoVenta,NombreVenta,Estado")] TipoVenta tipoVenta)
        {
            if (ModelState.IsValid)
            {
                await _tipoVentaRep.CrearTipoVenta(tipoVenta.NombreVenta, tipoVenta.Estado);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoVenta);
        }

        // GET: TipoVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            if (tipoVenta == null)
            {
                return NotFound();
            }
            return View(tipoVenta);
        }

        // POST: TipoVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoVenta,NombreVenta,Estado")] TipoVenta tipoVenta)
        {
            if (id != tipoVenta.IdTipoVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tipoVentaRep.ActualizarTipoVentas(tipoVenta.IdTipoVenta, tipoVenta.NombreVenta, tipoVenta.Estado);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoVenta);
        }

        // GET: TipoVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            if (tipoVenta == null)
            {
                return NotFound();
            }

            return View(tipoVenta);
        }

        // POST: TipoVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoVentaRep.EliminarTipoVenta(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
