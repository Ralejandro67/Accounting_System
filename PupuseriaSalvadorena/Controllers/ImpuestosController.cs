﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class ImpuestosController : Controller
    {
        private readonly IImpuestosRep _impuestosRep;

        public ImpuestosController(IImpuestosRep context)
        {
            _impuestosRep = context;
        }

        // GET: Impuestos
        public async Task<IActionResult> Index()
        {
            var impuestos = await _impuestosRep.MostrarImpuestos();
            return View(impuestos);
        }

        // GET: Impuestos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impuesto = await _impuestosRep.ConsultarImpuestos(id);
            if (impuesto == null)
            {
                return NotFound();
            }

            return View(impuesto);
        }

        // POST: Impuestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdImpuesto,NombreImpuesto,Tasa,Estado,Descripcion")] Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                await _impuestosRep.CrearImpuesto(impuesto.NombreImpuesto, impuesto.Tasa, impuesto.Estado, impuesto.Descripcion);
                return Json(new { success = true, message = "Impuesto agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar impuesto." });
        }

        // GET: Impuestos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impuesto = await _impuestosRep.ConsultarImpuestos(id);
            if (impuesto == null)
            {
                return NotFound();
            }
            return PartialView("_EditImpuestoPartial", impuesto);
        }

        // POST: Impuestos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdImpuesto,NombreImpuesto,Tasa,Estado,Descripcion")] Impuesto impuesto)
        {
            if (id != impuesto.IdImpuesto)
            {
                return Json(new { success = false, message = "Impuesto no encontrado." });
            }

            if (ModelState.IsValid)
            {
                await _impuestosRep.ActualizarImpuesto(impuesto.IdImpuesto, impuesto.NombreImpuesto, impuesto.Tasa, impuesto.Estado, impuesto.Descripcion);
                return Json(new { success = true, message = "Impuesto actualizado correctamente." });
            }
            return Json(new { success = false, message = "Datos inválidos." });
        }

        // GET: Impuestos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var impuesto = await _impuestosRep.ConsultarImpuestos(id);
            return Json(new { exists = impuesto != null });
        }

        // POST: Impuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _impuestosRep.EliminarImpuesto(id);
                return Json(new { success = true, message = "Impuesto eliminado correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el impuesto." });
            }
        }
    }
}
