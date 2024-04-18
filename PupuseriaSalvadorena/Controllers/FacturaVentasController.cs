using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using PupuseriaSalvadorena.ViewModels;
using System.Text;
using System.Collections.Immutable;
using System.Globalization;
using Rotativa.AspNetCore;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using PupuseriaSalvadorena.Filtros;

namespace PupuseriaSalvadorena.Controllers
{
    public class FacturaVentasController : Controller
    {
        private readonly IFacturaVentaRep _facturaVentaRep;
        private readonly IPlatilloRep _platilloRep;
        private readonly ITipoFacturaRep _tipoFacturaRep;
        private readonly ITipoPagoRep _tipoPagoRep;
        private readonly ITipoVentaRep _tipoVentaRep;
        private readonly INegociosRep _negociosRep;
        private readonly IEnvioFacturaRep _envioFacturaRep;
        private readonly IHistorialVentaRep _historialVentaRep;
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly IRegistroLibrosRep _registroLibrosRep;

        public FacturaVentasController(IFacturaVentaRep context, IPlatilloRep platilloRep, ITipoFacturaRep tipoFacturaRep, 
                                       ITipoPagoRep tipoPagoRep, ITipoVentaRep tipoVentaRep, INegociosRep negociosRep, 
                                       IEnvioFacturaRep envioFacturaRep, IHistorialVentaRep historialVentaRep,
                                       IDetallesTransacRep detallesTransacRep, IRegistroLibrosRep registroLibrosRep)
        {
            _facturaVentaRep = context;
            _platilloRep = platilloRep;
            _tipoFacturaRep = tipoFacturaRep;
            _tipoPagoRep = tipoPagoRep;
            _tipoVentaRep = tipoVentaRep;
            _negociosRep = negociosRep;
            _envioFacturaRep = envioFacturaRep;
            _historialVentaRep = historialVentaRep;
            _detallesTransacRep = detallesTransacRep;
            _registroLibrosRep = registroLibrosRep;
        }

        // GET: FacturaVentas
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            CultureInfo cultura = new CultureInfo("es-ES");
            DateTime fecha = DateTime.Now;

            var facturaVentas = await _facturaVentaRep.MostrarFacturasVentas();
            var facturasMesActual = facturaVentas.Where(f => f.FechaFactura.Month == fecha.Month && f.FechaFactura.Year == fecha.Year).ToList();

            var facturasPorMes = facturaVentas.Where(f => f.Estado == true)
                                              .GroupBy(f => new { Año = f.FechaFactura.Year, Mes = f.FechaFactura.Month })
                                              .Select(group => new { Año = group.Key.Año, Mes = group.Key.Mes, Cantidad = group.Count(), TotalVentasMes = group.Sum(f => f.TotalVenta) })
                                              .OrderByDescending(x => x.Año).ThenByDescending(x => x.Mes)
                                              .ToList();

            var facturasInvertidas = facturasPorMes.AsEnumerable().Reverse().ToList();
            ViewBag.Meses = facturasInvertidas.Select(x => new DateTime(x.Año, x.Mes, 1).ToString("MMM yyyy", cultura)).ToList();
            ViewBag.ventasPorMes = facturasInvertidas.Select(x => x.TotalVentasMes).ToList();

            ViewBag.totalVentas = facturaVentas.Where(f => f.Estado == true).Sum(f => f.TotalVenta);
            ViewBag.totalVentasMes = facturasPorMes.FirstOrDefault()?.TotalVentasMes ?? 0;
            ViewBag.facturasMes = facturasPorMes.FirstOrDefault()?.Cantidad ?? 0;
            ViewBag.mesActual = cultura.DateTimeFormat.GetMonthName(fecha.Month);
            ViewBag.facturas = facturaVentas.Where(f => f.Estado == true).Count();

            return View(facturasMesActual); 
        }

        // GET: FacturaVentas/Details/5
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Details(int? id)
        {
            var negocio = await _negociosRep.MostrarNegocio();
            var detallesEnvio = await _envioFacturaRep.ConsultarEnvioFacturasPorID(id.Value);
            var facturaVenta = await _facturaVentaRep.ConsultarFacturasVentas(id.Value);
            var detallesFactura = await _historialVentaRep.ConsultarDetallesVentas(id.Value);

            ViewBag.Impuesto = facturaVenta.TotalVenta - facturaVenta.SubTotal;

            if (detallesEnvio == null)
            {
                detallesEnvio = new EnvioFactura 
                {
                    Identificacion = null,
                    NombreCliente = "Cliente al contado",
                    CorreoElectronico = " ",
                    Telefono = null
                };
            }

            if (facturaVenta == null)
            {
                return NotFound();
            }

            var detalles = new DetalleFactura
            {
                Negocio = negocio,
                FacturaVenta = facturaVenta,
                EnvioFactura = detallesEnvio,
                DetallesF = detallesFactura
            };

            return View(detalles);
        }

