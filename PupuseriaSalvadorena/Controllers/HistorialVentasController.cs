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
    public class HistorialVentasController : Controller
    {
        private readonly IHistorialVentaRep _historialVentaRep;
        private readonly IFacturaVentaRep _facturaVentaRep;

        public HistorialVentasController(IHistorialVentaRep context, IFacturaVentaRep facturaVentaRep)
        {
            _historialVentaRep = context;
            _facturaVentaRep = facturaVentaRep;
        }

        // GET: HistorialVentas
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var facturas = await _facturaVentaRep.MostrarFacturasVentas();
            return View(facturas);   
        }
    }
}
