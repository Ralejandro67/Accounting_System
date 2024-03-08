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
            return PartialView("_newEstadoBPartial", new RegistroBancario());
        }

        // POST: RegistroBancarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistro,FechaRegistro,SaldoInicial,NumeroCuenta,Observaciones,CedulaJuridica")] RegistroBancario registroBancario)
        {
            if (ModelState.IsValid)
            {
                var cedula = await _negociosRep.ConsultarNegocio();
                await _registrosBancariosRep.CrearRegistroBancario(registroBancario.FechaRegistro, registroBancario.SaldoInicial, registroBancario.NumeroCuenta, registroBancario.Observaciones, cedula);
                return Json(new { success = true, message = "Estado Bancario agregado correctamente."});

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return Json(new { success = false, errors = errors });
            }
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

            return PartialView("_editEstadoBPartial", registroBancario);
        }

        // POST: RegistroBancarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdRegistro,EstadoBancario,FechaRegistro,SaldoInicial,NumeroCuenta,Observaciones,CedulaJuridica")] RegistroBancario registroBancario)
        {
            if (id != registroBancario.IdRegistro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _registrosBancariosRep.ActualizarRegistroBancario(registroBancario.IdRegistro, registroBancario.FechaRegistro, registroBancario.SaldoInicial, registroBancario.NumeroCuenta, registroBancario.Observaciones);
                return Json(new { success = true, message = "Cuenta Bancaria actualizada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: RegistroBancarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var registroBancario = await _registrosBancariosRep.ConsultarRegistrosBancarios(id);
            return Json(new { exists = registroBancario != null });
        }

        // POST: RegistroBancarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _registrosBancariosRep.EliminarRegistroBancario(id);
                return Json(new { success = true, message = "Cuenta bancaria eliminada correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "No se puede eliminar la cuenta bancaria." });
            }
        }
    }
}
