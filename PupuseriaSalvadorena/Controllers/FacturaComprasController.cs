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
        private readonly IRegistroLibrosRep _registroLibrosRep;

        public FacturaComprasController(IFacturaCompraRep context, IMateriaPrimaRep materiaPrimaRep, ITipoPagoRep tipoPagoRep, 
                                        ITipoFacturaRep tipoFacturaRep, IProveedorRep proveedorRep, ICuentaPagarRep cuentaPagarRep, 
                                        IDetallesTransacRep detallesTransacRep, IHistorialCompraRep historialCompraRep,
                                        IRegistroLibrosRep registroLibrosRep)
        {
            _facturaCompraRep = context;
            _materiaPrimaRep = materiaPrimaRep;
            _tipoPagoRep = tipoPagoRep;
            _tipoFacturaRep = tipoFacturaRep;
            _proveedorRep = proveedorRep;
            _cuentaPagarRep = cuentaPagarRep;
            _detallesTransacRep = detallesTransacRep;
            _historialCompraRep = historialCompraRep;
            _registroLibrosRep = registroLibrosRep;
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
            var tipoPagos = await _tipoPagoRep.MostrarTipoPagos();
            var tipoFacturas = await _tipoFacturaRep.MostrarTipoFacturas();
            var proveedores = await _proveedorRep.MostrarProveedores();

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
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);
                var IdFactura = await _facturaCompraRep.CrearFacturaId(facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago, facturaCompra.IdTipoFactura, facturaCompra.IdMateriaPrima);

                decimal montoTotal = Libro.MontoTotal - facturaCompra.TotalCompra;

                await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, montoTotal, Libro.Descripcion, Libro.Conciliado);
                await _detallesTransacRep.CrearDetalleTransaccion(IdLibro, facturaCompra.DetallesCompra, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.FechaFactura, 1, "TAX001", false, facturaCompra.FechaFactura, "No Recurrente", false);
                await _historialCompraRep.CrearHistorialCompra(facturaCompra.IdMateriaPrima, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.Peso, facturaCompra.FechaFactura, IdFactura);


                if (facturaCompra.Activo)
                {
                    await _cuentaPagarRep.CrearCuentaPagar(facturaCompra.FechaFactura, facturaCompra.FechaVencimiento, facturaCompra.TotalCompra, IdFactura, facturaCompra.IdProveedor, facturaCompra.Activo);
                }

                return Json(new { success = true, message = "Factura de Compra agregada correctamente." });
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

            var materiasPrimas = await _materiaPrimaRep.MostrarMateriaPrima();
            var tipoPagos = await _tipoPagoRep.MostrarTipoPagos();
            var tipoFacturas = await _tipoFacturaRep.MostrarTipoFacturas();

            ViewBag.MateriasPrimas = new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima");
            ViewBag.TipoPagos = new SelectList(tipoPagos, "IdTipoPago", "NombrePago");
            ViewBag.TipoFacturas = new SelectList(tipoFacturas, "IdTipoFactura", "NombreFactura");

            if (facturaCompra == null)
            {
                return NotFound();
            }
            return PartialView("_editFacturaCPartial", facturaCompra);
        }

        // POST: FacturaCompras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdFacturaCompra,FacturaCom,FechaFactura,TotalCompra,DetallesCompra,IdTipoPago,IdTipoFactura,IdMateriaPrima,FacturaDoc")] FacturaCompra facturaCompra, IFormFile? facturaDoc)
        {
            if (id != facturaCompra.IdFacturaCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var factura = await _facturaCompraRep.ConsultarFacturasCompras(id);
                var transaccion = await _detallesTransacRep.ConsultarTransaccionesDetalles(factura.DetallesCompra);
                var historialCompra = await _historialCompraRep.ConsultarHistorialComprasporFactura(id);
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(transaccion.IdRegistroLibros);
                decimal montoTotal = (Libro.MontoTotal + factura.TotalCompra) - facturaCompra.TotalCompra;

                if (facturaDoc != null && facturaDoc.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await facturaDoc.CopyToAsync(memoryStream);
                        facturaCompra.FacturaCom = memoryStream.ToArray();
                    }
                }
                else
                {
                    facturaCompra.FacturaCom = factura.FacturaCom;
                }

                await _registroLibrosRep.ActualizarRegistroLibros(transaccion.IdRegistroLibros, montoTotal, Libro.Descripcion, Libro.Conciliado);
                await _detallesTransacRep.ActualizarDetalleTransaccion(transaccion.IdTransaccion, facturaCompra.DetallesCompra, facturaCompra.Cantidad, facturaCompra.TotalCompra, 1, "TAX001", transaccion.Conciliado);
                await _historialCompraRep.ActualizarHistorialCompra(historialCompra.IdCompra, facturaCompra.IdMateriaPrima, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.Peso, facturaCompra.FechaFactura, facturaCompra.IdFacturaCompra);
                await _facturaCompraRep.ActualizarFacturaCompra(facturaCompra.IdFacturaCompra, facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago, facturaCompra.IdTipoFactura, facturaCompra.IdMateriaPrima);
                return Json(new { success = true, message = "Factura de compra actualizada correctamente." });
            }
            return Json(new { success = false, message = "Error al actualizar la factura de compra." });
        }

        // GET: FacturaCompras/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(id);
            return Json(new { exists = facturaCompra != null });
        }

        // POST: FacturaCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);
                var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(id);

                decimal montoTotal = Libro.MontoTotal + facturaCompra.TotalCompra;

                await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, montoTotal, Libro.Descripcion, Libro.Conciliado);
                await _facturaCompraRep.EliminarFacturaCompra(id);
                return Json(new { success = true, message = "Factura de compra eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Para continuar debes de eliminar la cuenta por pagar asociada." });
            }
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

        [HttpGet]
        public async Task<JsonResult> GetMateriasPrimas(string IdProveedor)
        {
            var materiasPrimas = await _materiaPrimaRep.ConsultarMateriasPrimasProveedor(IdProveedor);
            return Json(new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima"));
        }
    }
}
