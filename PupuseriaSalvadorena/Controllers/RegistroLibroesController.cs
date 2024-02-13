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
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class RegistroLibroesController : Controller
    {
        private readonly IRegistroLibrosRep _registroLibroRep;   

        public RegistroLibroesController(IRegistroLibrosRep context)
        {
            _registroLibroRep = context;
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
            if (registroLibro == null)
            {
                return NotFound();
            }

            return View(registroLibro);
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
            string Libro = "Libro " + Mes;

            _registroLibroRep.CrearRegistroLibros(FechaResgistro, 0, Libro, false);
        }
    }
}
