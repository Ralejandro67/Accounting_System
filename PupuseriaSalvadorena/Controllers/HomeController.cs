using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Scrypt;
using PupuseriaSalvadorena.Filtros;

namespace PupuseriaSalvadorena.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDetallesTransacRep _detallesTransacRep;
        private readonly IFacturaCompraRep _facturaCompraRep;
        private readonly IDetallesPronosticoRep _detallesPronosticoRep;
        private readonly IHistorialVentaRep _historialVentaRep;
        private readonly IPronosticoRep _pronosticoRep;

        public HomeController(IDetallesTransacRep detallesTransacRep, IFacturaCompraRep facturaCompraRep, IDetallesPronosticoRep detallesPronosticoRep, IHistorialVentaRep historialVentaRep, IPronosticoRep pronosticoRep)
        {
            _detallesTransacRep = detallesTransacRep;
            _facturaCompraRep = facturaCompraRep;
            _detallesPronosticoRep = detallesPronosticoRep;
            _historialVentaRep = historialVentaRep;
            _pronosticoRep = pronosticoRep;
        }

        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var cultura = new CultureInfo("es-ES");
            var ultimos3Meses = DateTime.Today.AddMonths(-3);
            var ultimas4Semenas = DateTime.Today.AddDays(-28);
            var fechaActual = DateTime.Today;
            var añoActual = fechaActual.Year;
            var mesActual = fechaActual.Month;

            var detallesTransac = await _detallesTransacRep.MostrarDetallesTransacciones();
            var facturasCompras = await _facturaCompraRep.MostrarFacturasCompras();
            var historialVentas = await _historialVentaRep.MostrarHistorialVenta();
            var IdPronostico = await _pronosticoRep.ObtenerIdPronostico();
            var detallesPronostico = await _detallesPronosticoRep.ConsultarDetallesPorPronosticos(IdPronostico);

            // Grafico Ingresos vs Egresos
            var transacciones3Meses = detallesTransac.Where(dt => dt.FechaTrans >= ultimos3Meses).ToList();
            var ingresos = transacciones3Meses.Where(dt => dt.IdMovimiento == 1).GroupBy(dt => new { dt.FechaTrans.Year, dt.FechaTrans.Month })
                                              .Select(group => new
                                              {
                                                  Año = group.Key.Year,
                                                  MesNumero = group.Key.Month,
                                                  Mes = cultura.DateTimeFormat.GetMonthName(group.Key.Month),
                                                  TotalMonto = group.Sum(dt => dt.Monto)
                                              }).OrderBy(x => x.Año).ThenBy(x => x.MesNumero).ToList();

            var egresos = transacciones3Meses.Where(dt => dt.IdMovimiento == 2).GroupBy(dt => new { dt.FechaTrans.Year, dt.FechaTrans.Month })
                                             .Select(group => new
                                             {
                                                 Año = group.Key.Year,
                                                 MesNumero = group.Key.Month,
                                                 Mes = cultura.DateTimeFormat.GetMonthName(group.Key.Month),
                                                 TotalMonto = group.Sum(dt => dt.Monto)
                                             }).OrderBy(x => x.Año).ThenBy(x => x.MesNumero).ToList();

            // Grafico Ventas
            var resultadoVentas = historialVentas.Where(venta => venta.FechaVenta >= ultimas4Semenas)
                                           .GroupBy(venta => venta.NombrePlatillo)
                                           .Select(grupo => new
                                           {
                                               NombrePlatillo = grupo.Key,
                                               TotalVentas = grupo.Count()
                                           })
                                           .ToList();

            // Grafico Compras
            var resultadoCompras = facturasCompras.Where(compra => compra.FechaFactura >= ultimas4Semenas)
                                                 .GroupBy(compra => compra.NombreMateriaPrima)
                                                 .Select(grupo => new
                                                 {
                                                     NombreMateriaPrima = grupo.Key,
                                                     TotalCompras = grupo.Count()
                                                 })
                                                 .ToList();

            // Grafico Pronostico
            var pronosticos = detallesPronostico.Select(d => new {
                                                     PCantVenta = d.PCantVenta,
                                                     FechaPronostico = d.FechaPronostico.ToString("dd-MM-yyyy")
                                                 }).ToList();

            ViewBag.Ingresos = JsonConvert.SerializeObject(ingresos);
            ViewBag.Egresos = JsonConvert.SerializeObject(egresos);
            ViewBag.Ventas = JsonConvert.SerializeObject(resultadoVentas);
            ViewBag.Compras = JsonConvert.SerializeObject(resultadoCompras);
            ViewBag.Pronosticos = JsonConvert.SerializeObject(pronosticos);

            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
    }
}