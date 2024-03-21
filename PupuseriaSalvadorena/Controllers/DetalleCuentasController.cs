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
using Rotativa.AspNetCore;
using PupuseriaSalvadorena.Filtros;

namespace PupuseriaSalvadorena.Controllers
{
    public class DetalleCuentasController : Controller
    {
        private readonly IAlertaCuentaPagarRep _alertaCuentaPagarRep;
        private readonly IDetallesCuentaRep _detallesCuentaRep;
        private readonly ICuentaPagarRep _cuentasPagarRep;
        private readonly INegociosRep _negociosRep;

        public DetalleCuentasController(IDetallesCuentaRep context, ICuentaPagarRep cuentasPagarRep, INegociosRep negociosRep, IAlertaCuentaPagarRep alertaCuentaPagarRep)
        {
            _detallesCuentaRep = context;
            _cuentasPagarRep = cuentasPagarRep;
            _negociosRep = negociosRep;
            _alertaCuentaPagarRep = alertaCuentaPagarRep;
        }

        // GET: DetalleCuentas/Details/5
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentaPagar = await _cuentasPagarRep.ConsultarCuentasPagar(id);
            var detalleCuenta = await _detallesCuentaRep.ConsultarCuentaDetalles(id);
            await _alertaCuentaPagarRep.ActualizarAlertaCuentaPagarID(id, true);

            decimal Pagado = detalleCuenta.Sum(x => x.Pago);
            decimal PorPagar = cuentaPagar.TotalPagado - Pagado;

            if (detalleCuenta == null)
            {
                return NotFound();
            }

            ViewBag.PorPagar = PorPagar;
            ViewBag.FechaVencimiento = cuentaPagar.FechaVencimiento.ToString("dd/MM/yyyy");
            ViewBag.Total = cuentaPagar.TotalPagado;
            ViewBag.Pago = Pagado;
            ViewBag.IdCuentaPagar = id;
            ViewBag.VencimientoExpirado = cuentaPagar.FechaVencimiento < DateTime.Now;
            ViewBag.PorPagarEsCero = PorPagar == 0;

            return View(detalleCuenta);
        }

        // GET: DetalleCuentas/Create
        public IActionResult Create(string id)
        {
            var detalleCuenta = new DetalleCuenta{ FechaIngreso = DateTime.Now };
            ViewBag.CuentasPagar = id;
            return PartialView("_newDetalleCuentaPartial", detalleCuenta);
        }

