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
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PupuseriaSalvadorena.Controllers
{
    public class ConciliacionBancariasController : Controller
    {
        private readonly IConciliacionRep _conciliacionRep;
        private readonly IRegistrosBancariosRep _registrosBancariosRep;
        private readonly IRegistroLibrosRep _registrosLibrosRep;
        private readonly IDetallesTransacRep _detallesTransacRep;

        public ConciliacionBancariasController(IConciliacionRep context, IRegistrosBancariosRep registrosBancariosRep, IRegistroLibrosRep registrosLibrosRep, IDetallesTransacRep detallesTransacRep, MiDbContext CContext)
        {
            _conciliacionRep = context;
            _registrosBancariosRep = registrosBancariosRep;
            _registrosLibrosRep = registrosLibrosRep;
            _detallesTransacRep = detallesTransacRep;
        }

        // GET: ConciliacionBancarias
        public async Task<IActionResult> Index()
        {
            var conciliacionBancarias = await _conciliacionRep.MostrarConciliacionesBancarias();
            return View(conciliacionBancarias);
        }

        // GET: ConciliacionBancarias/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }

            return View(conciliacionBancaria);
        }

        // GET: ConciliacionBancarias/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var registrosBancarios = await _registrosBancariosRep.MostrarRegistrosBancarios();
            var registrosLibros = await _registrosLibrosRep.MostrarRegistrosLibros();

            ViewBag.RegistrosBancarios = new SelectList(registrosBancarios, "IdRegistroBancario", "IdRegistroBancario");
            ViewBag.RegistrosLibros = new SelectList(registrosLibros, "IdRegistroLibros", "IdRegistroLibros");
            return PartialView("_newConciliacionPartial", new ConciliacionBancaria());
        }

        // POST: ConciliacionBancarias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConciliacion,FechaConciliacion,SaldoBancario,SaldoLibro,Diferencia,Observaciones,IdRegistro,IdRegistroLibros")] ConciliacionBancaria conciliacionBancaria)
        {
            if (ModelState.IsValid)
            {
                var registroBancario = await _registrosBancariosRep.ConsultarRegistrosBancarios(conciliacionBancaria.IdRegistro);
                var transaccioneslibros = await _detallesTransacRep.ConsultarTransacciones(conciliacionBancaria.IdRegistroLibros);
                var registroLibros = await _registrosLibrosRep.ConsultarRegistrosLibros(conciliacionBancaria.IdRegistroLibros);
                var contenidoPDF = registroBancario.EstadoBancario;

                var transaccionesPDF = ProcesarPDF(contenidoPDF, out decimal SaldoCorte);

                var diferencias = CompararTransacciones(transaccioneslibros, transaccionesPDF);




                await _conciliacionRep.CrearConciliacion(conciliacionBancaria.FechaConciliacion, conciliacionBancaria.SaldoBancario, conciliacionBancaria.SaldoLibro, conciliacionBancaria.Diferencia, conciliacionBancaria.Observaciones, conciliacionBancaria.IdRegistro, conciliacionBancaria.IdRegistroLibros);
                return Json(new { success = true, message = "Conciliacion creada correctamente." });
            }
            return Json(new { success = false, message = "Error al crear la conciliacion." });
        }

        // GET: ConciliacionBancarias/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }
            return View(conciliacionBancaria);
        }

        // POST: ConciliacionBancarias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdConciliacion,FechaConciliacion,SaldoBancario,SaldoLibro,Diferencia,Observaciones,IdRegistro,IdRegistroLibros")] ConciliacionBancaria conciliacionBancaria)
        {
            if (id != conciliacionBancaria.IdConciliacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _conciliacionRep.ActualizarConciliacion(conciliacionBancaria.IdConciliacion, conciliacionBancaria.SaldoBancario, conciliacionBancaria.SaldoLibro, conciliacionBancaria.Diferencia, conciliacionBancaria.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(conciliacionBancaria);
        }

        // GET: ConciliacionBancarias/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }

            return View(conciliacionBancaria);
        }

        // POST: ConciliacionBancarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _conciliacionRep.EliminarConciliacion(id);
            return RedirectToAction(nameof(Index));
        }

        public List<TransaccionesBancarias> ProcesarPDF(byte[] contenidoPDF, out decimal SaldoCorte)
        {
            var transacciones = new List<TransaccionesBancarias>();
            SaldoCorte = 0;
            bool Paginas = false;
            string textoAcumulado = "";
            
            using(var pdf = PdfDocument.Open(contenidoPDF))
            {
                foreach(var page in pdf.GetPages())
                {
                    var text = page.Text;
                    var inicio = text.IndexOf("NO. REFERENCIA");
                    var fin = text.LastIndexOf("SERVICIO AL CLIENTE");

                    if (inicio != -1 && fin != -1 && fin > inicio)
                    {
                        var transaccionesTexto = text.Substring(inicio, fin - inicio);
                        
                        if (Paginas)
                        {
                            transaccionesTexto = textoAcumulado + transaccionesTexto;
                            Paginas = false;
                        }

                        if (TransaccionesPaginas(transaccionesTexto, out textoAcumulado, out var saldoTemp))
                        {
                            Paginas = true;
                            SaldoCorte = saldoTemp;
                        }
                        else
                        {
                            transacciones.AddRange(ProcesarTransacciones(transaccionesTexto));
                        }
                    }
                }
            }

            if (Paginas && !string.IsNullOrEmpty(textoAcumulado))
            {
                transacciones.AddRange(ProcesarTransacciones(textoAcumulado));
            }

            return transacciones;
        }

        private bool TransaccionesPaginas(string text, out string textoRestante, out decimal saldoTemp)
        {
            textoRestante = string.Empty;
            saldoTemp = 0;
            var lineas = text.Split("\n");
            var lineaSaldoCorte = lineas.LastOrDefault(line => line.Contains("SALDO AL CORTE"));

            if (lineaSaldoCorte != null)
            {
                var matchSaldo = Regex.Match(lineaSaldoCorte, @"SALDO AL CORTE\s+([\d,]+\.?\d*)");
                if (matchSaldo.Success)
                {
                    var valorSaldo = matchSaldo.Groups[1].Value.Replace(",", "");
                    saldoTemp = decimal.Parse(valorSaldo, CultureInfo.InvariantCulture);
                }
            }

            var ultimaLinea = lineas.LastOrDefault(line => !String.IsNullOrWhiteSpace(line) && !line.Contains("SERVICIO AL CLIENTE"));
            
            if (String.IsNullOrEmpty(ultimaLinea) || ultimaLinea.Contains("SALDO AL CORTE"))
            {
                return false;
            }

            var regexCompleto = new Regex(@"^\d{7,10}\s+\w{3}\d{2}\s+");

            if (!regexCompleto.IsMatch(ultimaLinea))
            {
                textoRestante = ultimaLinea;
                return true;
            }

            return false;
        }

        private List<TransaccionesBancarias> ProcesarTransacciones(string textoTransacciones)
        {
            var transacciones = new List<TransaccionesBancarias>();
            var lineas = textoTransacciones.Split("\n");

            foreach (var linea in lineas)
            {
                var regexTransaccionCompleta = new Regex(@"^(?<Referencia>\d+)\s+(?<Fecha>\w{3}\d{2})\s+(?<Concepto>.+?)\s+(?<Debitos>\d+\.\d{2})\s+(?<Creditos>\d+\.\d{2})$");
                var match = regexTransaccionCompleta.Match(linea);

                if (match.Success)
                {
                    var transaccion = new TransaccionesBancarias
                    {
                        Referencia = match.Groups["Referencia"].Value,
                        Fecha = DateTime.ParseExact(match.Groups["Fecha"].Value, "MMMdd", CultureInfo.InvariantCulture),
                        Concepto = match.Groups["Concepto"].Value.Trim(),
                        Debitos = decimal.Parse(match.Groups["Debitos"].Value, CultureInfo.InvariantCulture),
                        Creditos = decimal.Parse(match.Groups["Creditos"].Value, CultureInfo.InvariantCulture)
                    };
                    transacciones.Add(transaccion);
                }
            }

            return transacciones;
        }

        private List<DiferenciaTransacciones> CompararTransacciones(List<DetalleTransaccion> transaccionesLibro, List<TransaccionesBancarias> transaccionesPDF)
        {
            var diferencias = new List<DiferenciaTransacciones>();

            foreach (var transLibro in transaccionesLibro)
            {
                var montoTransaccion = transLibro.Monto;
                var fechaTransaccion = transLibro.FechaTrans;

                var transPDFDebito = transaccionesPDF.FirstOrDefault(t => t.Fecha.Date == fechaTransaccion.Date &&
                                                                          t.Debitos == montoTransaccion);

                var transPDFCredito = transaccionesPDF.FirstOrDefault(t => t.Fecha.Date == fechaTransaccion.Date &&
                                                                           t.Creditos == montoTransaccion);

                if (transPDFDebito == null && transPDFCredito == null)
                {
                    diferencias.Add(new DiferenciaTransacciones
                    {
                        Fecha = fechaTransaccion,
                        Monto = montoTransaccion,
                        Tipo = "Solo en Libro Contable"
                    });
                }
            }

            foreach (var transPDF in transaccionesPDF)
            {
                var DebitoPDF = transPDF.Debitos;
                var CreditoPDF = transPDF.Creditos;
                var fechaPDF = transPDF.Fecha;

                var transLibroDebito = transaccionesLibro.FirstOrDefault(t => t.FechaTrans.Date == fechaPDF.Date &&
                                                                              t.Monto == DebitoPDF);

                var transLibroCredito = transaccionesLibro.FirstOrDefault(t => t.FechaTrans.Date == fechaPDF.Date &&
                                                                               t.Monto == CreditoPDF);

                if (transLibroDebito == null && transLibroCredito == null)
                {
                    diferencias.Add(new DiferenciaTransacciones
                    {
                        Fecha = fechaPDF,
                        Monto = DebitoPDF > 0 ? DebitoPDF : CreditoPDF,
                        Tipo = "Solo en Estado Bancario"
                    });
                }
            }

            return diferencias;
        }
    }
}
