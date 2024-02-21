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
using System.Text;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using System.Globalization;

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
        public async Task<IActionResult> Index()
        {
            CultureInfo cultura = new CultureInfo("es-ES");
            DateTime fecha = DateTime.Now;

            var facturaVentas = await _facturaVentaRep.MostrarFacturasVentas();
            var facturasPorMes = facturaVentas.GroupBy(f => new { Año = f.FechaFactura.Year, Mes = f.FechaFactura.Month })
                                               .Select(group => new { Año = group.Key.Año, Mes = group.Key.Mes, Cantidad = group.Count(), TotalVentasMes = group.Sum(f => f.TotalVenta) })
                                               .OrderByDescending(x => x.Año).ThenByDescending(x => x.Mes)
                                               .ToList();

            var facturasInvertidas = facturasPorMes.AsEnumerable().Reverse().ToList();
            ViewBag.Meses = facturasInvertidas.Select(x => new DateTime(x.Año, x.Mes, 1).ToString("MMM yyyy", cultura)).ToList();
            ViewBag.ventasPorMes = facturasInvertidas.Select(x => x.TotalVentasMes).ToList();

            ViewBag.totalVentas = facturaVentas.Sum(f => f.TotalVenta);
            ViewBag.totalVentasMes = facturasPorMes.FirstOrDefault()?.TotalVentasMes ?? 0;
            ViewBag.facturasMes = facturasPorMes.FirstOrDefault()?.Cantidad ?? 0;
            ViewBag.mesActual = cultura.DateTimeFormat.GetMonthName(fecha.Month);
            ViewBag.facturas = facturaVentas.Count();

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
        public async Task<IActionResult> Create([Bind("IdFacturaVenta,FechaFactura,SubTotal,TotalVenta,IdTipoPago,IdTipoFactura,Identificacion,NombreCliente,CorreoElectronico,Telefono,IdPlatillo,CantVenta,IdTipoVenta,FacturaElectronica,TipoId")] FacturaVenta facturaVenta)
        {
            if (ModelState.IsValid)
            {
                int[] idPlatillos = facturaVenta.IdPlatillo;
                int[] cantidades = facturaVenta.CantVenta;

                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);
                var empresa = await _negociosRep.ConsultarNegocio();
                var Pago = MetodosPago(facturaVenta.IdTipoPago);

                int ContactoFactura = await CrearContacto(facturaVenta.TipoId, facturaVenta.Identificacion, facturaVenta.NombreCliente, facturaVenta.CorreoElectronico, facturaVenta.Telefono);

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

                            var facturaId = await _facturaVentaRep.CrearFacturaVenta(empresa, facturaVenta.Consecutivo, facturaVenta.FechaFactura, facturaVenta.SubTotal, facturaVenta.TotalVenta, facturaVenta.IdTipoPago, facturaVenta.IdTipoFactura);

                            int cantTotal = 0;
                            decimal montoTotal = Libro.MontoTotal + facturaVenta.TotalVenta;
                            await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, montoTotal, Libro.Descripcion, Libro.Conciliado);

                            for (int i = 0; i < idPlatillos.Length; i++)
                            {
                                cantTotal += cantidades[i];
                                await _historialVentaRep.CrearHistorialVenta(idfactura, cantidades[i], facturaVenta.FechaFactura, idPlatillos[i], facturaId, facturaVenta.IdTipoVenta);
                            }

                            await _detallesTransacRep.CrearDetalleTransaccion(IdLibro, $"Factura de Venta: {facturaVenta.Consecutivo}", cantTotal, facturaVenta.TotalVenta, facturaVenta.FechaFactura, 2, "TAX002", false, facturaVenta.FechaFactura, "No Recurrente", false);

                            if (ContactoFactura != 2)
                            {
                                bool envio = await EnvioFactura(idfactura, facturaVenta.CorreoElectronico);

                                if (envio)
                                {
                                    await _envioFacturaRep.CrearEnvioFactura(facturaVenta.FechaFactura, facturaId, facturaVenta.Identificacion, facturaVenta.NombreCliente, facturaVenta.CorreoElectronico, facturaVenta.Telefono);
                                    return Json(new { success = true, message = "Factura generada y enviada correctamente." });
                                }
                            }

                            return Json(new { success = true, message = "Factura generada correctamente." });
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
            return Json(new { success = false, message = "Error al crear la factura." });
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
            return View("_editFacturaVentaPartial", facturaVenta);
        }

        // POST: FacturaVentas/Edit/5
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
                            // Si no es exitoso, leer el contenido del error
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
    }
}
