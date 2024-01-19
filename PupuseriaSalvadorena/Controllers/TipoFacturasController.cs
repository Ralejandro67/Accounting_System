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
    public class TipoFacturasController : Controller
    {
        private readonly ITipoFacturaRep _tipoFacturaRep;   

        public TipoFacturasController(ITipoFacturaRep context)
        {
            _tipoFacturaRep = context;
        }

        // GET: TipoFacturas
        public async Task<IActionResult> Index()
        {
            var tipoFacturas = await _tipoFacturaRep.MostrarTipoFacturas();
            return View(tipoFacturas);
        }

        // GET: TipoFacturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoFactura = await _tipoFacturaRep.ConsultarTipoFacturas(id.Value);
            if (tipoFactura == null)
            {
                return NotFound();
            }

            return View(tipoFactura);
        }

        // GET: TipoFacturas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoFacturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoFactura,NombreFactura,Estado")] TipoFactura tipoFactura)
        {
            if (ModelState.IsValid)
            {
                await _tipoFacturaRep.CrearTipoFactura(tipoFactura.NombreFactura, tipoFactura.Estado);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoFactura);
        }

        // GET: TipoFacturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoFactura = await _tipoFacturaRep.ConsultarTipoFacturas(id.Value);
            if (tipoFactura == null)
            {
                return NotFound();
            }
            return View(tipoFactura);
        }

        // POST: TipoFacturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoFactura,NombreFactura,Estado")] TipoFactura tipoFactura)
        {
            if (id != tipoFactura.IdTipoFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tipoFacturaRep.ActualizarTipoFactura(tipoFactura.IdTipoFactura, tipoFactura.NombreFactura, tipoFactura.Estado);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoFactura);
        }

        // GET: TipoFacturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoFactura = await _tipoFacturaRep.ConsultarTipoFacturas(id.Value);
            if (tipoFactura == null)
            {
                return NotFound();
            }

            return View(tipoFactura);
        }

        // POST: TipoFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoFacturaRep.EliminarTipoFactura(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
