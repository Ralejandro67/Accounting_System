using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using PupuseriaSalvadorena.ViewModels;

namespace PupuseriaSalvadorena.Controllers
{
    public class DeclaracionImpuestoesController : Controller
    {
        private readonly IDeclaracionTaxRep _declaracionTaxRep;
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly INegociosRep _negociosRep;

        public DeclaracionImpuestoesController(IDeclaracionTaxRep declaracionTaxRep, IDetallesTransacRep detallesTransacRep, INegociosRep negociosRep)
        {
            _declaracionTaxRep = declaracionTaxRep;
            _detallesTransacRep = detallesTransacRep;
            _negociosRep = negociosRep;
        }

        // GET: DeclaracionImpuestoes
        public async Task<IActionResult> Index()
        {
            var declaracionImpuesto = await _declaracionTaxRep.MostrarDeclaracionesImpuestos();
            return View(declaracionImpuesto);   
        }

        // GET: DeclaracionImpuestoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Error al crear la declaracion." });
            }

            var declaracionImpuesto = await _declaracionTaxRep.ConsultarDeclaracionesImpuestos(id);
            var trimestre = declaracionImpuesto.Trimestre.Split(' ');
            var nTrimestre = int.Parse(trimestre[1]);
            var year = int.Parse(trimestre[2]);

            int mesInicio = (nTrimestre - 1) * 3 + 1;
            int mesFinal = nTrimestre * 3;

            var transacciones = (await _detallesTransacRep.MostrarDetallesTransaccionesYear())
                                .Where(t => t.FechaTrans.Year == year && t.FechaTrans.Month >= mesInicio && t.FechaTrans.Month <= mesFinal).ToList();

            if (declaracionImpuesto == null)
            {
                return Json(new { success = false, message = "Error al crear la declaracion." });
            }

            var viewModel = new Declaraciones
            {
                DeclaracionImpuesto = declaracionImpuesto,
                DetallesTransacciones = transacciones
            };

            return View(viewModel);
        }

        // GET: DeclaracionImpuestoes/Create
        public IActionResult Create()
        {
            return PartialView("_newDeclaracionPartial", new Presupuesto());
        }

        // POST: DeclaracionImpuestoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDeclaracionImpuesto,CedulaJuridica,FechaInicio,Trimestre,MontoRenta,MontoIVA,MontoTotalImpuestos,MontoTotal,Observaciones")] DeclaracionImpuesto declaracionImpuesto)
        {
            if (ModelState.IsValid)
            {
                var trimestre = declaracionImpuesto.Trimestre.Split(' ');
                var nTrimestre = int.Parse(trimestre[1]);
                var year = int.Parse(trimestre[2]);

                int mesInicio = (nTrimestre - 1) * 3 + 1;
                int mesFinal = nTrimestre * 3;

                var transacciones = (await _detallesTransacRep.MostrarDetallesTransaccionesYear())
                                    .Where(t => t.FechaTrans.Year == year && t.FechaTrans.Month >= mesInicio && t.FechaTrans.Month <= mesFinal).ToList();

                decimal montoTotal = transacciones.Sum(t => t.Monto);
                decimal montoRenta = montoTotal * 0.02m;
                decimal montoIVA = montoTotal * 0.04m;
                decimal montoTotalImpuestos = montoRenta + montoIVA;

                var negocio = await _negociosRep.ConsultarNegocio();
                var IdDeclaracion = await _declaracionTaxRep.CrearDeclaracionTax(negocio, declaracionImpuesto.FechaInicio, declaracionImpuesto.Trimestre, montoRenta, montoIVA, montoTotalImpuestos, montoTotal, declaracionImpuesto.Observaciones);

                return RedirectToAction(nameof(Details), new { id = IdDeclaracion });
            }
            return Json(new { success = false, message = "Error al crear la declaracion." });
        }

        // GET: DeclaracionImpuestoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var declaracionImpuesto = await _declaracionTaxRep.ConsultarDeclaracionesImpuestos(id);
            if (declaracionImpuesto == null)
            {
                return NotFound();
            }
            return View(declaracionImpuesto);
        }

        // POST: DeclaracionImpuestoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDeclaracionImpuesto,CedulaJuridica,FechaInicio,Trimestre,MontoRenta,MontoIVA,MontoTotalImpuestos,MontoTotal,Observaciones")] DeclaracionImpuesto declaracionImpuesto)
        {
            if (id != declaracionImpuesto.IdDeclaracionImpuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _declaracionTaxRep.ActualizarDeclaracionTax(declaracionImpuesto.IdDeclaracionImpuesto, declaracionImpuesto.FechaInicio, declaracionImpuesto.Trimestre, declaracionImpuesto.MontoRenta, declaracionImpuesto.MontoIVA, declaracionImpuesto.MontoTotalImpuestos, declaracionImpuesto.MontoTotal, declaracionImpuesto.Observaciones);
                return RedirectToAction(nameof(Index));
            }
            return View(declaracionImpuesto);
        }

        // GET: DeclaracionImpuestoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var declaracionImpuesto = await _declaracionTaxRep.ConsultarDeclaracionesImpuestos(id);
            if (declaracionImpuesto == null)
            {
                return NotFound();
            }

            return View(declaracionImpuesto);
        }

        // POST: DeclaracionImpuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _declaracionTaxRep.EliminarDeclaracionTax(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
