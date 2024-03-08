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

namespace PupuseriaSalvadorena.Controllers
{
    public class TipoPagosController : Controller
    {
        private readonly ITipoPagoRep _tipoPagoRep; 

        public TipoPagosController(ITipoPagoRep context)
        {
            _tipoPagoRep = context;
        }

        // GET: TipoPagos
        public async Task<IActionResult> Index()
        {
            var tipoPagos = await _tipoPagoRep.MostrarTipoPagos();
            return View(tipoPagos); 
        }

        // GET: TipoPagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPago = await _tipoPagoRep.ConsultarTipoPagos(id.Value);
            if (tipoPago == null)
            {
                return NotFound();
            }

            return View(tipoPago);
        }

        // GET: TipoPagos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoPagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoPago,NombrePago,Estado")] TipoPago tipoPago)
        {
            if (ModelState.IsValid)
            {
                await _tipoPagoRep.CrearTipoPago(tipoPago.NombrePago, tipoPago.Estado);
                return Json(new { success = true, message = "Tipo de factura agregado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: TipoPagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPago = await _tipoPagoRep.ConsultarTipoPagos(id.Value);
            if (tipoPago == null)
            {
                return NotFound();
            }
            return PartialView("_editTipoPPartial", tipoPago);
        }

        // POST: TipoPagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoPago,NombrePago,Estado")] TipoPago tipoPago)
        {
            if (ModelState.IsValid)
            {
                await _tipoPagoRep.ActualizarTipoPago(tipoPago.IdTipoPago, tipoPago.NombrePago, tipoPago.Estado);
                return Json(new { success = true, message = "Tipo de pago actualizado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: TipoPagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var tipoPago = await _tipoPagoRep.ConsultarTipoPagos(id.Value);
            return Json(new { exists = tipoPago != null });
        }

        // POST: TipoPagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tipoPagoRep.EliminarTipoPago(id);
                return Json(new { success = true, message = "Tipo de pago eliminado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al eliminar el tipo de pago, hay facturas asociadas a el tipo de pago." });
            }
        }
    }
}
