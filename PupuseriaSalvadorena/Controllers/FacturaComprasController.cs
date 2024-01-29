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
    public class FacturaComprasController : Controller
    {
        private readonly IFacturaCompraRep _facturaCompraRep;
        private readonly IMateriaPrimaRep _materiaPrimaRep;
        private readonly ITipoPagoRep _tipoPagoRep;
        private readonly ITipoFacturaRep _tipoFacturaRep;
        private readonly IProveedorRep _proveedorRep;
        private readonly ICuentaPagarRep _cuentaPagarRep;
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly IHistorialCompraRep _historialCompraRep;

        public FacturaComprasController(IFacturaCompraRep context, IMateriaPrimaRep materiaPrimaRep, ITipoPagoRep tipoPagoRep, ITipoFacturaRep tipoFacturaRep, IProveedorRep proveedorRep, ICuentaPagarRep cuentaPagarRep, IDetallesTransacRep detallesTransacRep, IHistorialCompraRep historialCompraRep)
        {
            _facturaCompraRep = context;
            _materiaPrimaRep = materiaPrimaRep;
            _tipoPagoRep = tipoPagoRep;
            _tipoFacturaRep = tipoFacturaRep;
            _proveedorRep = proveedorRep;
            _cuentaPagarRep = cuentaPagarRep;
            _detallesTransacRep = detallesTransacRep;
            _historialCompraRep = historialCompraRep;
        }

        // GET: FacturaCompras
        public async Task<IActionResult> Index()
        {
            var facturaCompras = await _facturaCompraRep.MostrarFacturasCompras();
            var proveedores = await _proveedorRep.MostrarProveedores();
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "ProveedorCompleto");

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
        public async Task<IActionResult> Create()
        {
            var materiasPrimas = await _materiaPrimaRep.MostrarMateriaPrima();
            var tipoPagos = await _tipoPagoRep.MostrarTipoPagos();
            var tipoFacturas = await _tipoFacturaRep.MostrarTipoFacturas();
            var proveedores = await _proveedorRep.MostrarProveedores();

            ViewBag.MateriasPrimas = new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima");
            ViewBag.TipoPagos = new SelectList(tipoPagos, "IdTipoPago", "NombrePago");
            ViewBag.TipoFacturas = new SelectList(tipoFacturas, "IdTipoFactura", "NombreFactura");
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "ProveedorCompleto");
            return PartialView("_newFacturaCPartial", new FacturaCompra());
        }

        // POST: FacturaCompras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFacturaCompra,FacturaCom,FechaFactura,TotalCompra,DetallesCompra,IdTipoPago,IdTipoFactura,IdMateriaPrima,FacturaDoc,Activo,IdProveedor,FechaVencimiento,Peso,Cantidad")] FacturaCompra facturaCompra, IFormFile facturaDoc)
        {
            if (ModelState.IsValid)
            {
                if (facturaDoc != null && facturaDoc.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await facturaDoc.CopyToAsync(memoryStream);
                        facturaCompra.FacturaCom = memoryStream.ToArray();
                    }
                }

                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();

                if (facturaCompra.Activo)
                {
                    var IdFactura = await _facturaCompraRep.CrearFacturaId(facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago, facturaCompra.IdTipoFactura, facturaCompra.IdMateriaPrima);
                    
                    await _cuentaPagarRep.CrearCuentaPagar(facturaCompra.FechaFactura, facturaCompra.FechaVencimiento, facturaCompra.TotalCompra, IdFactura, facturaCompra.IdProveedor, facturaCompra.Activo);
                    await _historialCompraRep.CrearHistorialCompra(facturaCompra.IdMateriaPrima, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.Peso, facturaCompra.FechaFactura, IdFactura);
                    await _detallesTransacRep.CrearDetalleTransaccion(IdLibro, facturaCompra.DetallesCompra, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.FechaFactura, 2, "TAX002", false, facturaCompra.FechaFactura,"No Recurrente", false);

                    return Json(new { success = true, message = "Factura de Compra agregada correctamente." });
                }
                else
                {
                    var IdFactura = await _facturaCompraRep.CrearFacturaId(facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago, facturaCompra.IdTipoFactura, facturaCompra.IdMateriaPrima);

                    await _historialCompraRep.CrearHistorialCompra(facturaCompra.IdMateriaPrima, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.Peso, facturaCompra.FechaFactura, IdFactura);
                    await _detallesTransacRep.CrearDetalleTransaccion(IdLibro, facturaCompra.DetallesCompra, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.FechaFactura, 2, "TAX002", false, facturaCompra.FechaFactura, "No Recurrente", false);

                    return Json(new { success = true, message = "Factura de Compra agregada correctamente." });
                }
            }
            return Json(new { success = false, message = "Error al agregar la factura de compra." });
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
                await _facturaCompraRep.ActualizarFacturaCompra(facturaCompra.IdFacturaCompra, facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago, facturaCompra.IdTipoFactura, facturaCompra.IdMateriaPrima);
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

        public async Task<IActionResult> DescargarFactura(string IdFacturaCompra)
        {
            var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(IdFacturaCompra);

            if (facturaCompra != null && facturaCompra.FacturaCom != null)
            {
                string fechaFormato = facturaCompra.FechaFactura.ToString("yyyyMMdd");
                string nombreArchivo = $"Factura_{fechaFormato}.pdf";

                return File(facturaCompra.FacturaCom, "application/pdf", nombreArchivo);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
