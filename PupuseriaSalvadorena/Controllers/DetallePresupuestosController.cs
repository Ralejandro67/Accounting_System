using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.ViewModels;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using PupuseriaSalvadorena.Filtros;

namespace PupuseriaSalvadorena.Controllers
{
    public class DetallePresupuestosController : Controller
    {
        private readonly IDetallesPresupuestoRep _detallesPresupuestoRep;
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly IPresupuestoRep _presupuestoRep;

        public DetallePresupuestosController(IDetallesPresupuestoRep context, IDetallesTransacRep detallesTransacRep, IPresupuestoRep presupuestoRep)
        {
            _detallesPresupuestoRep = context;
            _detallesTransacRep = detallesTransacRep;
            _presupuestoRep = presupuestoRep;
        }

        // GET: DetallePresupuestos
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Error al buscar las transacciones." });
            }

            var detallePresupuesto = await _detallesPresupuestoRep.ConsultarTransacPresupuestos(id);
            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(id);

            ViewBag.Presupuesto = presupuesto.NombreP;

            if (detallePresupuesto == null)
            {
                return Json(new { success = false, message = "El presupuesto no tiene transacciones asociadas." });
            }

            return View(detallePresupuesto);    
        }

        // POST: DetallePresupuestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetallesPresupuestos detallePresupuesto)
        {
            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(detallePresupuesto.Presupuesto.IdPresupuesto);
            if (presupuesto.Estado == false)
            {
                return Json(new { success = false, message = "El presupuesto ya fue cerrado, no se pueden agregar transacciones." });
            }

            foreach (var idTransaccion in detallePresupuesto.TransaccionesSeleccionadas)
            {
                var transaccionP = await _detallesPresupuestoRep.ConsultarDetallesPresupuestos(detallePresupuesto.Presupuesto.IdPresupuesto, idTransaccion);
                if (transaccionP != null)
                {
                    return Json(new { success = false, message = $"La transacción '{transaccionP.Observaciones}' ya fue agregada al presupuesto." });
                }
            }

            if (detallePresupuesto.TransaccionesSeleccionadas != null && detallePresupuesto.TransaccionesSeleccionadas.Count > 0)
            {
                decimal saldoUsado = presupuesto.SaldoUsado;

                foreach (var idTransaccion in detallePresupuesto.TransaccionesSeleccionadas)
                {
                    var transaccion = await _detallesTransacRep.ConsultarDetallesTransacciones(idTransaccion);
                    if (transaccion != null)
                    {
                        await _detallesPresupuestoRep.CrearDetallesPresupuesto(
                            detallePresupuesto.Presupuesto.IdPresupuesto,
                            transaccion.IdRegistroLibros,
                            idTransaccion,
                            DateTime.Now,
                            transaccion.DescripcionTransaccion
                        );

                        saldoUsado += transaccion.Monto;
                    }
                }
                
                await _presupuestoRep.ActualizarPresupuesto(detallePresupuesto.Presupuesto.IdPresupuesto, presupuesto.NombreP, presupuesto.FechaInicio, presupuesto.FechaFin, presupuesto.Descripcion, saldoUsado, presupuesto.SaldoPresupuesto, presupuesto.Estado, presupuesto.IdCategoriaP.Value);
                return Json(new { success = true, message = "Transacciones incluidas correctamente." });
            }
            return Json(new { success = false, message = "Debes seleccionar al menos una transaacion." });
        }

        // GET: DetallePresupuestos/Delete/5
        public async Task<IActionResult> Delete(string IdPresupuesto, int? id)
        {
            var detallePresupuesto = await _detallesPresupuestoRep.ConsultarDetallesPresupuestos(IdPresupuesto, id.Value);
            return Json(new { exists = detallePresupuesto != null });
        }

        // POST: DetallePresupuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string IdPresupuesto, int id)
        {
            try
            {
                var detallePresupuesto = await _detallesPresupuestoRep.ConsultarDetallesPresupuestos(IdPresupuesto, id);
                var presupuesto = await _presupuestoRep.ConsultarPresupuestos(IdPresupuesto);

                await _detallesPresupuestoRep.EliminarDetallesPresupuesto(IdPresupuesto, id);
                await _presupuestoRep.ActualizarPresupuesto(presupuesto.IdPresupuesto, presupuesto.NombreP, presupuesto.FechaInicio, presupuesto.FechaFin, presupuesto.Descripcion, presupuesto.SaldoUsado - detallePresupuesto.Monto, presupuesto.SaldoPresupuesto, presupuesto.Estado, presupuesto.IdCategoriaP.Value);

                return Json(new { success = true, message = "Transaccion eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la transaccion." });
            }
        }
    }
}