        // POST: DetalleCuentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetallesCuenta,Pago,FechaIngreso,IdCuentaPagar")] DetalleCuenta detalleCuenta)
        {
            if (ModelState.IsValid)
            {
                var cuentaPagar = await _cuentasPagarRep.ConsultarCuentasPagar(detalleCuenta.IdCuentaPagar);
                var detalleCuentaPagar = await _detallesCuentaRep.ConsultarCuentaDetalles(detalleCuenta.IdCuentaPagar);

                decimal Pagado = detalleCuentaPagar.Sum(x => x.Pago);
                decimal PorPagar = cuentaPagar.TotalPagado - Pagado;

                if (PorPagar == 0)
                {
                    return Json(new { success = false, message = "La cuenta ya fue pagada." });
                }

                if (detalleCuenta.Pago > PorPagar)
                {
                    return Json(new { success = false, message = "El monto pagado no puede ser mayor al monto de la cuenta por pagar." });
                }
                else
                {
                    await _detallesCuentaRep.CrearDetallesCuenta(detalleCuenta.Pago, detalleCuenta.FechaIngreso, detalleCuenta.IdCuentaPagar);
                    var pagosActualizados= await _detallesCuentaRep.ConsultarCuentaDetalles(detalleCuenta.IdCuentaPagar);

                    decimal PagadoActualizado = pagosActualizados.Sum(x => x.Pago);
                    decimal PorPagarActualizado = cuentaPagar.TotalPagado - PagadoActualizado;

                    if (PorPagarActualizado == 0)
                    {
                        await _cuentasPagarRep.ActualizarCuentaPagar(cuentaPagar.IdCuentaPagar, cuentaPagar.FechaCreacion, cuentaPagar.FechaVencimiento, cuentaPagar.TotalPagado, cuentaPagar.IdFacturaCompra, cuentaPagar.IdProveedor, false);
                    }

                    return Json(new { success = true, message = "Pago agregado correctamente." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: DetalleCuentas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCuenta = await _detallesCuentaRep.ConsultarDetallesCuentas(id);

            if (detalleCuenta == null)
            {
                return NotFound();
            }

            return PartialView("_editDetalleCuentaPartial", detalleCuenta);
        }

        // POST: DetalleCuentas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDetallesCuenta,Pago,FechaIngreso,IdCuentaPagar")] DetalleCuenta detalleCuenta)
        {
            if (ModelState.IsValid)
            {
                var cuentaPagar = await _cuentasPagarRep.ConsultarCuentasPagar(detalleCuenta.IdCuentaPagar);
                var detallesPago = await _detallesCuentaRep.ConsultarDetallesCuentas(id);
                var detalleCuentaPagar = await _detallesCuentaRep.ConsultarCuentaDetalles(detalleCuenta.IdCuentaPagar);

                decimal Pagado = detalleCuentaPagar.Sum(x => x.Pago);
                decimal PorPagar = cuentaPagar.TotalPagado - Pagado + detallesPago.Pago;

                if (PorPagar == 0)
                {
                    return Json(new { success = false, message = "La cuenta ya fue pagada." });
                }

                if (detalleCuenta.Pago > PorPagar)
                {
                    return Json(new { success = false, message = "El monto pagado no puede ser mayor al monto de la cuenta por pagar." });
                }
                else
                {
                    await _detallesCuentaRep.ActualizarDetallesCuentas(detalleCuenta.IdDetallesCuenta, detalleCuenta.Pago, detalleCuenta.FechaIngreso);
                    var pagosActualizados = await _detallesCuentaRep.ConsultarCuentaDetalles(detalleCuenta.IdCuentaPagar);

                    decimal PagadoActualizado = pagosActualizados.Sum(x => x.Pago);
                    decimal PorPagarActualizado = cuentaPagar.TotalPagado - PagadoActualizado;

                    if (PorPagarActualizado == 0)
                    {
                        await _cuentasPagarRep.ActualizarCuentaPagar(cuentaPagar.IdCuentaPagar, cuentaPagar.FechaCreacion, cuentaPagar.FechaVencimiento, cuentaPagar.TotalPagado, cuentaPagar.IdFacturaCompra, cuentaPagar.IdProveedor, false);
                    }

                    return Json(new { success = true, message = "Pago actualizado correctamente." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: DetalleCuentas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCuenta = await _detallesCuentaRep.ConsultarDetallesCuentas(id);
            if (detalleCuenta == null)
            {
                return NotFound();
            }

            return View(detalleCuenta);
        }

        // POST: DetalleCuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var detalleCuenta = await _detallesCuentaRep.ConsultarDetallesCuentas(id);
                var cuentaPagar = await _cuentasPagarRep.ConsultarCuentasPagar(detalleCuenta.IdCuentaPagar);

                if (cuentaPagar.Estado)
                {
                    await _detallesCuentaRep.EliminarDetallesCuenta(id);
                    return Json(new { success = true, message = "Impuesto eliminado correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se puede eliminar el pago, la cuenta ya fue pagada." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el pago." });
            }
        }

        public async Task<IActionResult> DescargarCuenta(string id)
        {
            var negocio = await _negociosRep.MostrarNegocio();
            var cuentaPagar = await _cuentasPagarRep.ConsultarCuentasPagar(id);
            var detallesCuenta = await _detallesCuentaRep.ConsultarCuentaDetalles(id);

            decimal Pagado = detallesCuenta.Sum(x => x.Pago);
            decimal PorPagar = cuentaPagar.TotalPagado - Pagado;

            var viewModel = new CuentaPagarPDF
            {
                CuentaPagar = cuentaPagar,
                DetallesC = detallesCuenta,
                PorPagar = PorPagar,
                Pagado = Pagado,
                Negocio = negocio
            };

            return new ViewAsPdf("DescargarCuenta", viewModel)
            {
                FileName = $"CuentaPorPagar_{id}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 12, 12, 12)
            };
        }
    }
}
