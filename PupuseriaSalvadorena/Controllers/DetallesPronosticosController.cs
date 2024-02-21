using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Services;
using PupuseriaSalvadorena.Repositorios.Interfaces;
namespace PupuseriaSalvadorena.Controllers
{
    public class DetallesPronosticosController : Controller
    {
        private readonly IDetallesPronosticoRep _detallesPronosticoRep;
        private readonly IPronosticoRep _pronosticoRep;

        public DetallesPronosticosController(IDetallesPronosticoRep detallesPronosticoRep, IPronosticoRep pronosticoRep)
        {
            _detallesPronosticoRep = detallesPronosticoRep;
            _pronosticoRep = pronosticoRep;
        }

        // GET: DetallesPronosticos
        public async Task<IActionResult> Index()
        {
            var detallesPronostico = await _detallesPronosticoRep.MostrarDetallesPronosticos();
            return View(detallesPronostico);
        }

        // GET: DetallesPronosticos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallesPronostico = await _detallesPronosticoRep.ConsultarDetallesPronosticos(id.Value);
            if (detallesPronostico == null)
            {
                return NotFound();
            }

            return View(detallesPronostico);
        }

        // GET: DetallesPronosticos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DetallesPronosticos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetallePronostico,IdPronostico,FechaPronostico,PCantVenta,PValorVenta")] DetallesPronostico detallesPronostico)
        {
            if (ModelState.IsValid)
            {
                await _detallesPronosticoRep.CrearDetallesPronostico(detallesPronostico.IdPronostico, detallesPronostico.FechaPronostico, detallesPronostico.PCantVenta, detallesPronostico.PValorVenta);
                return RedirectToAction(nameof(Index));
            }
            return View(detallesPronostico);
        }

        // GET: DetallesPronosticos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallesPronostico = await _detallesPronosticoRep.ConsultarDetallesPronosticos(id.Value);
            if (detallesPronostico == null)
            {
                return NotFound();
            }

            return View(detallesPronostico);
        }

        // POST: DetallesPronosticos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _detallesPronosticoRep.EliminarDetallesPronostico(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
