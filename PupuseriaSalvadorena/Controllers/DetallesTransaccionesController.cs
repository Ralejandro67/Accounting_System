using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class DetallesTransaccionesController : Controller
    {
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly IImpuestosRep _impuestosRep;
        private readonly IRegistroLibrosRep _registroLibrosRep;
        private readonly ITipoTransacRep _tipoTransacRep;

        public DetallesTransaccionesController(IDetallesTransacRep detallesTransacRep, IImpuestosRep impuestosRep, IRegistroLibrosRep registroLibrosRep, ITipoTransacRep tipoTransacRep)
        {
            _detallesTransacRep = detallesTransacRep;
            _impuestosRep = impuestosRep;
            _registroLibrosRep = registroLibrosRep;
            _tipoTransacRep = tipoTransacRep;
        }

        // GET: DetallesTransacciones
        public async Task<IActionResult> Index()
        {
            var detalleTransaccion = await _detallesTransacRep.MostrarDetallesTransacciones();
            return View(detalleTransaccion);
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

        // GET: DetallesTransacciones/Create
        [HttpGet]
        public async Task<IActionResult> GetDetalleTransaccionPartial()
        {
            var impuestos = await _impuestosRep.MostrarImpuestos();
            var registroLibros = await _registroLibrosRep.MostrarRegistrosLibros();
            var tipoTransac = await _tipoTransacRep.MostrarTipoTransaccion();

            ViewBag.TipoTransac = new SelectList(tipoTransac, "IdTipo", "TipoTransac");
            ViewBag.RegistroLibros = new SelectList(registroLibros, "IdRegistroLibros", "IdRegistroLibros");
            ViewBag.Impuestos = new SelectList(impuestos, "IdImpuesto", "NombreImpuesto");
            return PartialView("_newDetallesTPartial", new DetalleTransaccion());
        }

        // POST: DetallesTransacciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistroLibros,IdTransaccion,DescripcionTransaccion,Cantidad,Monto,FechaTrans,IdTipo,IdImpuesto,NombreImpuesto,TipoTransac,Recurrencia,FechaRecurrencia,Frecuencia")] DetalleTransaccion detalleTransaccion)
        {
            if (ModelState.IsValid)
            {
                if (detalleTransaccion.Recurrencia)
                {
                    var idTransaccion =  await _detallesTransacRep.CrearTransaccionRecurrente(detalleTransaccion.IdRegistroLibros, detalleTransaccion.DescripcionTransaccion, detalleTransaccion.Cantidad, detalleTransaccion.Monto, detalleTransaccion.FechaTrans, detalleTransaccion.IdTipo, detalleTransaccion.IdImpuesto, detalleTransaccion.Recurrencia, detalleTransaccion.FechaRecurrencia, detalleTransaccion.Frecuencia);
                    var cronExpression = FrecuenciaACron(detalleTransaccion.Frecuencia);

                    RecurringJob.AddOrUpdate($"transaccion_{idTransaccion}",
                                 () => TransaccionRecurrente(idTransaccion), 
                                 cronExpression);
                    
                    return Json(new { success = true, message = "Transaccion agregada correctamente." });
                }
                else
                {
                    await _detallesTransacRep.CrearDetalleTransaccion(detalleTransaccion.IdRegistroLibros, detalleTransaccion.DescripcionTransaccion, detalleTransaccion.Cantidad, detalleTransaccion.Monto, detalleTransaccion.FechaTrans, detalleTransaccion.IdTipo, detalleTransaccion.IdImpuesto, detalleTransaccion.Recurrencia, detalleTransaccion.FechaRecurrencia, detalleTransaccion.Frecuencia);
                    return Json(new { success = true, message = "Transaccion agregada correctamente." });
                }
            }
            return Json(new { success = false, message = "Error al agregar la transaccion." });
        }

        // GET: DetallesTransacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleTransaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(id.Value);
            var impuestos = await _impuestosRep.MostrarImpuestos();
            var tipoTransac = await _tipoTransacRep.MostrarTipoTransaccion();

            ViewBag.TipoTransac2 = new SelectList(tipoTransac, "IdTipo", "TipoTransac");
            ViewBag.Impuestos2 = new SelectList(impuestos, "IdImpuesto", "NombreImpuesto");

            if (detalleTransaccion == null)
            {
                return NotFound();
            }
            return PartialView("_editDetallesTPartial", detalleTransaccion);
        }

        // POST: DetallesTransacciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRegistroLibros,IdTransaccion,DescripcionTransaccion,Cantidad,Monto,FechaTrans,IdTipo,IdImpuesto,NombreImpuesto,TipoTransac")] DetalleTransaccion detalleTransaccion)
        {
            if (id != detalleTransaccion.IdTransaccion)
            {
                return Json(new { success = false, message = "Transaccion no encontrada." });
            }

            if (ModelState.IsValid)
            {
                await _detallesTransacRep.ActualizarDetalleTransaccion(detalleTransaccion.IdTransaccion, detalleTransaccion.DescripcionTransaccion, detalleTransaccion.Cantidad, detalleTransaccion.Monto, detalleTransaccion.IdTipo, detalleTransaccion.IdImpuesto);
                return Json(new { success = true, message = "Transaccion actualizada correctamente." });
            }
            return Json(new { success = false, message = "Datos inválidos." });
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

            if(TransaccionOriginal != null && TransaccionOriginal.Recurrencia) 
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
                    TransaccionOriginal.IdRegistroLibros,
                    TransaccionOriginal.DescripcionTransaccion,
                    TransaccionOriginal.Cantidad,
                    TransaccionOriginal.Monto,
                    FechaTransaccion,
                    TransaccionOriginal.IdTipo,
                    TransaccionOriginal.IdImpuesto,
                    TransaccionOriginal.Recurrencia,
                    TransaccionOriginal.FechaRecurrencia,
                    TransaccionOriginal.Frecuencia
                );
            }
        }
    }
}