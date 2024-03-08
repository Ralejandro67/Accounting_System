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
    public class MateriaPrimasController : Controller
    {
        private readonly IMateriaPrimaRep _materiaPrimaRep; 
        private readonly IProveedorRep _proveedorRep;

        public MateriaPrimasController(IMateriaPrimaRep context, IProveedorRep proveedorRep)
        {
            _materiaPrimaRep = context;
            _proveedorRep = proveedorRep;
        }

        // GET: MateriaPrimas
        public async Task<IActionResult> Index()
        {
            var materiaPrimas = await _materiaPrimaRep.MostrarMateriaPrima();
            return View(materiaPrimas); 
        }

        // GET: MateriaPrimas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaPrima = await _materiaPrimaRep.ConsultarMateriasPrimas(id.Value);
            if (materiaPrima == null)
            {
                return NotFound();
            }

            return View(materiaPrima);
        }

        // GET: MateriaPrimas/Create
        public async Task<IActionResult> Create()
        {
            var proveedores = await _proveedorRep.MostrarProveedores();
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "ProveedorCompleto");
            return PartialView("_newMateriaPrimaPartial", new MateriaPrima());
        }

        // POST: MateriaPrimas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMateriaPrima,NombreMateriaPrima,IdProveedor")] MateriaPrima materiaPrima)
        {
            if (ModelState.IsValid)
            {
                await _materiaPrimaRep.CrearMateriaPrima(materiaPrima.NombreMateriaPrima, materiaPrima.IdProveedor);
                return Json(new { success = true, message = "Materia Prima agregada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: MateriaPrimas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaPrima = await _materiaPrimaRep.ConsultarMateriasPrimas(id.Value);
            var proveedores = await _proveedorRep.MostrarProveedores();
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "ProveedorCompleto");

            if (materiaPrima == null)
            {
                return NotFound();
            }

            return PartialView("_editMateriaPrimaPartial", materiaPrima);
        }

        // POST: MateriaPrimas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMateriaPrima,NombreMateriaPrima,IdProveedor")] MateriaPrima materiaPrima)
        {
            if (ModelState.IsValid)
            {
                await _materiaPrimaRep.ActualizarMateriaPrima(materiaPrima.IdMateriaPrima, materiaPrima.NombreMateriaPrima, materiaPrima.IdProveedor);  
                return Json(new { success = true, message = "Materia Prima actualizada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: MateriaPrimas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaPrima = await _materiaPrimaRep.ConsultarMateriasPrimas(id.Value);
            if (materiaPrima == null)
            {
                return NotFound();
            }

            return View(materiaPrima);
        }

        // POST: MateriaPrimas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _materiaPrimaRep.EliminarMateriaPrima(id);
                return Json(new { success = true, message = "Materia Prima eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "No se puede eliminar la materia prima, ya que hay facturas asociadas." });
            }
        }
    }
}
