using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using PupuseriaSalvadorena.ViewModels;
using Rotativa.AspNetCore;
using PupuseriaSalvadorena.Filtros;
using System.Text.RegularExpressions;

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
        private readonly INegociosRep _negociosRep;
        private readonly IDetallesCuentaRep _detallesCuentaRep;

        public FacturaComprasController(IFacturaCompraRep context, IMateriaPrimaRep materiaPrimaRep, ITipoPagoRep tipoPagoRep, ITipoFacturaRep tipoFacturaRep, IProveedorRep proveedorRep, ICuentaPagarRep cuentaPagarRep, IDetallesTransacRep detallesTransacRep, IHistorialCompraRep historialCompraRep, IRegistroLibrosRep registroLibrosRep, INegociosRep negociosRep, IDetallesCuentaRep detallesCuentaRep)
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
            _negociosRep = negociosRep;
            _detallesCuentaRep = detallesCuentaRep;
        }

        // GET: FacturaCompras
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var fechaActual = DateTime.Today;
            var añoActual = fechaActual.Year;
            var mesActual = fechaActual.Month;

            var facturaCompras = await _facturaCompraRep.MostrarFacturasCompras();
            var proveedores = await _proveedorRep.MostrarProveedores();

            var facturasActuales = facturaCompras.Where(f => f.FechaFactura.Year == añoActual && f.FechaFactura.Month == mesActual).ToList();

            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "ProveedorCompleto");

            return View(facturasActuales);
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
        public async Task<IActionResult> Create([Bind("IdFacturaCompra,FacturaCom,FechaFactura,TotalCompra,DetallesCompra,IdTipoPago,IdTipoFactura,IdMateriaPrima,FacturaDoc,Activo,IdProveedor,FechaVencimiento,Peso,Cantidad,TipoReporte,FechaReporte")] FacturaCompra facturaCompra)
        {
            if (ModelState.IsValid)
            {
                if (facturaCompra.FacturaDoc != null && facturaCompra.FacturaDoc.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await facturaCompra.FacturaDoc.CopyToAsync(memoryStream);
                        facturaCompra.FacturaCom = memoryStream.ToArray();
                    }
                }
                else 
                {                     
                    facturaCompra.FacturaCom = new byte[0];
                }

                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);

                if (Libro.Conciliado)
                {
                    return Json(new { success = false, message = "No se puede registrar la factura de compra, el libro actual esta conciliado." });
                }

                var IdFactura = await _facturaCompraRep.CrearFacturaId(facturaCompra.FacturaCom, facturaCompra.FechaFactura, facturaCompra.TotalCompra, facturaCompra.DetallesCompra, facturaCompra.IdTipoPago.Value, facturaCompra.IdTipoFactura, facturaCompra.IdMateriaPrima.Value);

                decimal montoTotal = Libro.MontoTotal - facturaCompra.TotalCompra;

                await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, montoTotal, Libro.Descripcion, Libro.Conciliado);
                await _detallesTransacRep.CrearDetalleTransaccion(IdLibro, $"Factura de Compra: {IdFactura}", facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.FechaFactura, 1, "TAX001", false, facturaCompra.FechaFactura, "No Recurrente", false);
                await _historialCompraRep.CrearHistorialCompra(facturaCompra.IdMateriaPrima.Value, facturaCompra.Cantidad, facturaCompra.TotalCompra, facturaCompra.Peso, facturaCompra.FechaFactura, IdFactura);


                if (facturaCompra.Activo)
                {
                    await _cuentaPagarRep.CrearCuentaPagar(facturaCompra.FechaFactura, facturaCompra.FechaVencimiento, facturaCompra.TotalCompra, IdFactura, facturaCompra.IdProveedor, facturaCompra.Activo);
                }

                return Json(new { success = true, message = "Factura de Compra agregada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: FacturaCompras/Delete/5
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            var cuentapagar = await _cuentaPagarRep.ConsultarCuentasPagarporFactura(id);

            if(cuentapagar != null && cuentapagar.TotalPagado != 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
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

                if (Libro.Conciliado)
                {
                    return Json(new { success = false, message = "No se puede eliminar la factura de compra, el libro actual esta conciliado." });
                }

                var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(id);
                var cuentapagar = await _cuentaPagarRep.ConsultarCuentasPagarporFactura(id);
                var transaccion = await _detallesTransacRep.ConsultarTransaccionesDetalles($"Factura de Compra: {facturaCompra.IdFacturaCompra}");

                if (cuentapagar != null)
                {
                    var detalleCuenta = await _detallesCuentaRep.ConsultarCuentaDetalles(cuentapagar.IdCuentaPagar);
                    decimal Pagado = detalleCuenta.Sum(x => x.Pago);
                    decimal PorPagar = cuentapagar.TotalPagado - Pagado;

                    if (cuentapagar.TotalPagado == facturaCompra.TotalCompra || PorPagar == 0)
                    {
                        await _cuentaPagarRep.EliminarCuentaPagar(cuentapagar.IdCuentaPagar);
                    }
                    else
                    {
                       return Json(new { success = false, message = "La cuenta por pagar asociada debe pagarse en su totalidad para proceder con la eliminacion." });
                    }
                }

                decimal montoTotal = Libro.MontoTotal + facturaCompra.TotalCompra;

                await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, montoTotal, Libro.Descripcion, Libro.Conciliado);
                await _detallesTransacRep.EliminarDetallesTransaccion(transaccion.IdTransaccion);
                await _historialCompraRep.EliminarHistorialCompraFactura(id);
                await _facturaCompraRep.EliminarFacturaCompra(id);
                return Json(new { success = true, message = "Factura de compra eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Para continuar debes de eliminar la cuenta por pagar asociada." });
            }
        }

        // Descargar Factura
        [HttpGet("FacturaCompras/DescargarFactura/{IdFactura}")]
        public async Task<IActionResult> DescargarFactura(string IdFactura)
        {
            var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(IdFactura);

            if (facturaCompra != null && facturaCompra.FacturaCom != null && facturaCompra.FacturaCom.Length > 0)
            {
                string fechaFormato = facturaCompra.FechaFactura.ToString("yyyyMMdd");
                string nombreArchivo = $"Factura_{fechaFormato}.pdf";

                return File(facturaCompra.FacturaCom, "application/pdf", nombreArchivo);
            }
            else if (facturaCompra != null && (facturaCompra.FacturaCom == null || facturaCompra.FacturaCom.Length == 0))
            {
                return Json(new { success = false, message = "Esta factura no tiene una documento adjunto." });
            }
            else
            {
                return Json(new { success = false, message = "No se encontro la factura de compra." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMateriasPrimas(string IdProveedor)
        {
            var materiasPrimas = await _materiaPrimaRep.ConsultarMateriasPrimasProveedor(IdProveedor);
            return Json(new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima"));
        }

        // Reporte de Facturas Excel o Pdf
        public async Task<IActionResult> ReporteFacturas(FacturaCompra facturaCompra)
        {
            var facturas = await _facturaCompraRep.MostrarFacturasCompras();
            var negocio = await _negociosRep.MostrarNegocio();

            string mes = facturaCompra.FechaReporte.ToString("MMMM", new CultureInfo("es-ES"));
            string year = facturaCompra.FechaReporte.Year.ToString();

            var facturasMes = facturas.Where(f => f.FechaFactura.Year == facturaCompra.FechaReporte.Year && f.FechaFactura.Month == facturaCompra.FechaReporte.Month).ToList();
            decimal totalVentas = facturasMes.Sum(f => f.TotalCompra);
            int totalFacturas = facturasMes.Count();

            var viewModel = new FacturaComprasPDF
            {
                Mes = mes,
                Year = year,
                Negocio = negocio,
                Facturas = facturasMes,
                TotalVentas = totalVentas,
                TotalFacturas = totalFacturas
            };

            if (facturaCompra.TipoReporte == "Pdf")
            {
                return new ViewAsPdf("DescargarFacturaCompras", viewModel)
                {
                    FileName = $"Reporte Compras {mes}.pdf",
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 12, 12, 12)
                };
            }
            else
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Compras");
                        var currentRow = 1;

                        CultureInfo ci = new CultureInfo("es-CR");

                        worksheet.Cell(currentRow, 1).Value = "ID";
                        worksheet.Cell(currentRow, 2).Value = "Producto";
                        worksheet.Cell(currentRow, 3).Value = "Fecha";
                        worksheet.Cell(currentRow, 4).Value = "Pago";
                        worksheet.Cell(currentRow, 5).Value = "Monto Compra";
                        worksheet.Cell(currentRow, 6).Value = "Detalles";

                        worksheet.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:F1").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A1:F1").Style.Font.Bold = true;

                        foreach (var factura in viewModel.Facturas)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = factura.IdFacturaCompra;
                            worksheet.Cell(currentRow, 2).Value = factura.NombreMateriaPrima;
                            worksheet.Cell(currentRow, 3).Value = factura.FechaFactura.ToString("dd/MM/yyyy");
                            worksheet.Cell(currentRow, 4).Value = factura.NombrePago;
                            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = ci.NumberFormat.CurrencySymbol + " #,##0.00";
                            worksheet.Cell(currentRow, 5).Value = factura.TotalCompra;
                            worksheet.Cell(currentRow, 6).Value = factura.DetallesCompra;
                        }

                        worksheet.Range("A2:F" + currentRow).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A2:F" + currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:F" + currentRow).SetAutoFilter();

                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = "Total Compras";
                        worksheet.Cell(currentRow, 4).Value = viewModel.TotalVentas;
                        worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = ci.NumberFormat.CurrencySymbol + " #,##0.00";
                        worksheet.Range("A" + currentRow + ":" + "C" + currentRow).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("D" + currentRow + ":" + "F" + currentRow).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A" + currentRow + ":" + "F" + currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A" + currentRow + ":" + "F" + currentRow).Style.Font.Bold = true;
                        worksheet.Range("A" + currentRow + ":" + "C" + currentRow).Merge();
                        worksheet.Range("D" + currentRow + ":" + "F" + currentRow).Merge();

                        worksheet.Columns().AdjustToContents();

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reporte Compras {mes}.xlsx");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al generar el reporte. Detalle: " + ex.Message });
                }
            }
        }
    }
}
