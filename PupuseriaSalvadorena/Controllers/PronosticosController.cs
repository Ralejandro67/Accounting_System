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
    public class PronosticosController : Controller
    {
        private readonly IPronosticoRep _pronosticoRep;
        private readonly IHistorialVentaRep _historialVentaRep;
        private readonly ServicioPronosticos _servicioPronosticos;

        public PronosticosController(IPronosticoRep pronosticoRep, IHistorialVentaRep historialVentaRep, ServicioPronosticos servicioPronosticos)
        {
            _pronosticoRep = pronosticoRep;
            _historialVentaRep = historialVentaRep;
            _servicioPronosticos = servicioPronosticos;
        }

        // GET: Pronosticos
        public async Task<IActionResult> Index()
        {
            var pronosticos = await _pronosticoRep.MostrarPronostico();
            return View(pronosticos);
        }

        // GET: Pronosticos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pronostico = await _pronosticoRep.ConsultarPronosticos(id.Value);
            if (pronostico == null)
            {
                return NotFound();
            }

            return View(pronostico);
        }

        // GET: Pronosticos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pronosticos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPronostico,IdPlatillo,FechaInicio,FechaFinal,CantTotalProd,TotalVentas,PronosticoDoc")] Pronostico pronostico)
        {
            if (ModelState.IsValid)
            {
                var historialVentas = (await _historialVentaRep.ConsultarHistorialVentasPlatillos(pronostico.IdPlatillo)).ToArray();

                var descomposicion = _servicioPronosticos.DescomposicionMutiplicativa(historialVentas);

                return RedirectToAction(nameof(Index));
            }
            return View(pronostico);
        }

        // GET: Pronosticos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pronostico = await _pronosticoRep.ConsultarPronosticos(id.Value);
            if (pronostico == null)
            {
                return NotFound();
            }
            return View(pronostico);
        }

        // POST: Pronosticos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPronostico,IdPlatillo,FechaInicio,FechaFinal,CantTotalProd,TotalVentas,PronosticoDoc")] Pronostico pronostico)
        {
            if (id != pronostico.IdPronostico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _pronosticoRep.ActualizarPronosticos(pronostico.IdPronostico, pronostico.IdPlatillo, pronostico.FechaInicio, pronostico.FechaFinal, pronostico.CantTotalProd, pronostico.TotalVentas, pronostico.PronosticoDoc);
                return RedirectToAction(nameof(Index));
            }
            return View(pronostico);
        }

        // GET: Pronosticos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pronostico = await _pronosticoRep.ConsultarPronosticos(id.Value);
            if (pronostico == null)
            {
                return NotFound();
            }

            return View(pronostico);
        }

        // POST: Pronosticos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pronosticoRep.EliminarPronostico(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
