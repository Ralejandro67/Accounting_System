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
    public class RegistroBancariosController : Controller
    {
        private readonly IRegistrosBancariosRep _registrosBancariosRep;
        private readonly INegociosRep _negociosRep;

        public RegistroBancariosController(IRegistrosBancariosRep context, INegociosRep negociosRep)
        {
            _registrosBancariosRep = context;
            _negociosRep = negociosRep;
        }

        // GET: RegistroBancarios
        public async Task<IActionResult> Index()
        {
            var registroBancario = await _registrosBancariosRep.MostrarRegistrosBancarios();
            return View(registroBancario);
        }

        // GET: RegistroBancarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroBancario = await _registrosBancariosRep.ConsultarRegistrosBancarios(id);    
            if (registroBancario == null)
            {
                return NotFound();
            }

            return View(registroBancario);
        }

        // GET: RegistroBancarios/Create
        public async Task<IActionResult> Create()
        {
            var negocios = await _negociosRep.MostrarNegocio();
            ViewBag.Negocio = new SelectList(negocios, "CedulaJuridica", "NombreEmpresa");
            return PartialView("_newEstadoBPartial", new RegistroBancario());
        }

        // POST: RegistroBancarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistroBancario,FechaRegistro,SaldoInicial,NumeroCuenta,Observaciones,CedulaJuridica,ArchivoEstado")] RegistroBancario registroBancario, IFormFile ArchivoEstado)
        {
            if (ModelState.IsValid)
            {
                if (ArchivoEstado != null && ArchivoEstado.Length > 0)
                {
                    try
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ArchivoEstado.CopyToAsync(memoryStream);
                            registroBancario.EstadoBancario = memoryStream.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ArchivoEstado", $"Ocurrió un problema al cargar el archivo: {ex.Message}");
                    }
                }
                else
                {
                    ModelState.AddModelError("ArchivoEstado", "Es necesario proporcionar un archivo del Estado Bancario.");
                }

                if (!ModelState.Values.Any(v => v.Errors.Count > 0))
                {
                    await _registrosBancariosRep.CrearRegistroBancario(registroBancario.EstadoBancario, registroBancario.FechaRegistro, registroBancario.SaldoInicial, registroBancario.NumeroCuenta, registroBancario.Observaciones, registroBancario.CedulaJuridica);
                    return Json(new { success = true, message = "Estado Bancario agregado correctamente." });
                }
            }

            var errorList = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList();
            return Json(new { success = false, message = "Error al agregar el estado bancario.", errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)) });
        }

        // GET: RegistroBancarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroBancario = await _registrosBancariosRep.ConsultarRegistrosBancarios(id);
            if (registroBancario == null)
            {
                return NotFound();
            }
            return View(registroBancario);
        }

        // POST: RegistroBancarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdRegistroBancario,EstadoBancario,FechaRegistro,SaldoInicial,NumeroCuenta,Observaciones,CedulaJuridica,ArchivoEstado")] RegistroBancario registroBancario)
        {
            if (id != registroBancario.IdRegistro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _registrosBancariosRep.ActualizarRegistroBancario(registroBancario.IdRegistro, registroBancario.EstadoBancario, registroBancario.FechaRegistro, registroBancario.SaldoInicial, registroBancario.NumeroCuenta, registroBancario.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(registroBancario);
        }

        // GET: RegistroBancarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroBancario = await _registrosBancariosRep.ConsultarRegistrosBancarios(id);
            if (registroBancario == null)
            {
                return NotFound();
            }

            return View(registroBancario);
        }

        // POST: RegistroBancarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _registrosBancariosRep.EliminarRegistroBancario(id);
            return RedirectToAction(nameof(Index));
        }

        // Descargar archivo
        public async Task<IActionResult> DescargaEstadoBac(string id)
        {
            var registroBancario = await _registrosBancariosRep.ConsultarRegistrosBancarios(id);
            if (registroBancario != null && registroBancario.EstadoBancario != null)
            {
                return File(registroBancario.EstadoBancario, "application/pdf", "EstadoBancario.pdf");
            }

            return NotFound();
        }
    }
}
