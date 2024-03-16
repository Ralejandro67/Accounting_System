using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Filtros;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class HistorialComprasController : Controller
    {
        private readonly IHistorialCompraRep _historialCompraRep;
        private readonly IMateriaPrimaRep _materiaPrimaRep;
        private readonly IFacturaCompraRep _facturaCompraRep;
        private readonly ICuentaPagarRep _cuentaPagarRep;
        private readonly IDetallesCuentaRep _detallesCuentaRep;

        public HistorialComprasController(IHistorialCompraRep context, IMateriaPrimaRep materiaPrimaRep, IFacturaCompraRep facturaCompraRep, ICuentaPagarRep cuentaPagarRep, IDetallesCuentaRep detallesCuentaRep)
        {
            _historialCompraRep = context;
            _materiaPrimaRep = materiaPrimaRep;
            _facturaCompraRep = facturaCompraRep;
            _cuentaPagarRep = cuentaPagarRep;
            _detallesCuentaRep = detallesCuentaRep;
        }

        // GET: HistorialCompras
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var historialCompras = await _historialCompraRep.MostrarHistorialCompras();
            return View(historialCompras);
        }

        // GET: HistorialCompras/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
            if (historialCompra == null)
            {
                return NotFound();
            }

            return View(historialCompra);
        }

        // GET: HistorialCompras/Create
        public async Task<IActionResult> Create()
        {
            var materiasPrimas = await _materiaPrimaRep.MostrarMateriaPrima();
            var facturasCompras = await _facturaCompraRep.MostrarFacturasCompras();

            ViewBag.MateriasPrimas = new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima");
            ViewBag.FacturasCompras = new SelectList(facturasCompras, "IdFacturaCompra", "IdFacturaCompra");
            return PartialView("_newHistorialCPartial", new HistorialCompra());
        }

        // POST: HistorialCompras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdMateriaPrima,CantCompra,Precio,Peso,FechaCompra,IdFacturaCompra")] HistorialCompra historialCompra)
        {
            if (ModelState.IsValid)
            {
                await _historialCompraRep.CrearHistorialCompra(historialCompra.IdMateriaPrima, historialCompra.CantCompra, historialCompra.Precio, historialCompra.Peso, historialCompra.FechaCompra, historialCompra.IdFacturaCompra);
                return Json(new { success = true, message = "Compra agregada correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la compra." });
        }

        // GET: HistorialCompras/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);

            var materiasPrimas = await _materiaPrimaRep.MostrarMateriaPrima();
            var facturasCompras = await _facturaCompraRep.MostrarFacturasCompras();

            ViewBag.MateriasPrimas = new SelectList(materiasPrimas, "IdMateriaPrima", "NombreMateriaPrima");
            ViewBag.FacturasCompras = new SelectList(facturasCompras, "IdFacturaCompra", "IdFacturaCompra");

            if (historialCompra == null)
            {
                return NotFound();
            }
            return PartialView("_editHistorialCPartial", historialCompra);
        }

        // POST: HistorialCompras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdCompra,IdMateriaPrima,CantCompra,Precio,Peso,FechaCompra,IdFacturaCompra")] HistorialCompra historialCompra)
        {
            if (id != historialCompra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _historialCompraRep.ActualizarHistorialCompra(historialCompra.IdCompra, historialCompra.IdMateriaPrima, historialCompra.CantCompra, historialCompra.Precio, historialCompra.Peso, historialCompra.FechaCompra, historialCompra.IdFacturaCompra);
                return Json(new { success = true, message = "Compra actualizada correctamente." });
            }
            return Json(new { success = false, message = "Error al actualizar la compra." });
        }

        // GET: HistorialCompras/Delete/5
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
            var cuentapagar = await _cuentaPagarRep.ConsultarCuentasPagarporFactura(historialCompra.IdFacturaCompra);

            if (cuentapagar != null && cuentapagar.TotalPagado != 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // POST: HistorialCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var historialCompra = await _historialCompraRep.ConsultarHistorialCompras(id);
                var facturaCompra = await _facturaCompraRep.ConsultarFacturasCompras(historialCompra.IdFacturaCompra);
                var cuentapagar = await _cuentaPagarRep.ConsultarCuentasPagarporFactura(historialCompra.IdFacturaCompra);

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

                await _historialCompraRep.EliminarHistorialCompraFactura(historialCompra.IdFacturaCompra);
                await _facturaCompraRep.EliminarFacturaCompra(historialCompra.IdFacturaCompra);
                return Json(new { success = true, message = "Compra eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la compra." });
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
                return Json(new { success = false, message = "No se encontro la factura de compra." });
            }
        }
    }
}
