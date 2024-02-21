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

namespace PupuseriaSalvadorena.Controllers
{
    public class RegistroLibroesController : Controller
    {
        private readonly IRegistroLibrosRep _registroLibroRep;   
        private readonly IDetallesTransacRep _detallesTransacRep;

        public RegistroLibroesController(IRegistroLibrosRep context, IDetallesTransacRep detallesTransacRep)
        {
            _registroLibroRep = context;
            _detallesTransacRep = detallesTransacRep;
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
            return Json(new { success = false, message = "Error al agregar el libro." });
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
            return Json(new { success = false, message = "Datos inválidos." });
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
