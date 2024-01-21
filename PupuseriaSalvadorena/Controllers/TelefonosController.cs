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
    public class TelefonosController : Controller
    {
        private readonly ITelefonosRep _telefonosRep;

        public TelefonosController(ITelefonosRep context)
        {
            _telefonosRep = context;
        }

        // GET: Telefonos
        public async Task<IActionResult> Index()
        {
            var telefonos = await _telefonosRep.MostrarTelefonos();
            return View(telefonos);
        }

        // GET: Telefonos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefonos = await _telefonosRep.ConsultarTelefonos(id.Value);
            if (telefonos == null)
            {
                return NotFound();
            }

            return View(telefonos);
        }

        // GET: Telefonos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Telefonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTelefono,Telefono,Estado")] Telefonos telefonos)
        {
            if (ModelState.IsValid)
            {
                await _telefonosRep.CrearTelefono(telefonos.Telefono, telefonos.Estado);
                return Json(new { success = true, message = "Telefono agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el telefono." });
        }

        // GET: Telefonos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefonos = await _telefonosRep.ConsultarTelefonos(id.Value);
            if (telefonos == null)
            {
                return NotFound();
            }
            return View(telefonos);
        }

        // POST: Telefonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTelefono,Telefono,Estado")] Telefonos telefonos)
        {
            if (id != telefonos.IdTelefono)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _telefonosRep.ActualizarTelefono(telefonos.IdTelefono, telefonos.Telefono, telefonos.Estado);
                return RedirectToAction(nameof(Index));
            }
            return View(telefonos);
        }

        // GET: Telefonos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefonos = await _telefonosRep.ConsultarTelefonos(id.Value);
            if (telefonos == null)
            {
                return NotFound();
            }

            return View(telefonos);
        }

        // POST: Telefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _telefonosRep.EliminarTelefono(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
