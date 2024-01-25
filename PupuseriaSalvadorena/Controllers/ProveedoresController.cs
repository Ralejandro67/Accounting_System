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
    public class ProveedoresController : Controller
    {
        private readonly IProveedorRep _proveedorRep;

        public ProveedoresController(IProveedorRep context)
        {
            _proveedorRep = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorRep.MostrarProveedores();
            return View(proveedores);
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _proveedorRep.ConsultarProveedores(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProveedor,NombreProveedor,ApellidoProveedor,Telefono")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                await _proveedorRep.CrearProveedor(proveedor.NombreProveedor, proveedor.ApellidoProveedor, proveedor.Telefono);
                return Json(new { success = true, message = "Proveedor agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el proveedor." });
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _proveedorRep.ConsultarProveedores(id);   
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdProveedor,NombreProveedor,ApellidoProveedor,Telefono")] Proveedor proveedor)
        {
            if (id != proveedor.IdProveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _proveedorRep.ActualizarProveedor(proveedor.IdProveedor, proveedor.NombreProveedor, proveedor.ApellidoProveedor, proveedor.Telefono);
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _proveedorRep.ConsultarProveedores(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _proveedorRep.EliminarProveedor(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
