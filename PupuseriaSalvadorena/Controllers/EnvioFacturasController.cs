using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Filtros;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Controllers
{
    public class EnvioFacturasController : Controller
    {
        private readonly IEnvioFacturaRep _envioFacturaRep;

        public EnvioFacturasController(IEnvioFacturaRep context)
        {
            _envioFacturaRep = context;
        }

        // GET: EnvioFacturas
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var envioFacturas = await _envioFacturaRep.MostrarEnvioFactura();
            return View(envioFacturas);
        }

        // GET: EnvioFacturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var envioFactura = await _envioFacturaRep.ConsultarEnvioFacturas(id.Value);
            if (envioFactura == null)
            {
                return NotFound();
            }

            return View(envioFactura);
        }

        // POST: EnvioFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _envioFacturaRep.EliminarEnvioFactura(id);
                return Json(new { success = true, message = "Registro de envio eliminado correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el registro de envio." });
            }
        }
    }
}
