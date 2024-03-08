using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class CuentaPagarsController : Controller
    {
        private readonly ICuentaPagarRep _cuentaPagarRep;
        private readonly IProveedorRep _proveedorRep;
        private readonly IFacturaCompraRep _facturaCompraRep;
        private readonly IDetallesCuentaRep _detallesCuentaRep;

        public CuentaPagarsController(ICuentaPagarRep context, IProveedorRep proveedorRep, IFacturaCompraRep facturaCompraRep, IDetallesCuentaRep detallesCuentaRep)
        {
            _cuentaPagarRep = context;
            _proveedorRep = proveedorRep;
            _facturaCompraRep = facturaCompraRep;
            _detallesCuentaRep = detallesCuentaRep;
        }

        // GET: CuentaPagars
        public async Task<IActionResult> Index()
        {
            var cuentaPagars = await _cuentaPagarRep.MostrarCuentasPagar();
            ViewBag.cuentasPagadas = cuentaPagars.Count(c => c.Estado == false);
            ViewBag.cuentasPorPagar = cuentaPagars.Count(c => c.Estado == true);

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

            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "ProveedorCompleto");
            ViewBag.FacturaCompras = new SelectList(facturaCompras, "IdFacturaCompra", "IdFacturaCompra");
            return PartialView("_newCuentaPagarPartial", new CuentaPagar());
        }

        // POST: CuentaPagars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCuentaPagar,FechaCreacion,FechaVencimiento,TotalPagado,IdFacturaCompra,IdProveedor,Estado")] CuentaPagar cuentaPagar)
        {
            if (ModelState.IsValid)
            {
                await _cuentaPagarRep.CrearCuentaPagar(cuentaPagar.FechaCreacion, cuentaPagar.FechaVencimiento, cuentaPagar.TotalPagado, cuentaPagar.IdFacturaCompra, cuentaPagar.IdProveedor, cuentaPagar.Estado);
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
            var proveedores = await _proveedorRep.MostrarProveedores();
            var facturaCompras = await _facturaCompraRep.MostrarFacturasCompras();

            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "ProveedorCompleto");
            ViewBag.FacturaCompras = new SelectList(facturaCompras, "IdFacturaCompra", "IdFacturaCompra");

            if (cuentaPagar == null)
            {
                return NotFound();
            }
            return PartialView("_editCuentaPagarPartial", cuentaPagar);
        }

        // POST: CuentaPagars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdCuentaPagar,FechaCreacion,FechaVencimiento,TotalPagado,IdFacturaCompra,IdProveedor,Estado")] CuentaPagar cuentaPagar)
        {
            if (id != cuentaPagar.IdCuentaPagar)
            {
                return Json(new { success = false, message = "Cuenta por pagar no encontrada." });
            }

            if (ModelState.IsValid)
            {
                await _cuentaPagarRep.ActualizarCuentaPagar(cuentaPagar.IdCuentaPagar, cuentaPagar.FechaCreacion, cuentaPagar.FechaVencimiento, cuentaPagar.TotalPagado, cuentaPagar.IdFacturaCompra, cuentaPagar.IdProveedor, cuentaPagar.Estado);
                return Json(new { success = true, message = "Cuenta por Pagar actualizada correctamente." });
            }
            return Json(new { success = false, message = "Datos inválidos." });
        }

        // GET: CuentaPagars/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var cuentaPagar = await _cuentaPagarRep.ConsultarCuentasPagar(id);
            return Json(new { exists = cuentaPagar != null });
        }

        // POST: CuentaPagars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var cuenta = await _cuentaPagarRep.ConsultarCuentasPagar(id);
                var detalleCuenta = await _detallesCuentaRep.ConsultarCuentaDetalles(cuenta.IdCuentaPagar);

                decimal Pagado = detalleCuenta.Sum(x => x.Pago);
                decimal PorPagar = cuenta.TotalPagado - Pagado;

                if (cuenta.TotalPagado == PorPagar || PorPagar == 0)
                {
                    await _cuentaPagarRep.EliminarCuentaPagar(id);
                    return Json(new { success = true, message = "Cuenta por pagar eliminada correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "Ya se han realizados pagos sobre la cuenta para proceder con la eliminacion se debe pagar la cuenta en su totalidad." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar la cuenta por pagar." });
            }
        }
    }
}
