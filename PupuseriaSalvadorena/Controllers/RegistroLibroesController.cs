using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.ViewModels;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using ClosedXML.Excel;
using Rotativa.AspNetCore;

namespace PupuseriaSalvadorena.Controllers
{
    public class RegistroLibroesController : Controller
    {
        private readonly IRegistroLibrosRep _registroLibroRep;   
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly INegociosRep _negociosRep;

        public RegistroLibroesController(IRegistroLibrosRep context, IDetallesTransacRep detallesTransacRep, INegociosRep negociosRep)
        {
            _registroLibroRep = context;
            _detallesTransacRep = detallesTransacRep;
            _negociosRep = negociosRep;
            LibrosMensual();
        }

        // GET: RegistroLibroes
        public async Task<IActionResult> Index()
        {
            var registroLibro = await _registroLibroRep.MostrarRegistrosLibros();
            return View(registroLibro);
        }

        // GET: RegistroLibroes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroLibro = await _registroLibroRep.ConsultarRegistrosLibros(id);
            var transacciones = await _detallesTransacRep.ConsultarTransacciones(id);
            var totales = transacciones.Count();

            var Ingresos = transacciones.Where(dt => dt.IdMovimiento == 1).Sum(dt => dt.Monto);
            var cantIngresos = transacciones.Where(dt => dt.IdMovimiento == 1).Count();

            var Egresos = transacciones.Where(dt => dt.IdMovimiento == 2).Sum(dt => dt.Monto);
            var cantEgresos = transacciones.Where(dt => dt.IdMovimiento == 2).Count();

            if (registroLibro == null)
            {
                return NotFound();
            }

            DetallesLibros detallesLibros = new DetallesLibros
            {
                RegistroLibro = registroLibro,
                DetallesTransacciones = transacciones
            };

            ViewBag.Totales = totales;
            ViewBag.Ingresos = Ingresos;
            ViewBag.cantIngresos = cantIngresos;
            ViewBag.Egresos = Egresos;
            ViewBag.cantEgresos = cantEgresos;
            ViewBag.Saldo = registroLibro.MontoTotal;

            return View(detallesLibros);
        }

        // POST: RegistroLibroes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistroLibros,FechaRegistro,MontoTotal,Descripcion")] RegistroLibro registroLibro)
        {
            if (ModelState.IsValid)
            {
                await _registroLibroRep.CrearRegistroLibros(registroLibro.FechaRegistro, registroLibro.MontoTotal, registroLibro.Descripcion, registroLibro.Conciliado);
                return Json(new { success = true, message = "Libro agregado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return Json(new { success = false, errors = errors });
            }
        }

        // GET: RegistroLibroes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroLibro = await _registroLibroRep.ConsultarRegistrosLibros(id);
            if (registroLibro == null)
            {
                return NotFound();
            }

            return PartialView("_editLibroPartial", registroLibro);
        }

        // POST: RegistroLibroes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdRegistroLibros,FechaRegistro,MontoTotal,Descripcion")] RegistroLibro registroLibro)
        {
            if (id != registroLibro.IdRegistroLibros)
            {
                return Json(new { success = false, message = "Libro no encontrado." });
            }

            if (ModelState.IsValid)
            {
                await _registroLibroRep.ActualizarRegistroLibros(registroLibro.IdRegistroLibros, registroLibro.MontoTotal, registroLibro.Descripcion, registroLibro.Conciliado);
                return Json(new { success = true, message = "Libro actualizado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return Json(new { success = false, errors = errors });
            }
        }

        // GET: RegistroLibroes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var registroLibro = await _registroLibroRep.ConsultarRegistrosLibros(id);
            return Json(new { exists = registroLibro != null });
        }

        // POST: RegistroLibroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _registroLibroRep.EliminarRegistroLibros(id);
                return Json(new { success = true, message = "Libro eliminado correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el libro." });
            }
        }

