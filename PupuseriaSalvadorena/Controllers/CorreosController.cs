using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class CorreosController : Controller
    {
        private readonly ICorreosRep _correosRep;

        public CorreosController(ICorreosRep correosRep)
        {
            _correosRep = correosRep;
        }

        // GET: Correos
        public async Task<IActionResult> Index()
        {
            var correos = await _correosRep.MostrarCorreos();
            return View(correos);
        }

        // GET: Correos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var correoElectronico = await _correosRep.ConsultarCorreos(id.Value);
            if (correoElectronico == null)
            {
                return NotFound();
            }

            return View(correoElectronico);
        }

        // GET: Correos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Correos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCorreoElectronico,Correo")] CorreoElectronico correoElectronico)
        {
            if (ModelState.IsValid)
            {
                await _correosRep.CrearCorreo(correoElectronico.Correo);
                return RedirectToAction(nameof(Index));
            }
            return View(correoElectronico);
        }

        // GET: Correos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var correoElectronico = await _correosRep.ConsultarCorreos(id.Value);
            if (correoElectronico == null)
            {
                return NotFound();
            }
            return View(correoElectronico);
        }

        // POST: Correos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCorreoElectronico,Correo")] CorreoElectronico correoElectronico)
        {
            if (id != correoElectronico.IdCorreoElectronico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _correosRep.ActualizarCorreo(correoElectronico.IdCorreoElectronico, correoElectronico.Correo);
                return RedirectToAction(nameof(Index));
            }
            return View(correoElectronico);
        }

        // GET: Correos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var correoElectronico = await _correosRep.ConsultarCorreos(id.Value);
            if (correoElectronico == null)
            {
                return NotFound();
            }

            return View(correoElectronico);
        }

        // POST: Correos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _correosRep.EliminarCorreo(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
