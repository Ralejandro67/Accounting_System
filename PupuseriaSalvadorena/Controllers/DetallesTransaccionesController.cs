using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace PupuseriaSalvadorena.Controllers
{
    public class DetallesTransaccionesController : Controller
    {
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly IImpuestosRep _impuestosRep;
        private readonly IRegistroLibrosRep _registroLibrosRep;
        private readonly ITipoTransacRep _tipoTransacRep;
        private readonly IDetallesPresupuestoRep _detallesPresupuestoRep;

        public DetallesTransaccionesController(IDetallesTransacRep detallesTransacRep, IImpuestosRep impuestosRep, IRegistroLibrosRep registroLibrosRep, ITipoTransacRep tipoTransacRep, IDetallesPresupuestoRep detallesPresupuestoRep)
        {
            _detallesTransacRep = detallesTransacRep;
            _impuestosRep = impuestosRep;
            _registroLibrosRep = registroLibrosRep;
            _tipoTransacRep = tipoTransacRep;
            _detallesPresupuestoRep = detallesPresupuestoRep;
        }

        // GET: DetallesTransacciones
        public async Task<IActionResult> Index()
        {
            var cultura = new CultureInfo("es-ES");
            var fechaInicio = DateTime.Today.AddMonths(-4);
            var fechaActual = DateTime.Today;
            var añoActual = fechaActual.Year;
            var mesActual = fechaActual.Month;

            var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
            var detalleTransaccion = await _detallesTransacRep.MostrarDetallesTransacciones();

            var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);
            var transacciones = await _detallesTransacRep.ConsultarTransacciones(IdLibro);

            var IngresosActuales = detalleTransaccion.Where(dt => dt.IdMovimiento == 1 && dt.FechaTrans.Year == añoActual && dt.FechaTrans.Month == mesActual).Sum(dt => dt.Monto);
            var EgresosActuales = detalleTransaccion.Where(dt => dt.IdMovimiento == 2 && dt.FechaTrans.Year == añoActual && dt.FechaTrans.Month == mesActual).Sum(dt => dt.Monto);

            var transacciones4Meses = detalleTransaccion.Where(t => t.FechaTrans >= fechaInicio).ToList();
            var ingresos = transacciones4Meses.Where(dt => dt.IdMovimiento == 1).GroupBy(dt => new { dt.FechaTrans.Year, dt.FechaTrans.Month })
                                              .Select(group => new
                                              {
                                                  Año = group.Key.Year,
                                                  MesNumero = group.Key.Month,
                                                  Mes = cultura.DateTimeFormat.GetMonthName(group.Key.Month),
                                                  TotalMonto = group.Sum(dt => dt.Monto)
                                              }).OrderBy(x => x.Año).ThenBy(x => x.MesNumero).ToList();

            var egresos = transacciones4Meses.Where(dt => dt.IdMovimiento == 2).GroupBy(dt => new { dt.FechaTrans.Year, dt.FechaTrans.Month })
                                             .Select(group => new
                                             {
                                                 Año = group.Key.Year,
                                                 MesNumero = group.Key.Month,
                                                 Mes = cultura.DateTimeFormat.GetMonthName(group.Key.Month),
                                                 TotalMonto = group.Sum(dt => dt.Monto)
                                             }).OrderBy(x => x.Año).ThenBy(x => x.MesNumero).ToList();

            ViewBag.Libro = Libro.Descripcion;
            ViewBag.Mes = cultura.DateTimeFormat.GetMonthName(mesActual);
            ViewBag.MontoTotal = Libro.MontoTotal;
            ViewBag.IngresosActuales = IngresosActuales;
            ViewBag.EgresosActuales = EgresosActuales;

            ViewBag.Ingresos = JsonConvert.SerializeObject(ingresos);
            ViewBag.Egresos = JsonConvert.SerializeObject(egresos);

            return View(transacciones);
        }

        // GET: DetallesTransacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleTransaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(id.Value);
            if (detalleTransaccion == null)
            {
                return NotFound();
            }

            return View(detalleTransaccion);
        }

        // GET: DetallesTransacciones/GetDetalleTransaccionPartial
        [HttpGet]
        public async Task<IActionResult> GetDetalleTransaccionPartial()
        {
            var impuestos = await _impuestosRep.MostrarImpuestos();
            ViewBag.Impuestos = new SelectList(impuestos, "IdImpuesto", "NombreImpuesto");
            return PartialView("_newDetallesTPartial", new DetalleTransaccion());
        }

        // POST: DetallesTransacciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistroLibros,IdTransaccion,DescripcionTransaccion,Cantidad,Monto,FechaTrans,IdTipo,IdImpuesto,NombreImpuesto,TipoTransac,Recurrencia,FechaRecurrencia,Frecuencia,Conciliado")] DetalleTransaccion detalleTransaccion)
        {
            if (ModelState.IsValid)
            {
                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
                var idTransaccion = await _detallesTransacRep.CrearTransaccionRecurrente(IdLibro, detalleTransaccion.DescripcionTransaccion, detalleTransaccion.Cantidad, detalleTransaccion.Monto, detalleTransaccion.FechaTrans, detalleTransaccion.IdTipo, detalleTransaccion.IdImpuesto, detalleTransaccion.Recurrencia, detalleTransaccion.FechaRecurrencia, detalleTransaccion.Frecuencia, false);
                var transaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(idTransaccion);
                var registroLibro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);

                decimal MontoLibro = 0;

                if (transaccion.IdMovimiento == 2)
                {
                    MontoLibro = registroLibro.MontoTotal - transaccion.Monto;
                }
                else
                {
                    MontoLibro = registroLibro.MontoTotal + transaccion.Monto;
                }

                if (detalleTransaccion.Recurrencia)
                {
                    var cronExpression = FrecuenciaACron(detalleTransaccion.Frecuencia);

                    RecurringJob.AddOrUpdate($"{idTransaccion}",
                                 () => TransaccionRecurrente(idTransaccion), 
                                 cronExpression);
                }

                await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, MontoLibro, registroLibro.Descripcion, registroLibro.Conciliado);
                return Json(new { success = true, message = "Transaccion agregada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: DetallesTransacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleTransaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(id.Value);
            var impuestoTransaccion = await _impuestosRep.ConsultarImpuestos(detalleTransaccion.IdImpuesto); 
            var impuestos = await _impuestosRep.MostrarImpuestos();
            var tipos = await _tipoTransacRep.MostrarTipoTransaccion();

            var impuestoActual = (impuestoTransaccion.Tasa / 100);
            var valorSubtotal = detalleTransaccion.Monto / (1 + impuestoActual);

            ViewBag.Impuestos2 = new SelectList(impuestos, "IdImpuesto", "NombreImpuesto");
            ViewBag.Tipos = new SelectList(tipos, "IdTipo", "TipoTransac");
            ViewBag.Monto = detalleTransaccion.Monto;
            ViewBag.ValorSubtotal = valorSubtotal;
            ViewBag.valorImpuesto = detalleTransaccion.Monto - valorSubtotal;

            if (detalleTransaccion == null)
            {
                return NotFound();
            }
            return PartialView("_editDetallesTPartial", detalleTransaccion);
        }

        // POST: DetallesTransacciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRegistroLibros,IdTransaccion,DescripcionTransaccion,Cantidad,Monto,FechaTrans,IdTipo,IdImpuesto,NombreImpuesto,TipoTransac,Recurrencia,FechaRecurrencia,Frecuencia,Conciliado")] DetalleTransaccion detalleTransaccion)
        {
            if (id != detalleTransaccion.IdTransaccion)
            {
                return Json(new { success = false, message = "Transaccion no encontrada." });
            }

            if (ModelState.IsValid)
            {
                var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
                var libro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);
                var transaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(detalleTransaccion.IdTransaccion);
                decimal MontoLibro;

                if (detalleTransaccion.IdMovimiento == 2)
                {
                    MontoLibro = (libro.MontoTotal + transaccion.Monto) - detalleTransaccion.Monto;
                }
                else
                {
                    MontoLibro = (libro.MontoTotal - transaccion.Monto) + detalleTransaccion.Monto;
                }

                await _detallesTransacRep.ActualizarDetalleTransaccion(detalleTransaccion.IdTransaccion, detalleTransaccion.DescripcionTransaccion, detalleTransaccion.Cantidad, detalleTransaccion.Monto, detalleTransaccion.IdTipo, detalleTransaccion.IdImpuesto, false);
                await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, MontoLibro, libro.Descripcion, libro.Conciliado);
                return Json(new { success = true, message = "Transaccion actualizada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: DetallesTransacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var detalleTransaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(id.Value);
            return Json(new { exists = detalleTransaccion != null });
        }

        // POST: DetallesTransacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                string jobId = id.ToString();
                RecurringJob.RemoveIfExists(jobId);

                var detalleTransaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(id);
                var Libro = await _registroLibrosRep.ConsultarRegistrosLibros(detalleTransaccion.IdRegistroLibros);

                decimal MontoLibro;

                if (detalleTransaccion.IdMovimiento == 2)
                {
                    MontoLibro = Libro.MontoTotal + detalleTransaccion.Monto;
                }
                else
                {
                    MontoLibro = Libro.MontoTotal - detalleTransaccion.Monto;
                }

                await _registroLibrosRep.ActualizarRegistroLibros(detalleTransaccion.IdRegistroLibros, MontoLibro, Libro.Descripcion, Libro.Conciliado);

                await _detallesTransacRep.EliminarDetallesTransaccion(id);
                return Json(new { success = true, message = "Transaccion eliminada correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar la transaccion." });
            }
        }

        // FrecuenciaACron convierte una frecuencia en una expresión cron.
        private string FrecuenciaACron(string frecuencia)
        {
            switch (frecuencia)
            {
                case "Semanal":
                    return Cron.Weekly();

                case "Mensual":
                    return Cron.Monthly();

                case "Anual":
                    return Cron.Yearly();

                default:
                    throw new ArgumentException("Frecuencia no válida");
            }
        }

        public async Task TransaccionRecurrente(int idTransaccion)
        {
            var TransaccionOriginal = await _detallesTransacRep.ConsultarDetallesTransacciones(idTransaccion);
            var IdLibro = await _detallesTransacRep.ObtenerIdLibroMasReciente();
            var registroLibro = await _registroLibrosRep.ConsultarRegistrosLibros(IdLibro);
            decimal MontoLibro;

            if (TransaccionOriginal.IdMovimiento == 2)
            {
                MontoLibro = registroLibro.MontoTotal - TransaccionOriginal.Monto;
            }
            else
            {
                MontoLibro = registroLibro.MontoTotal + TransaccionOriginal.Monto;
            }

            if (TransaccionOriginal != null && TransaccionOriginal.Recurrencia) 
            {
                DateTime FechaTransaccion;
                switch (TransaccionOriginal.Frecuencia) 
                {
                    case "Semanal":
                        FechaTransaccion = TransaccionOriginal.FechaTrans.AddDays(7);
                        break;
                    case "Mensual":
                        FechaTransaccion = TransaccionOriginal.FechaTrans.AddMonths(1);
                        break;
                    case "Anual":
                        FechaTransaccion = TransaccionOriginal.FechaTrans.AddYears(1);
                        break;
                    default:
                        throw new ArgumentException("Frecuencia no válida");
                }

                await _detallesTransacRep.CrearDetalleTransaccion(
                    IdLibro,
                    TransaccionOriginal.DescripcionTransaccion,
                    TransaccionOriginal.Cantidad,
                    TransaccionOriginal.Monto,
                    FechaTransaccion,
                    TransaccionOriginal.IdTipo,
                    TransaccionOriginal.IdImpuesto,
                    TransaccionOriginal.Recurrencia,
                    TransaccionOriginal.FechaRecurrencia,
                    TransaccionOriginal.Frecuencia,
                    false
                );

                await _registroLibrosRep.ActualizarRegistroLibros(IdLibro, MontoLibro, registroLibro.Descripcion, registroLibro.Conciliado);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTipoTransaccion(string IdImpuesto)
        {
            var tipotransaccion = await _tipoTransacRep.ConsultarImpuestosporTransaccion(IdImpuesto);
            return Json(new SelectList(tipotransaccion, "IdTipo", "TipoTransac"));
        }

        [HttpGet]
        public async Task<IActionResult> GetImpuesto(string IdImpuesto, decimal monto)
        {
            Impuesto impuesto = await _impuestosRep.ConsultarImpuestos(IdImpuesto);
            decimal montoImpuesto = monto * (impuesto.Tasa.Value / 100);
            decimal montoTotal = monto + montoImpuesto;
            return Json(new { montoImpuesto = montoImpuesto, montoTotal = montoTotal });
        }
    }
}