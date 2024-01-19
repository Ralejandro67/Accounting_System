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
    public class NegociosController : Controller
    {
        private readonly INegociosRep _negociosRep;

        public NegociosController(INegociosRep context)
        {
            _negociosRep = context;
        }

        // GET: Negocios
        public async Task<IActionResult> Index()
        {
            var negocios = await _negociosRep.MostrarNegocio();
            return View(negocios);
        }

        // GET: Negocios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _negociosRep.ConsultarNegocio(id.Value);
            if (negocio == null)
            {
                return NotFound();
            }

            return View(negocio);
        }

        // GET: Negocios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Negocios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CedulaJuridica,NombreEmpresa,IdDireccion,IdTelefono")] Negocio negocio)
        {
            if (ModelState.IsValid)
            {
                await _negociosRep.CrearNegocio(negocio.CedulaJuridica, negocio.NombreEmpresa, negocio.IdDireccion, negocio.IdTelefono);
                return RedirectToAction(nameof(Index));
            }
            return View(negocio);
        }

        // GET: Negocios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _negociosRep.ConsultarNegocio(id.Value);
            if (negocio == null)
            {
                return NotFound();
            }
            return View(negocio);
        }

        // POST: Negocios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CedulaJuridica,NombreEmpresa,IdDireccion,IdTelefono")] Negocio negocio)
        {
            if (id != negocio.CedulaJuridica)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _negociosRep.ActualizarNegocio(negocio.CedulaJuridica, negocio.NombreEmpresa);
                return RedirectToAction(nameof(Index));
            }
            return View(negocio);
        }

        // GET: Negocios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _negociosRep.ConsultarNegocio(id.Value);
            if (negocio == null)
            {
                return NotFound();
            }

            return View(negocio);
        }

        // POST: Negocios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _negociosRep.EliminarNegocio(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
