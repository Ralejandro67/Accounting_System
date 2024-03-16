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
using PupuseriaSalvadorena.Filtros;

namespace PupuseriaSalvadorena.Controllers
{
    public class CategoriaPresupuestoesController : Controller
    {
        private readonly ICatPresupuestoRep _categoriaPresupuestoRep;

        public CategoriaPresupuestoesController(ICatPresupuestoRep context)
        {
            _categoriaPresupuestoRep = context;
        }

        // GET: CategoriaPresupuestoes
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var categoriaPresupuesto = await _categoriaPresupuestoRep.MostrarCatPresupuestos();
            return View(categoriaPresupuesto);
        }

        // GET: CategoriaPresupuestoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaPresupuesto = await _categoriaPresupuestoRep.ConsultarCatPresupuestos(id.Value);
            if (categoriaPresupuesto == null)
            {
                return NotFound();
            }

            return View(categoriaPresupuesto);
        }

        // GET: CategoriaPresupuestoes/Create
        public IActionResult Create()
        {
            return PartialView("_newCategoriaPPartial", new CategoriaPresupuesto());
        }

        // POST: CategoriaPresupuestoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoriaP,Nombre,Estado")] CategoriaPresupuesto categoriaPresupuesto)
        {
            if (ModelState.IsValid)
            {
                await _categoriaPresupuestoRep.CrearCatPresupuesto(categoriaPresupuesto.Nombre, categoriaPresupuesto.Estado);
                return Json(new { success = true, message = "Categoria agregada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: CategoriaPresupuestoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaPresupuesto = await _categoriaPresupuestoRep.ConsultarCatPresupuestos(id.Value);
            if (categoriaPresupuesto == null)
            {
                return NotFound();
            }
            return PartialView("_editCategoriaPPartial", categoriaPresupuesto);
        }

        // POST: CategoriaPresupuestoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoriaP,Nombre,Estado")] CategoriaPresupuesto categoriaPresupuesto)
        {
            if (id != categoriaPresupuesto.IdCategoriaP)
            {
                return Json(new { success = false, message = "Error al editar la categoria." });
            }

            if (ModelState.IsValid)
            {
                await _categoriaPresupuestoRep.ActualizarCatPresupuestos(categoriaPresupuesto.IdCategoriaP, categoriaPresupuesto.Nombre, categoriaPresupuesto.Estado);
                return Json(new { success = true, message = "Categoria editada correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: CategoriaPresupuestoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var categoriaPresupuesto = await _categoriaPresupuestoRep.ConsultarCatPresupuestos(id.Value);
            return Json(new { exists = categoriaPresupuesto != null });
        }

        // POST: CategoriaPresupuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categoriaPresupuestoRep.EliminarCatPresupuestos(id);
                return Json(new { success = true, message = "Categoria eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la categoria." });
            }
        }
    }
}
