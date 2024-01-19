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
    public class FacturaVentasController : Controller
    {
        private readonly IFacturaVentaRep _facturaVentaRep;

        public FacturaVentasController(IFacturaVentaRep context)
        {
            _facturaVentaRep = context;
        }

        // GET: FacturaVentas
        public async Task<IActionResult> Index()
        {
            var facturaVentas = await _facturaVentaRep.MostrarFacturasVentas();
            return View(facturaVentas); 
        }

        // GET: FacturaVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaVenta = await _facturaVentaRep.ConsultarFacturasVentas(id.Value);
            if (facturaVenta == null)
            {
                return NotFound();
            }

            return View(facturaVenta);
        }

        // GET: FacturaVentas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FacturaVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFacturaVenta,CedulaJuridica,Consecutivo,Clave,FechaFactura,SubTotal,TotalVenta,IdTipoPago,IdTipoFactura")] FacturaVenta facturaVenta)
        {
            if (ModelState.IsValid)
            {
                await _facturaVentaRep.CrearFacturaVenta(facturaVenta.CedulaJuridica, facturaVenta.Consecutivo, facturaVenta.Clave, facturaVenta.FechaFactura, facturaVenta.SubTotal, facturaVenta.TotalVenta, facturaVenta.IdTipoPago, facturaVenta.IdTipoFactura);
                return RedirectToAction(nameof(Index));
            }
            return View(facturaVenta);
        }

        // GET: FacturaVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaVenta = await _facturaVentaRep.ConsultarFacturasVentas(id.Value);
            if (facturaVenta == null)
            {
                return NotFound();
            }
            return View(facturaVenta);
        }

        // POST: FacturaVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFacturaVenta,CedulaJuridica,Consecutivo,Clave,FechaFactura,SubTotal,TotalVenta,IdTipoPago,IdTipoFactura")] FacturaVenta facturaVenta)
        {
            if (id != facturaVenta.IdFacturaVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _facturaVentaRep.ActualizarFacturaVenta(facturaVenta.IdFacturaVenta, facturaVenta.IdTipoPago, facturaVenta.IdTipoFactura);
                return RedirectToAction(nameof(Index));
            }
            return View(facturaVenta);
        }

        // GET: FacturaVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaVenta = await _facturaVentaRep.ConsultarFacturasVentas(id.Value);
            if (facturaVenta == null)
            {
                return NotFound();
            }

            return View(facturaVenta);
        }

        // POST: FacturaVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _facturaVentaRep.EliminarFacturaVenta(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
