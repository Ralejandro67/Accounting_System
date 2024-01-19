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

        public RegistroBancariosController(IRegistrosBancariosRep context)
        {
            _registrosBancariosRep = context;
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistroBancarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistroBancario,EstadoBancario,FechaRegistro,NumeroCuenta,Observaciones,CedulaJuridica")] RegistroBancario registroBancario)
        {
            if (ModelState.IsValid)
            {
                await _registrosBancariosRep.CrearRegistroBancario(registroBancario.EstadoBancario, registroBancario.FechaRegistro, registroBancario.NumeroCuenta, registroBancario.Observaciones, registroBancario.CedulaJuridica);
                return RedirectToAction(nameof(Index));
            }
            return View(registroBancario);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdRegistroBancario,EstadoBancario,FechaRegistro,NumeroCuenta,Observaciones,CedulaJuridica")] RegistroBancario registroBancario)
        {
            if (id != registroBancario.IdRegistroBancario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _registrosBancariosRep.ActualizarRegistroBancario(registroBancario.IdRegistroBancario, registroBancario.EstadoBancario, registroBancario.FechaRegistro, registroBancario.NumeroCuenta, registroBancario.Observaciones);
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
    }
}
