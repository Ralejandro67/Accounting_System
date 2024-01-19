using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class PersonasController : Controller
    {
        private readonly IPersonasRep _personasRep;

        public PersonasController(IPersonasRep context)
        {
            _personasRep = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var personas = await _personasRep.MostrarPersonas();
            return View(personas);
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personasRep.ConsultarPersonas(id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,Cedula,Nombre,Apellido,FechaNac,IdCorreoElectronico,IdDireccion,IdTelefono")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                await _personasRep.CrearPersona(persona.Cedula, persona.Nombre, persona.Apellido, persona.FechaNac, persona.IdCorreoElectronico, persona.IdDireccion, persona.IdTelefono);
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personasRep.ConsultarPersonas(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdPersona,Cedula,Nombre,Apellido,FechaNac,IdCorreoElectronico,IdDireccion,IdTelefono")] Persona persona)
        {
            if (id != persona.IdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _personasRep.ActualizarPersona(persona.IdPersona, persona.Nombre, persona.Apellido);
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personasRep.ConsultarPersonas(id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _personasRep.EliminarPersona(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
