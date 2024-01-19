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
    public class EnvioFacturasController : Controller
    {
        private readonly IEnvioFacturaRep _envioFacturaRep;

        public EnvioFacturasController(IEnvioFacturaRep context)
        {
            _envioFacturaRep = context;
        }

        // GET: EnvioFacturas
        public async Task<IActionResult> Index()
        {
            var envioFacturas = await _envioFacturaRep.MostrarEnvioFactura();
            return View(envioFacturas);
        }

        // GET: EnvioFacturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var envioFactura = await _envioFacturaRep.ConsultarEnvioFacturas(id.Value);
            if (envioFactura == null)
            {
                return NotFound();
            }

            return View(envioFactura);
        }

        // GET: EnvioFacturas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EnvioFacturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEnvioFactura,FechaEnvio,IdFacturaVenta")] EnvioFactura envioFactura)
        {
            if (ModelState.IsValid)
            {
                await _envioFacturaRep.CrearEnvioFactura(envioFactura.FechaEnvio, envioFactura.IdFacturaVenta);
                return RedirectToAction(nameof(Index));
            }
            return View(envioFactura);
        }

        // GET: EnvioFacturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var envioFactura = await _envioFacturaRep.ConsultarEnvioFacturas(id.Value);
            if (envioFactura == null)
            {
                return NotFound();
            }
            return View(envioFactura);
        }

        // POST: EnvioFacturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEnvioFactura,FechaEnvio,IdFacturaVenta")] EnvioFactura envioFactura)
        {
            if (id != envioFactura.IdEnvioFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _envioFacturaRep.ActualizarEnvioFactura(envioFactura.IdEnvioFactura, envioFactura.FechaEnvio, envioFactura.IdFacturaVenta);
                return RedirectToAction(nameof(Index));
            }
            return View(envioFactura);
        }

        // GET: EnvioFacturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var envioFactura = await _envioFacturaRep.ConsultarEnvioFacturas(id.Value);
            if (envioFactura == null)
            {
                return NotFound();
            }

            return View(envioFactura);
        }

        // POST: EnvioFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _envioFacturaRep.EliminarEnvioFactura(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
