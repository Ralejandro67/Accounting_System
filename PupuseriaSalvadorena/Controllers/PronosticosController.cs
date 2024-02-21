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
using PupuseriaSalvadorena.ViewModels;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using Newtonsoft.Json;

namespace PupuseriaSalvadorena.Controllers
{
    public class PronosticosController : Controller
    {
        private readonly IPronosticoRep _pronosticoRep;
        private readonly IHistorialVentaRep _historialVentaRep;
        private readonly IPlatilloRep _platilloRep;
        private readonly IDetallesPronosticoRep _detallesPronosticoRep;
        private readonly ServicioPronosticos _servicioPronosticos;

        public PronosticosController(IPronosticoRep pronosticoRep, IHistorialVentaRep historialVentaRep, IPlatilloRep platilloRep, IDetallesPronosticoRep detallesPronosticoRep, ServicioPronosticos servicioPronosticos)
        {
            _pronosticoRep = pronosticoRep;
            _historialVentaRep = historialVentaRep;
            _platilloRep = platilloRep;
            _detallesPronosticoRep = detallesPronosticoRep;
            _servicioPronosticos = servicioPronosticos;
        }

        // GET: Pronosticos
        public async Task<IActionResult> Index()
        {
            var pronosticos = await _pronosticoRep.MostrarPronosticos();
            return View(pronosticos);
        }

        // GET: Pronosticos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "No se encontro el pronostico solicitado." });
            }

            var pronostico = await _pronosticoRep.ConsultarPronosticos(id.Value);
            var detallesPronostico = await _detallesPronosticoRep.ConsultarDetallesPorPronosticos(id.Value);

            var detalles = detallesPronostico.Select(d => new {
                IdDetallePronostico = d.IdDetallePronostico,
                IdPronostico = d.IdPronostico,
                PCantVenta = d.PCantVenta,
                PValorVenta = d.PValorVenta,
                FechaPronostico = d.FechaPronostico.ToString("dd-MM-yyyy")
            }).ToList();

            var JsonDetallesPronostico = JsonConvert.SerializeObject(detalles);
            var cantPronosticos = 0;

            if (pronostico != null)
            {
                cantPronosticos = detallesPronostico.Count();
            }

            ViewBag.JsonDetallesPronostico = JsonDetallesPronostico;
            ViewBag.CantPronosticos = cantPronosticos;

            if (pronostico == null)
            {
                return Json(new { success = false, message = "No se encontro el pronostico solicitado." });
            }

            var detallesPronosticos = new DetallesPronosticos
            {
                Pronostico = pronostico,
                DetallesP = detallesPronostico
            };

            return View(detallesPronosticos);
        }

        // GET: Pronosticos/Create
        public async Task<IActionResult> Create()
        {
            var platillos = await _platilloRep.MostrarPlatillos();
            ViewBag.IdPlatillos = new SelectList(platillos, "IdPlatillo", "NombrePlatillo");

            return PartialView("_newPronosticoPartial", new Pronostico());
        }

        // POST: Pronosticos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPronostico,IdPlatillo,FechaInicio,FechaFinal,CantTotalProd,TotalVentas")] Pronostico pronostico)
        {
            if (ModelState.IsValid)
            {
                var historialVentas = (await _historialVentaRep.ConsultarHistorialVentasPlatillos(pronostico.IdPlatillo)).ToArray();
                var pronosticos = _servicioPronosticos.CalcularPronosticoHoltWinters(historialVentas);
                var platillo = await _platilloRep.ConsultarPlatillos(pronostico.IdPlatillo);

                DateTime fechaNext = pronostico.FechaInicio.Date.AddDays(1);
                decimal valorDiario;
                decimal totalVentas = 0;
                int cantTotal = 0;

                var idPronostico = await _pronosticoRep.CrearPronostico(pronostico.IdPlatillo, pronostico.FechaInicio, pronostico.FechaFinal, pronostico.CantTotalProd, pronostico.TotalVentas);

                foreach (var pronosticoDiario in pronosticos)
                {
                    if (fechaNext < pronostico.FechaFinal)
                    {
                        valorDiario = pronosticoDiario.CantVenta * platillo.PrecioVenta;
                        cantTotal += pronosticoDiario.CantVenta;
                        totalVentas += valorDiario;
                        await _detallesPronosticoRep.CrearDetallesPronostico(idPronostico, fechaNext, pronosticoDiario.CantVenta, valorDiario);
                        fechaNext = fechaNext.AddDays(1);
                    }
                    else
                    {
                        break;
                    }
                }

                await _pronosticoRep.ActualizarPronosticos(idPronostico, pronostico.IdPlatillo, pronostico.FechaInicio, pronostico.FechaFinal, cantTotal, totalVentas);
                return Json(new { success = true, message = "El pronostico fue creado correctamente." });
            }
            return Json(new { success = false, message = "Error al crear el pronostico." });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPronostico,IdPlatillo,FechaInicio,FechaFinal,CantTotalProd,TotalVentas")] Pronostico pronostico)
        {
            if (id != pronostico.IdPronostico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _pronosticoRep.ActualizarPronosticos(pronostico.IdPronostico, pronostico.IdPlatillo, pronostico.FechaInicio, pronostico.FechaFinal, pronostico.CantTotalProd, pronostico.TotalVentas);
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
