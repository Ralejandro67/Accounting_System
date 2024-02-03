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
using System.Text.RegularExpressions;
using System.Globalization;

namespace PupuseriaSalvadorena.Controllers
{
    public class ConciliacionBancariasController : Controller
    {
        private readonly IConciliacionRep _conciliacionRep;
        private readonly IRegistrosBancariosRep _registrosBancariosRep;
        private readonly IRegistroLibrosRep _registrosLibrosRep;
        private readonly IDetallesTransacRep _detallesTransacRep;

        public ConciliacionBancariasController(IConciliacionRep context, IRegistrosBancariosRep registrosBancariosRep, IRegistroLibrosRep registrosLibrosRep, IDetallesTransacRep detallesTransacRep)
        {
            _conciliacionRep = context;
            _registrosBancariosRep = registrosBancariosRep;
            _registrosLibrosRep = registrosLibrosRep;
            _detallesTransacRep = detallesTransacRep;
        }

        // GET: ConciliacionBancarias
        public async Task<IActionResult> Index()
        {
            var conciliacionBancarias = await _conciliacionRep.MostrarConciliacionesBancarias();
            return View(conciliacionBancarias);
        }

        // GET: ConciliacionBancarias/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }

            return View(conciliacionBancaria);
        }

        // GET: ConciliacionBancarias/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var registrosBancarios = await _registrosBancariosRep.MostrarRegistrosBancarios();
            var registrosLibros = await _registrosLibrosRep.MostrarRegistrosLibros();

            ViewBag.RegistrosBancarios = new SelectList(registrosBancarios, "IdRegistro", "IdRegistro");
            ViewBag.RegistrosLibros = new SelectList(registrosLibros, "IdRegistroLibros", "IdRegistroLibros");
            return PartialView("_newConciliacionPartial", new ConciliacionBancaria());
        }

        // POST: ConciliacionBancarias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConciliacion,FechaConciliacion,SaldoBancario,SaldoLibro,Diferencia,Observaciones,IdRegistro,IdRegistroLibros")] ConciliacionBancaria conciliacionBancaria)
        {
            if (ModelState.IsValid)
            {
                var registrosBancarios = await _registrosBancariosRep.ConsultarRegistrosBancarios(conciliacionBancaria.IdRegistro);
                var registrosLibros = await _registrosLibrosRep.ConsultarRegistrosLibros(conciliacionBancaria.IdRegistroLibros);
                var detallesTransac = await _detallesTransacRep.ConsultarTransacciones(conciliacionBancaria.IdRegistroLibros);

                decimal SaldoConciliacion = conciliacionBancaria.SaldoBancario - registrosBancarios.SaldoInicial;
                decimal SaldoLibro = 0;
                foreach (var item in detallesTransac)
                {
                    if (item.IdMovimiento == 1)
                    {
                        SaldoLibro += item.Monto;
                    }
                    else
                    {
                        SaldoLibro -= item.Monto;
                    }
                }

                var diferencia = SaldoConciliacion - SaldoLibro;
                var Conciliacion = new ConciliacionBancaria
                {
                    FechaConciliacion = DateTime.Now,
                    SaldoBancario = conciliacionBancaria.SaldoBancario,
                    SaldoLibro = SaldoLibro,
                    Diferencia = diferencia,
                    Observaciones = "Prueba",
                    IdRegistro = conciliacionBancaria.IdRegistro,
                    IdRegistroLibros = conciliacionBancaria.IdRegistroLibros
                };

                await _conciliacionRep.CrearConciliacion(Conciliacion.FechaConciliacion, Conciliacion.SaldoBancario, Conciliacion.SaldoLibro, Conciliacion.Diferencia, Conciliacion.Observaciones, Conciliacion.IdRegistro, Conciliacion.IdRegistroLibros);
                return Json(new { success = true, message = "Conciliacion creada correctamente."});
            }
            return Json(new { success = false, message = "Error al crear la conciliacion." });
        }

        // GET: ConciliacionBancarias/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }
            return View(conciliacionBancaria);
        }

        // POST: ConciliacionBancarias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdConciliacion,FechaConciliacion,SaldoBancario,SaldoLibro,Diferencia,Observaciones,IdRegistro,IdRegistroLibros")] ConciliacionBancaria conciliacionBancaria)
        {
            if (id != conciliacionBancaria.IdConciliacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _conciliacionRep.ActualizarConciliacion(conciliacionBancaria.IdConciliacion, conciliacionBancaria.SaldoBancario, conciliacionBancaria.SaldoLibro, conciliacionBancaria.Diferencia, conciliacionBancaria.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(conciliacionBancaria);
        }

        // GET: ConciliacionBancarias/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            if (conciliacionBancaria == null)
            {
                return NotFound();
            }

            return View(conciliacionBancaria);
        }

        // POST: ConciliacionBancarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _conciliacionRep.EliminarConciliacion(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
