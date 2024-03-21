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
using PupuseriaSalvadorena.Filtros;

namespace PupuseriaSalvadorena.Controllers
{
    public class TipoVentasController : Controller
    {
        private readonly ITipoVentaRep _tipoVentaRep;

        public TipoVentasController(ITipoVentaRep context)
        {
            _tipoVentaRep = context;
        }

        // GET: TipoVentas
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador" })]
        public async Task<IActionResult> Index()
        {
            var tipoVentas = await _tipoVentaRep.MostrarTipoVentas();
            return View(tipoVentas);    
        }

        // GET: TipoVentas/Create
        public IActionResult Create()
        {
            return PartialView("_newTipoVPartial", new TipoVenta());
        }

        // POST: TipoVentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoVenta,NombreVenta,Estado")] TipoVenta tipoVenta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _tipoVentaRep.CrearTipoVenta(tipoVenta.NombreVenta, tipoVenta.Estado);
                    return Json(new { success = true, message = "Tipo de venta agregada correctamente." });
                }
                catch
                {
                    return Json(new { success = false, message = $"Ya existe un tipo de venta llamada '{tipoVenta.NombreVenta}'." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: TipoVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Error al editar el tipo de venta." });
            }

            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            if (tipoVenta == null)
            {
                return Json(new { success = false, message = "No se encontro el tipo de venta." });
            }
            return PartialView("_editTipoVPartial", tipoVenta);
        }

        // POST: TipoVentas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoVenta,NombreVenta,Estado")] TipoVenta tipoVenta)
        {
            if (ModelState.IsValid)
            {
                var tipo = await _tipoVentaRep.ConsultarTipoVentas(id);

                if(tipo.NombreVenta != tipoVenta.NombreVenta)
                {
                    var exist = await _tipoVentaRep.MostrarTipoVentas();
                    var existTipo = exist.Where(x => x.NombreVenta == tipoVenta.NombreVenta).ToList();

                    if (existTipo.Count > 0)
                    {
                        return Json(new { success = false, message = $"Ya existe un tipo de venta llamada '{tipoVenta.NombreVenta}'." });
                    }
                }

                await _tipoVentaRep.ActualizarTipoVentas(tipoVenta.IdTipoVenta, tipoVenta.NombreVenta, tipoVenta.Estado);
                return Json(new { success = true, message = "Tipo de venta actualizada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: TipoVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var tipoVenta = await _tipoVentaRep.ConsultarTipoVentas(id.Value);
            return Json(new { exists = tipoVenta != null });
        }

        // POST: TipoVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try 
            {                 
                await _tipoVentaRep.EliminarTipoVenta(id);
                return Json(new { success = true, message = "Tipo de venta eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el tipo de venta, hay facturas asociadas a el tipo de venta." });
            }
        }
    }
}
