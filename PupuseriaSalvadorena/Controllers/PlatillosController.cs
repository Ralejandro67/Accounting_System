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
    public class PlatillosController : Controller
    {
        private readonly IPlatilloRep _platilloRep;

        public PlatillosController(IPlatilloRep context)
        {
            _platilloRep = context;
        }

        // GET: Platillos
        public async Task<IActionResult> Index()
        {
            var platillos = await _platilloRep.MostrarPlatillos();
            return View(platillos);
        }

        // GET: Platillos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platillo = await _platilloRep.ConsultarPlatillos(id);
            if (platillo == null)
            {
                return NotFound();
            }

            return View(platillo);
        }

        // GET: Platillos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Platillos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlatillo,NombrePlatillo,CostoProduccion,PrecioVenta")] Platillo platillo)
        {
            if (ModelState.IsValid)
            {
                await _platilloRep.CrearPlatillo(platillo.NombrePlatillo, platillo.CostoProduccion, platillo.PrecioVenta);
                return RedirectToAction(nameof(Index));
            }
            return View(platillo);
        }

        // GET: Platillos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platillo = await _platilloRep.ConsultarPlatillos(id);
            if (platillo == null)
            {
                return NotFound();
            }
            return View(platillo);
        }

        // POST: Platillos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdPlatillo,NombrePlatillo,CostoProduccion,PrecioVenta")] Platillo platillo)
        {
            if (id != platillo.IdPlatillo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _platilloRep.ActualizarPlatillo(platillo.IdPlatillo, platillo.NombrePlatillo, platillo.CostoProduccion, platillo.PrecioVenta);
                return RedirectToAction(nameof(Index));
            }
            return View(platillo);
        }

        // GET: Platillos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platillo = await _platilloRep.ConsultarPlatillos(id);
            if (platillo == null)
            {
                return NotFound();
            }

            return View(platillo);
        }

        // POST: Platillos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _platilloRep.EliminarPlatillo(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