        // Reporte Excel
        public async Task<IActionResult> ExcelLibroContable(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroLibro = await _registroLibroRep.ConsultarRegistrosLibros(id);
            var transacciones = await _detallesTransacRep.ConsultarTransacciones(id);

            var totales = transacciones.Count();

            var Ingresos = transacciones.Where(dt => dt.IdMovimiento == 1).Sum(dt => dt.Monto);
            var cantIngresos = transacciones.Where(dt => dt.IdMovimiento == 1).Count();

            var Egresos = transacciones.Where(dt => dt.IdMovimiento == 2).Sum(dt => dt.Monto);
            var cantEgresos = transacciones.Where(dt => dt.IdMovimiento == 2).Count();

            if (registroLibro == null)
            {
                return NotFound();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Libro Contable");
                var currentRow = 3;

                worksheet.Cell("A1").Value = "Ingresos";
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Range("A1:B1").Merge();

                worksheet.Cell("A2").Value = "Total";
                worksheet.Cell("A2").Style.Font.Bold = true;
                worksheet.Cell("B2").Value = "Valor";
                worksheet.Cell("B2").Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Value = cantIngresos;
                worksheet.Cell(currentRow, 2).Value = Ingresos;

                worksheet.Cell("C1").Value = "Egresos";
                worksheet.Cell("C1").Style.Font.Bold = true;
                worksheet.Range("C1:D1").Merge();

                worksheet.Cell("C2").Value = "Total";
                worksheet.Cell("C2").Style.Font.Bold = true;
                worksheet.Cell("D2").Value = "Valor";
                worksheet.Cell("D2").Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Value = cantEgresos;
                worksheet.Cell(currentRow, 4).Value = Egresos;

                worksheet.Cell("E1").Value = "Saldo";
                worksheet.Cell("E1").Style.Font.Bold = true;
                worksheet.Range("E1:F1").Merge();

                worksheet.Cell("E2").Value = "Total";
                worksheet.Cell("E2").Style.Font.Bold = true;
                worksheet.Cell("F2").Value = "Valor";
                worksheet.Cell("F2").Style.Font.Bold = true;
                worksheet.Cell(currentRow, 5).Value = totales;
                worksheet.Cell(currentRow, 6).Value = registroLibro.MontoTotal;

                worksheet.Range("A1:F2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);

                currentRow += 2;
                worksheet.Cell(currentRow, 1).Value = "ID Factura";
                worksheet.Cell(currentRow, 2).Value = "Fecha";
                worksheet.Cell(currentRow, 3).Value = "Descripcion";
                worksheet.Cell(currentRow, 4).Value = "Cantidad";
                worksheet.Cell(currentRow, 5).Value = "Debito";
                worksheet.Cell(currentRow, 6).Value = "Tipo de Transaccion";
                worksheet.Range("A5:F5").Style.Font.Bold = true;
                worksheet.Range("A5:F5").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);

                foreach (var transaccionesLibro in transacciones)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = transaccionesLibro.IdTransaccion;
                    worksheet.Cell(currentRow, 2).Value = transaccionesLibro.FechaTrans.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 3).Value = transaccionesLibro.DescripcionTransaccion;
                    worksheet.Cell(currentRow, 4).Value = transaccionesLibro.Cantidad;
                    worksheet.Cell(currentRow, 5).Value = transaccionesLibro.Monto;
                    worksheet.Cell(currentRow, 6).Value = transaccionesLibro.TipoTransac;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{registroLibro.Descripcion}.xlsx");
                }
            }
        }

        // Reporte Pdf
        public async Task<IActionResult> DescargarLibro(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroLibro = await _registroLibroRep.ConsultarRegistrosLibros(id);
            var transacciones = await _detallesTransacRep.ConsultarTransacciones(id);
            var negocio = await _negociosRep.MostrarNegocio();

            var totales = transacciones.Count();

            var Ingresos = transacciones.Where(dt => dt.IdMovimiento == 1).Sum(dt => dt.Monto);
            var cantIngresos = transacciones.Where(dt => dt.IdMovimiento == 1).Count();

            var Egresos = transacciones.Where(dt => dt.IdMovimiento == 2).Sum(dt => dt.Monto);
            var cantEgresos = transacciones.Where(dt => dt.IdMovimiento == 2).Count();

            var viewModel = new LibroContablePDF
            {
                totalTransacciones = totales,
                valorIngresos = Ingresos,
                totalIngresos = cantIngresos,
                valorEgresos = Egresos,
                totalEgresos = cantEgresos,
                Negocio = negocio,
                RegistroLibro = registroLibro,
                DetallesTransacciones = transacciones
            };

            return new ViewAsPdf("DescargarLibro", viewModel)
            {
                FileName = $"{registroLibro.Descripcion}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 12, 12, 12)
            };
        }


        // Creacion Libros Mensuales
        public void LibrosMensual()
        {
            RecurringJob.AddOrUpdate(
                "CrearLibroMensual",
                () => CrearLibroMensual(),
                "0 0 1 * *");
        }

        public void CrearLibroMensual()
        {
            DateTime FechaResgistro = DateTime.Now;
            CultureInfo cultura = new CultureInfo("es-ES");
            DateTime fechaMes = FechaResgistro.AddMonths(1);
            string Mes = fechaMes.ToString("MMMM", cultura);
            string Libro = "Libro de " + Mes;

            _registroLibroRep.CrearRegistroLibros(FechaResgistro, 0, Libro, false);
        }
    }
}