        // GET: FacturaVentas/Create
        public async Task<IActionResult> Create()
        {
            var tipoFactura = await _tipoFacturaRep.MostrarTipoFacturas();
            var tipoPago = await _tipoPagoRep.MostrarTipoPagos();
            var tipoVenta = await _tipoVentaRep.MostrarTipoVentas();
            var platillos = await _platilloRep.MostrarPlatillos();

            ViewBag.IdTipoFactura = new SelectList(tipoFactura, "IdTipoFactura", "NombreFactura");
            ViewBag.IdTipoPago = new SelectList(tipoPago, "IdTipoPago", "NombrePago");
            ViewBag.IdTipoVenta = new SelectList(tipoVenta, "IdTipoVenta", "NombreVenta");
            ViewBag.IdPlatillo = platillos.Select(p => new
            {
                IdPlatillo = p.IdPlatillo,
                NombrePlatillo = p.NombrePlatillo,
                PrecioVenta = p.PrecioVenta
            }).ToList();

            return PartialView("_newFacturaVentaPartial", new FacturaVenta());
        }

        // POST: FacturaVentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFacturaVenta,FechaFactura,SubTotal,TotalVenta,IdTipoPago,IdTipoFactura,Identificacion,NombreCliente,CorreoElectronico,Telefono,IdPlatillo,CantVenta,IdTipoVenta,FacturaElectronica,TipoId,TipoReporte,FechaReporte,Estado")] FacturaVenta facturaVenta)
        {
            if (ModelState.IsValid)
            {
                int[] idPlatillos = facturaVenta.IdPlatillo;
                int[] cantidades = facturaVenta.CantVenta;

                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);

                if (Libro.Conciliado)
                {
                    return Json(new { success = false, message = "No se puede realizar la venta, el libro actual ya fue conciliado" });
                }

                var empresa = await _negociosRep.ConsultarNegocio();
                var Pago = MetodosPago(facturaVenta.IdTipoPago.Value);

                int ContactoFactura = await CrearContacto(facturaVenta.TipoId, facturaVenta.Identificacion.Value, facturaVenta.NombreCliente, facturaVenta.CorreoElectronico, facturaVenta.Telefono.Value);

                dynamic[] items = await ListaPlatillos(idPlatillos, cantidades);

                var factura = new
                {
                    client = new { id = ContactoFactura },
                    paymentMethod = Pago,
                    saleCondition = "CASH",
                    status = "open",
                    items,
                    dueDate = facturaVenta.FechaFactura.ToString("yyyy-MM-dd"),
                    date = facturaVenta.FechaFactura.ToString("yyyy-MM-dd")
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==");
                    try
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(factura), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("https://api.alegra.com/api/v1/invoices", content);

                        if (response.IsSuccessStatusCode)
                        {
                            response.EnsureSuccessStatusCode();
                            var body = await response.Content.ReadAsStringAsync();
                            var jsonObject = JObject.Parse(body);

                            facturaVenta.Consecutivo = (decimal)jsonObject["numberTemplate"]["fullNumber"];
                            int idfactura = (int)jsonObject["id"];

                            var facturaId = await _facturaVentaRep.CrearFacturaVenta(empresa, facturaVenta.Consecutivo, facturaVenta.FechaFactura, facturaVenta.SubTotal, facturaVenta.TotalVenta, facturaVenta.IdTipoPago.Value, facturaVenta.IdTipoFactura.Value, facturaVenta.Estado);

                            int cantTotal = 0;
                            decimal montoTotal = Libro.MontoTotal + facturaVenta.TotalVenta;
                            await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, montoTotal, Libro.Descripcion, Libro.Conciliado);

                            for (int i = 0; i < idPlatillos.Length; i++)
                            {
                                cantTotal += cantidades[i];
                                await _historialVentaRep.CrearHistorialVenta(idfactura, cantidades[i], facturaVenta.FechaFactura, idPlatillos[i], facturaId, facturaVenta.IdTipoVenta.Value);
                            }

                            await _detallesTransacRep.CrearDetalleTransaccion(IdLibro, $"Factura de Venta: {facturaVenta.Consecutivo}", cantTotal, facturaVenta.TotalVenta, facturaVenta.FechaFactura, 2, "TAX002", false, facturaVenta.FechaFactura, "No Recurrente", false);

                            if (ContactoFactura != 2)
                            {
                                bool envio = await EnvioFactura(idfactura, facturaVenta.CorreoElectronico);

                                if (envio)
                                {
                                    await _envioFacturaRep.CrearEnvioFactura(facturaVenta.FechaFactura, facturaId, facturaVenta.Identificacion.Value, facturaVenta.NombreCliente, facturaVenta.CorreoElectronico, facturaVenta.Telefono.Value);
                                    return Json(new { success = true, message = $"Factura: {facturaVenta.Consecutivo} generada y enviada correctamente." });
                                }
                            }

                            return Json(new { success = true, message = $"Factura: {facturaVenta.Consecutivo} generada correctamente." });
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Error en la solicitud HTTP: " + errorContent);
                            return Json(new { success = false, message = "Error al crear la factura." });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al crear el contenido de la solicitud: " + ex.Message);
                        return Json(new { success = false, message = "Error al crear la factura." });
                    }
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // FacturaVentas/ImprimirFactura/5
        [HttpGet]
        public async Task<IActionResult> ImprimirFactura(int id)
        {
            try
            {
                var factura = await _historialVentaRep.ConsultarHistorialVentasFactura(id);
                var url = await FacturaImprimir(factura.IdVenta);
                return Json(new { success = true, url = url });
            }
            catch
            {
                return Json(new { success = false, message = "Error al imprimir la factura." });
            }
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);
                var detallesFactura = await _facturaVentaRep.ConsultarFacturasVentas(id);
                var factura = await _historialVentaRep.ConsultarHistorialVentasFactura(id);

                if (!detallesFactura.Estado)
                {
                    return Json(new { success = false, message = "La factura ya se encuentra anulada." });
                }

                bool anulada = await AnularFactura(factura.IdVenta);

                if (anulada)
                {
                    string transaccion = $"Factura de Venta: {detallesFactura.Consecutivo}";
                    decimal montoTotal = Libro.MontoTotal - detallesFactura.TotalVenta;

                    var transaccionE = await _detallesTransacRep.ConsultarTransaccionesDetalles(transaccion);

                    await _facturaVentaRep.ActualizarFacturaVenta(id, false);
                    await _detallesTransacRep.EliminarDetallesTransaccion(transaccionE.IdTransaccion);
                    await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, montoTotal, Libro.Descripcion, Libro.Conciliado);

                    return Json(new { success = true, message = "Factura anulada correctamente." });
                }

