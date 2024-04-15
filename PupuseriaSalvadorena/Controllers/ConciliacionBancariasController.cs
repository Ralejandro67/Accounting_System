using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.ViewModels;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Text.RegularExpressions;
using System.Globalization;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Filtros;

namespace PupuseriaSalvadorena.Controllers
{
    public class ConciliacionBancariasController : Controller
    {
        private readonly IConciliacionRep _conciliacionRep;
        private readonly IRegistrosBancariosRep _registrosBancariosRep;
        private readonly IRegistroLibrosRep _registrosLibrosRep;
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly INegociosRep _negociosRep;

        public ConciliacionBancariasController(IConciliacionRep context, IRegistrosBancariosRep registrosBancariosRep, IRegistroLibrosRep registrosLibrosRep, IDetallesTransacRep detallesTransacRep, INegociosRep negociosRep)
        {
            _conciliacionRep = context;
            _registrosBancariosRep = registrosBancariosRep;
            _registrosLibrosRep = registrosLibrosRep;
            _detallesTransacRep = detallesTransacRep;
            _negociosRep = negociosRep;
        }

        // GET: ConciliacionBancarias
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var conciliacionBancarias = await _conciliacionRep.MostrarConciliacionesBancarias();
            return View(conciliacionBancarias);
        }

        // GET: ConciliacionBancarias/Details/5
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            var transacciones = await _detallesTransacRep.ConsultarTransacciones(conciliacionBancaria.IdRegistroLibros);
            var libro = await _registrosLibrosRep.ConsultarRegistrosLibros(conciliacionBancaria.IdRegistroLibros);

            if (conciliacionBancaria == null)
            {
                return NotFound();
            }

            ViewBag.totalTransacciones = transacciones.Count();

            DetallesConciliacion detallesConciliacion = new DetallesConciliacion
            {
                ConciliacionBancaria = conciliacionBancaria,
                RegistroLibro = libro,
                DetallesTransacciones = transacciones
            };

            return View(detallesConciliacion);
        }

        // GET: ConciliacionBancarias/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var registrosBancarios = await _registrosBancariosRep.MostrarRegistrosBancarios();
            var registrosLibros = await _registrosLibrosRep.MostrarRegistrosLibros();

            var libros = registrosLibros.Where(x => x.Conciliado == false).ToList();

            ViewBag.RegistrosBancarios = new SelectList(registrosBancarios, "IdRegistro", "NumeroCuenta");
            ViewBag.RegistrosLibros = new SelectList(libros, "IdRegistroLibros", "Descripcion");

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

                if(diferencia == 0)
                {
                    await _conciliacionRep.CrearConciliacion(DateTime.Now, SaldoConciliacion, SaldoLibro, diferencia, conciliacionBancaria.Observaciones, conciliacionBancaria.IdRegistro, conciliacionBancaria.IdRegistroLibros);
                    await _registrosBancariosRep.ActualizarRegistroBancario(registrosBancarios.IdRegistro, registrosBancarios.FechaRegistro, conciliacionBancaria.SaldoBancario, registrosBancarios.NumeroCuenta, registrosBancarios.Observaciones);
                    await _detallesTransacRep.ActualizarConciliado(registrosLibros.IdRegistroLibros, true);
                    await _registrosLibrosRep.ActualizarRegistroLibros(registrosLibros.IdRegistroLibros, registrosLibros.MontoTotal, registrosLibros.Descripcion, true);
                    return Json(new { success = true, message = "La conciliacion fue creda con exito." });
                }
                else
                {
                    return Json(new { success = false, message = "La diferencia no es 0, por favor verifica el saldo de la cuenta y el libro contable." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = errors });
            }
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
            try
            {
                var conciliacion = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
                var libro = await _registrosLibrosRep.ConsultarRegistrosLibros(conciliacion.IdRegistroLibros);
                var cuenta = await _registrosBancariosRep.ConsultarRegistrosBancarios(conciliacion.IdRegistro);
                var detallesTransac = await _detallesTransacRep.ConsultarTransacciones(conciliacion.IdRegistroLibros);

                var SaldoBancario = conciliacion.SaldoBancario - libro.MontoTotal;

                await _registrosBancariosRep.ActualizarRegistroBancario(conciliacion.IdRegistro, cuenta.FechaRegistro, SaldoBancario, cuenta.NumeroCuenta, cuenta.Observaciones);
                await _registrosLibrosRep.ActualizarRegistroLibros(conciliacion.IdRegistroLibros, libro.MontoTotal, libro.Descripcion, false);
                await _detallesTransacRep.ActualizarConciliado(conciliacion.IdRegistroLibros, false);

                await _conciliacionRep.EliminarConciliacion(id); 
                return Json(new { success = true, message = "La conciliacion fue eliminada con exito." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el impuesto." });
            }
        }

        // Reporte Pdf
        public async Task<IActionResult> DescargarConciliacion(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _negociosRep.MostrarNegocio();
            var conciliacionBancaria = await _conciliacionRep.ConsultarConciliacionesBancarias(id);
            var cuenta = await _registrosBancariosRep.ConsultarRegistrosBancarios(conciliacionBancaria.IdRegistro);
            var transacciones = await _detallesTransacRep.ConsultarTransacciones(conciliacionBancaria.IdRegistroLibros);
            var libro = await _registrosLibrosRep.ConsultarRegistrosLibros(conciliacionBancaria.IdRegistroLibros);

            var viewModel = new ConciliacionPDF
            {
                Negocio = negocio,
                RegistroLibro = libro,
                RegistroBancario = cuenta,
                ConciliacionBancaria = conciliacionBancaria,
                DetallesTransacciones = transacciones
            };

            return new ViewAsPdf("DescargarConciliacion", viewModel)
            {
                FileName = $"Conciliacion_{conciliacionBancaria.IdRegistroLibros}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 12, 12, 12)
            };
        }
    }
}
