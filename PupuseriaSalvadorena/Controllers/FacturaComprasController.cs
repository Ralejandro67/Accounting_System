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
    public class FacturaComprasController : Controller
    {
        private readonly IFacturaCompraRep _facturaCompraRep;   

        public FacturaComprasController(IFacturaCompraRep context)
        {
            _facturaCompraRep = context;
        }

        // GET: FacturaCompras
        public async Task<IActionResult> Index()
        {
            var facturaCompras = await _facturaCompraRep.MostrarFacturasCompras();
            return View(facturaCompras);
        }

        // GET: FacturaCompras/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(id);
            if (facturaCompra == null)
            {
                return NotFound();
            }

            return View(facturaCompra);
        }

        // GET: FacturaCompras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FacturaCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFacturaCompra,FacturaCom,FechaFactura,TotalCompra,DetallesCompra,IdTipoPago,IdTipoFactura")] FacturaCompra facturaCompra)
        {
            if (ModelState.IsValid)
            {
                await _facturaCompraRep.CrearFacturaCompra(facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago, facturaCompra.IdTipoFactura);
                return RedirectToAction(nameof(Index));
            }
            return View(facturaCompra);
        }

        // GET: FacturaCompras/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(id);
            if (facturaCompra == null)
            {
                return NotFound();
            }
            return View(facturaCompra);
        }

        // POST: FacturaCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdFacturaCompra,FacturaCom,FechaFactura,TotalCompra,DetallesCompra,IdTipoPago,IdTipoFactura")] FacturaCompra facturaCompra)
        {
            if (id != facturaCompra.IdFacturaCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _facturaCompraRep.ActualizarFacturaCompra(facturaCompra.IdFacturaCompra, facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago, facturaCompra.IdTipoFactura);
                return RedirectToAction(nameof(Index));
            }
            return View(facturaCompra);
        }

        // GET: FacturaCompras/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(id);
            if (facturaCompra == null)
            {
                return NotFound();
            }

            return View(facturaCompra);
        }

        // POST: FacturaCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _facturaCompraRep.EliminarFacturaCompra(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