                return Json(new { success = false, message = "Error al anular la factura." });

            }
            catch
            {
                return Json(new { success = false, message = "Error al anular la factura." });
            }
        }

        // Anular Factura
        private async Task<bool> AnularFactura(int idFactura)
        {
            string requestUri = "https://api.alegra.com/api/v1/invoices/" + idFactura + "/void";

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestUri),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==" },
                    },
                };

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error en la solicitud HTTP: " + errorContent);
                    return false;
                }
            }
        }
        
        // Enviar Factura
        private async Task<string> FacturaImprimir(int idFactura)
        {
            string requestUri = "https://api.alegra.com/api/v1/invoices/" + idFactura + "?fields=pdf";

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUri),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==" },
                    },
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(content);

                    string pdfUrl = (string)jsonObject["pdf"];
                    return pdfUrl;
                };
            }
        }

        // Contacto Factura Electronica
        private async Task<int> CrearContacto(string tipoId, long id, string nombre, string correo, int telefono)
        {
            int idContacto = 2;

            if (id == 0)
            {
                return idContacto;
            }

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.alegra.com/api/v1/contacts"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==" },
                    },
                    Content = new StringContent("{\"identificationObject\":{\"type\":\"" + tipoId + "\",\"number\":\"" + id + "\"},\"name\":\"" + nombre + "\",\"phonePrimary\":\"" + telefono + "\",\"type\":\"client\",\"status\":\"active\",\"email\":\"" + correo + "\"}")
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                    }
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(body);

                    idContacto = (int)jsonObject["id"];
                    return idContacto;
                }
            }
        }

        // Envio Facura Electronica
        private async Task<bool> EnvioFactura(int idFactura, string CorreoElectronico)
        {
            string requestUri = "https://api.alegra.com/api/v1/invoices/" + idFactura + "/email";

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestUri),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==" },
                    },
                    Content = new StringContent($"{{\"emails\":[\"{CorreoElectronico}\"]}}", Encoding.UTF8, "application/json")
                    {
                        Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
                    }
                };

                try
                {
                    using (var response = await client.SendAsync(request))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Error en la solicitud HTTP: " + errorContent);
                            return false; 
                        }

                        var body = await response.Content.ReadAsStringAsync();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inesperado: " + ex.Message);
                    return false; 
                }
            }
        }

        // Lista Platillos
        private async Task<dynamic[]> ListaPlatillos(int[] idPlatillos, int[] cantidades)
        {
            List<dynamic> items = new List<dynamic>();
            for (int i = 0; i < idPlatillos.Length; i++)
            {
                var platillos = await _platilloRep.ConsultarPlatillos(idPlatillos[i]);

                dynamic item = new
                {
                    id = idPlatillos[i],
                    tax = new[] { new { id = 1 } },
                    price = platillos.PrecioVenta,
                    quantity = cantidades[i]
                };
                items.Add(item);
            }
            return items.ToArray();
        }

        // Metodo de Pago
        private string MetodosPago(int idTipoPago)
        {
            switch (idTipoPago)
            {
                case 1:
                    return "CASH";
                case 2:
                    return "TRANSFER";
                case 3:
                    return "CARD";
                case 4:
                    return "OTHER";
                default:
                    return "OTHER";
            }
        }

        // Reporte Facturas Excel o Pdf
        public async Task<IActionResult> ReporteFacturas(FacturaVenta facturaVenta)
        {
            var facturas = await _facturaVentaRep.MostrarFacturasVentas();
            var negocio = await _negociosRep.MostrarNegocio();
            var facturasMes = facturas.Where(f => f.FechaFactura.Year == facturaVenta.FechaReporte.Year && f.FechaFactura.Month == facturaVenta.FechaReporte.Month).ToList();

            string mes = facturaVenta.FechaReporte.ToString("MMMM", new CultureInfo("es-ES"));
            string year = facturaVenta.FechaReporte.Year.ToString();
            int totalFacturas = facturasMes.Count();
            decimal subtotalVentas = facturasMes.Sum(f => f.SubTotal);
            decimal totalVentas = facturasMes.Sum(f => f.TotalVenta);

            var viewModel = new FacturaVentasPDF
            {
                Mes = mes,
                Year = year,
                Negocio = negocio,
                Facturas = facturasMes,
                TotalVentas = totalVentas,
                TotalFacturas = totalFacturas,
                SubtotalVentas = subtotalVentas
            };

            if (facturaVenta.TipoReporte == "Pdf")
            {
                return new ViewAsPdf("DescargarFacturasVentas", viewModel)
                {
                    FileName = $"ReporteVentas_{mes}.pdf",
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
                        CultureInfo ci = new CultureInfo("es-CR");

                        var worksheet = workbook.Worksheets.Add("Ventas");
                        var currentRow = 1;

                        worksheet.Cell(currentRow, 1).Value = "ID";
                        worksheet.Cell(currentRow, 2).Value = "Consecutivo";
                        worksheet.Cell(currentRow, 3).Value = "Fecha";
                        worksheet.Cell(currentRow, 4).Value = "Tipo de Pago";
                        worksheet.Cell(currentRow, 5).Value = "SubTotal Venta";
                        worksheet.Cell(currentRow, 6).Value = "Total Venta";

                        worksheet.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:F1").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A1:F1").Style.Font.Bold = true;

                        foreach (var factura in viewModel.Facturas)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = factura.IdFacturaVenta;
                            worksheet.Cell(currentRow, 2).Style.NumberFormat.Format = " ###0";
                            worksheet.Cell(currentRow, 2).Value = factura.Consecutivo;
                            worksheet.Cell(currentRow, 3).Value = factura.FechaFactura.ToString("dd/MM/yyyy");
                            worksheet.Cell(currentRow, 4).Value = factura.NombrePago;
                            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = ci.NumberFormat.CurrencySymbol + " #,##0.00";
                            worksheet.Cell(currentRow, 5).Value = factura.SubTotal;
                            worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = ci.NumberFormat.CurrencySymbol + " #,##0.00";
                            worksheet.Cell(currentRow, 6).Value = factura.TotalVenta;
                        }

                        worksheet.Range("A2:F" + currentRow).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A2:F" + currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:F" + currentRow).SetAutoFilter();

                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = "Total Ventas";
                        worksheet.Cell(currentRow, 6).Value = viewModel.TotalVentas;
                        worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = ci.NumberFormat.CurrencySymbol + " #,##0.00";
                        worksheet.Range("A" + currentRow + ":" + "F" + currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A" + currentRow + ":" + "F" + currentRow).Style.Font.Bold = true;
                        worksheet.Range("A" + currentRow + ":" + "E" + currentRow).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Cell(currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A" + currentRow + ":" + "E" + currentRow).Merge();

                        worksheet.Columns().AdjustToContents();

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reporte Ventas {mes}.xlsx");
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
