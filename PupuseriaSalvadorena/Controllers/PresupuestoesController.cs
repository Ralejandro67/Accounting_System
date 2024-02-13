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

namespace PupuseriaSalvadorena.Controllers
{
    public class PresupuestoesController : Controller
    {
        private readonly IPresupuestoRep _presupuestoRep;
        private readonly ICatPresupuestoRep _categoriaPresupuestoRep;
        private readonly IRegistroLibrosRep _registroLibrosRep;
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly IDetallesPresupuestoRep _detallesPresupuestoRep;

        public PresupuestoesController(IPresupuestoRep context, ICatPresupuestoRep categoriaPresupuestoRep, IRegistroLibrosRep registroLibrosRep, IDetallesTransacRep detallesTransacRep, IDetallesPresupuestoRep detallesPresupuestoRep)
        {
            _presupuestoRep = context;
            _categoriaPresupuestoRep = categoriaPresupuestoRep;
            _registroLibrosRep = registroLibrosRep;
            _detallesTransacRep = detallesTransacRep;
            _detallesPresupuestoRep = detallesPresupuestoRep;
        }

        // GET: Presupuestoes
        public async Task<IActionResult> Index()
        {
            var presupuestos = await _presupuestoRep.MostrarPresupuestos();
            return View(presupuestos); 
        }

        // GET: Presupuestoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(id);
            var transaccionesPresupuesto = await _detallesPresupuestoRep.ConsultarTransacPresupuestos(id);
            var transacciones = await _detallesTransacRep.MostrarDetallesTransacciones();
            var totaltransacciones = 0;

            if (transaccionesPresupuesto != null)
            {
                totaltransacciones = transaccionesPresupuesto.Count();
            }

            decimal saldoRestante = presupuesto.SaldoPresupuesto - presupuesto.SaldoUsado;

            ViewBag.SaldoUsado = presupuesto.SaldoUsado;
            ViewBag.SaldoRestante = saldoRestante;
            ViewBag.TotalTransacciones = totaltransacciones;

            if (presupuesto == null)
            {
                return NotFound();
            }

            var viewModel = new DetallesPresupuestos
            {
                Presupuesto = presupuesto,
                DetallesTransacciones = transacciones
            };

            return View(viewModel);
        }

        // GET: Presupuestoes/Create
        public async Task<IActionResult> Create()
        {
            var categorias = await _categoriaPresupuestoRep.MostrarCatPresupuestos();
            var libros = await _registroLibrosRep.MostrarRegistrosLibros();
            var presupuesto = await _presupuestoRep.MostrarPresupuestos();
            decimal saldolibros = 0;
            decimal saldoPresupuesto = 0;

            if (presupuesto != null)
            {
                foreach (var item in presupuesto)
                {
                    saldoPresupuesto += item.SaldoPresupuesto;
                }
            }

            foreach (var item in libros)
            {
                saldolibros += item.MontoTotal;
            }

            saldoPresupuesto = saldolibros - saldoPresupuesto;

            ViewBag.SaldoPresupuestos = saldoPresupuesto;
            ViewBag.SaldoLibros = saldolibros;
            ViewBag.CategoriasP = new SelectList(categorias, "IdCategoriaP", "Nombre");

            return PartialView("_newPresupuestoPartial", new Presupuesto());
        }

        // POST: Presupuestoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPresupuesto,NombreP,FechaInicio,FechaFin,Descripcion,SaldoUsado,SaldoPresupuesto,Estado,IdCategoriaP")] Presupuesto presupuesto)
        {
            if (ModelState.IsValid)
            {
                await _presupuestoRep.CrearPresupuesto(presupuesto.NombreP, presupuesto.FechaInicio, presupuesto.FechaFin, presupuesto.Descripcion, presupuesto.SaldoUsado, presupuesto.SaldoPresupuesto, presupuesto.Estado, presupuesto.IdCategoriaP);
                return Json(new { success = true, message = "Presupuesto creado correctamente." });
            }
            return Json(new { success = false, message = "Error al crear el presupuesto." });
        }

        // GET: Presupuestoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(id);
            var categorias = await _categoriaPresupuestoRep.MostrarCatPresupuestos();
            ViewBag.CategoriasP = new SelectList(categorias, "IdCategoriaP", "Nombre");

            if (presupuesto == null)
            {
                return NotFound();
            }
            return PartialView("_editPresupuestoPartial", presupuesto);
        }

        // POST: Presupuestoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdPresupuesto,NombreP,FechaInicio,FechaFin,Descripcion,SaldoUsado,SaldoPresupuesto,Estado,IdCategoriaP")] Presupuesto presupuesto)
        {
            if (id != presupuesto.IdPresupuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _presupuestoRep.ActualizarPresupuesto(presupuesto.IdPresupuesto, presupuesto.NombreP, presupuesto.FechaInicio, presupuesto.FechaFin, presupuesto.Descripcion, presupuesto.SaldoUsado, presupuesto.SaldoPresupuesto, presupuesto.Estado, presupuesto.IdCategoriaP);
                return Json(new { success = true, message = "Presupuesto actualizado correctamente." });
            }
            return Json(new { success = false, message = "Error al actualizar el presupuesto." });
        }

        // GET: Presupuestoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var presupuesto = await _presupuestoRep.ConsultarPresupuestos(id);
            return Json(new { exists = presupuesto != null });
        }

        // POST: Presupuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _presupuestoRep.EliminarPresupuesto(id);
                return Json(new { success = true, message = "Presupuesto eliminado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al eliminar el presupuesto." });
            }
        }
    }
}
