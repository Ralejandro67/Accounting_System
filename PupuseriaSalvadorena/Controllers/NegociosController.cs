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
    public class NegociosController : Controller
    {
        private readonly INegociosRep _negociosRep;
        private readonly ITelefonosRep _telefonosRep;
        private readonly IDireccionesRep _direccionesRep;
        private readonly IDistritosRep _distritosRep;
        private readonly ICantonesRep _cantonesRep;
        private readonly IProvinciasRep _provinciasRep;

        public NegociosController(INegociosRep context, ITelefonosRep telefonosRep, IDireccionesRep direccionesRep, IDistritosRep distritosRep, ICantonesRep cantonesRep, IProvinciasRep provinciasRep)
        {
            _negociosRep = context;
            _direccionesRep = direccionesRep;
            _telefonosRep = telefonosRep;
            _distritosRep = distritosRep;
            _cantonesRep = cantonesRep;
            _provinciasRep = provinciasRep;
        }

        // GET: Negocios
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador" })]
        public async Task<IActionResult> Index()
        {
            var negocios = await _negociosRep.MostrarNegocio();
            return View(negocios);
        }

        [HttpGet]
        public async Task<JsonResult> GetCantones(int IdProvincia)
        {
            var cantones = await _cantonesRep.ConsultarCantones(IdProvincia);
            return Json(new SelectList(cantones, "IdCanton", "NombreCanton"));
        }

        [HttpGet]
        public async Task<JsonResult> GetDistritos(int IdCanton)
        {
            var distritos = await _distritosRep.ConsultarDistritos(IdCanton);
            return Json(new SelectList(distritos, "IdDistrito", "NombreDistrito"));
        }

        // GET: Negocios/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negocio = await _negociosRep.ConsultarNegocio(id.Value);
            var distrito = await _distritosRep.ConsultarDistritos(negocio.IdDistrito.Value);
            var canton = await _cantonesRep.ConsultarCantones(negocio.IdCanton.Value);
            var provincia = await _provinciasRep.MostrarProvincias();

            ViewBag.Provincias = new SelectList(provincia, "IdProvincia", "NombreProvincia");
            ViewBag.Cantones = new SelectList(canton, "IdCanton", "NombreCanton");
            ViewBag.Distritos = new SelectList(distrito, "IdDistrito", "NombreDistrito");

            if (negocio == null)
            {
                return NotFound();
            }

            return PartialView("_editNegocioPartial", negocio);
        }

        // POST: Negocios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CedulaJuridica,NombreEmpresa,IdTelefono,Telefono,IdDireccion,Detalles,IdDistrito,IdCanton,IdProvincia")] Negocio negocio)
        {
            if (ModelState.IsValid)
            {
                var telefono = await _telefonosRep.ConsultarTelefonos(negocio.IdTelefono);

                if(telefono.Telefono != negocio.Telefono)
                {
                    var telefonos = await _telefonosRep.MostrarTelefonos();
                    var telefonosExistentes = telefonos.Where(t => t.Telefono == negocio.Telefono).ToList();

                    if (telefonosExistentes.Count > 0)
                    {
                        return Json(new { success = false, message = "El telefono brindado ya esta registrado." });
                    }
                }

                await _negociosRep.ActualizarNegocio(negocio.CedulaJuridica, negocio.NombreEmpresa);
                await _telefonosRep.ActualizarTelefono(negocio.IdTelefono, negocio.Telefono, negocio.Estado);
                await _direccionesRep.ActualizarDireccion(negocio.IdDireccion, negocio.Estado, negocio.Detalles, negocio.IdDistrito.Value);

                return Json(new { success = true, message = "Negocio actualizado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: Negocios/Delete/5
        public async Task<IActionResult> Delete(long? id)
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
