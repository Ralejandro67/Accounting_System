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
    public class CuentaPagarsController : Controller
    {
        private readonly ICuentaPagarRep _cuentaPagarRep;
        private readonly IProveedorRep _proveedorRep;
        private readonly IFacturaCompraRep _facturaCompraRep;

        public CuentaPagarsController(ICuentaPagarRep context, IProveedorRep proveedorRep, IFacturaCompraRep facturaCompraRep)
        {
            _cuentaPagarRep = context;
            _proveedorRep = proveedorRep;
            _facturaCompraRep = facturaCompraRep;
        }

        // GET: CuentaPagars
        public async Task<IActionResult> Index()
        {
            var cuentaPagars = await _cuentaPagarRep.MostrarCuentasPagar();
            return View(cuentaPagars);  
        }

        // GET: CuentaPagars/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentaPagar = await _cuentaPagarRep.ConsultarCuentasPagar(id);
            if (cuentaPagar == null)
            {
                return NotFound();
            }

            return View(cuentaPagar);
        }

        // GET: CuentaPagars/Create
        public async Task<IActionResult> Create()
        {
            var proveedores = await _proveedorRep.MostrarProveedores();
            var facturaCompras = await _facturaCompraRep.MostrarFacturasCompras();

            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "NombreProveedor");
            ViewBag.FacturaCompras = new SelectList(facturaCompras, "IdFacturaCompra", "IdFacturaCompra");
            return PartialView("_newCuentaPagarPartial", new CuentaPagar());
        }

        // POST: CuentaPagars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCuentaPagar,FechaCreacion,FechaVencimiento,TotalPagado,IdFacturaCompra,IdProveedor")] CuentaPagar cuentaPagar)
        {
            if (ModelState.IsValid)
            {
                await _cuentaPagarRep.CrearCuentaPagar(cuentaPagar.FechaCreacion, cuentaPagar.FechaVencimiento, cuentaPagar.TotalPagado, cuentaPagar.IdFacturaCompra, cuentaPagar.IdProveedor);
                return Json(new { success = true, message = "Cuenta por pagar agregada correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la cuenta por pagar." });
        }

        // GET: CuentaPagars/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentaPagar = await _cuentaPagarRep.ConsultarCuentasPagar(id);
            if (cuentaPagar == null)
            {
                return NotFound();
            }
            return View(cuentaPagar);
        }

        // POST: CuentaPagars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdCuentaPagar,FechaCreacion,FechaVencimiento,TotalPagado,IdFacturaCompra,IdProveedor")] CuentaPagar cuentaPagar)
        {
            if (id != cuentaPagar.IdCuentaPagar)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _cuentaPagarRep.ActualizarCuentaPagar(cuentaPagar.IdCuentaPagar, cuentaPagar.TotalPagado, cuentaPagar.IdFacturaCompra, cuentaPagar.IdProveedor);
                return RedirectToAction(nameof(Index));
            }
            return View(cuentaPagar);
        }

        // GET: CuentaPagars/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentaPagar = await _cuentaPagarRep.ConsultarCuentasPagar(id);
            if (cuentaPagar == null)
            {
                return NotFound();
            }

            return View(cuentaPagar);
        }

        // POST: CuentaPagars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _cuentaPagarRep.EliminarCuentaPagar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
