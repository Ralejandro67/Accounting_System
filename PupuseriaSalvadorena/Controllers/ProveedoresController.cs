using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Filtros;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using PupuseriaSalvadorena.ViewModels;

namespace PupuseriaSalvadorena.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly IProveedorRep _proveedorRep;
        private readonly IMateriaPrimaRep _materiaPrimaRep;
        private readonly ICuentaPagarRep _cuentaPagarRep;
        private readonly IFacturaCompraRep _facturaCompraRep;

        public ProveedoresController(IProveedorRep context, IMateriaPrimaRep materiaPrimaRep, ICuentaPagarRep cuentaPagarRep, IFacturaCompraRep facturaCompraRep)
        {
            _proveedorRep = context;
            _materiaPrimaRep = materiaPrimaRep;
            _cuentaPagarRep = cuentaPagarRep;
            _facturaCompraRep = facturaCompraRep;
        }

        // GET: Proveedores
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorRep.MostrarProveedores();
            return View(proveedores);
        }

        // GET: Proveedores/Details/5
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Details(string id)
        {
            CultureInfo cultura = new CultureInfo("es-ES");
            DateTime fecha = DateTime.Now;

            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _proveedorRep.ConsultarProveedores(id);
            var materiasPrimas = await _materiaPrimaRep.ConsultarMateriasPrimasProveedor(id);
            var cuentasPagar = await _cuentaPagarRep.MostrarCuentasPagar();
            var facturasCompra = await _facturaCompraRep.MostrarFacturasCompras();
            var primerDiaDelMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaDelMes = primerDiaDelMes.AddMonths(1).AddDays(-1);

            var cuentasPagarProveedor = cuentasPagar.Where(c => c.IdProveedor == id)
                                                    .OrderByDescending(cp => cp.FechaCreacion)
                                                    .ToList();

            var conteoComprasPorMateria = materiasPrimas.Where(mp => mp.IdProveedor == id)
                                                        .GroupJoin(facturasCompra.Where(f => f.FechaFactura >= primerDiaDelMes && f.FechaFactura <= ultimoDiaDelMes),
                                                        materiaPrima => materiaPrima.IdMateriaPrima,
                                                        factura => factura.IdMateriaPrima,
                                                        (materiaPrima, facturasGrupo) => new
                                                        {
                                                            NombreMateriaPrima = materiaPrima.NombreMateriaPrima,
                                                            ConteoCompras = facturasGrupo.Count()
                                                        })
                                                        .ToList();

            var JsonConteo = JsonConvert.SerializeObject(conteoComprasPorMateria);

            if (proveedor == null)
            {
                return NotFound();
            }

            ViewBag.MateriasPrimasConteo = JsonConteo;
            ViewBag.TotalCuentas = cuentasPagarProveedor.Count();
            ViewBag.TotalMateriasPrimas = materiasPrimas.Count();
            ViewBag.mesActual = cultura.DateTimeFormat.GetMonthName(fecha.Month);

            var detallesProveedor = new DetallesProveedor
            {
                Proveedor = proveedor,
                MateriasPrimas = materiasPrimas,
                CuentasPagar = cuentasPagarProveedor
            };

            return View(detallesProveedor);
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
                await _proveedorRep.CrearProveedor(proveedor.NombreProveedor, proveedor.ApellidoProveedor, proveedor.Telefono.Value);
                return Json(new { success = true, message = "Proveedor agregado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
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
            return PartialView("_editProveedorPartial", proveedor);
        }

        // POST: Proveedores/Edit/5
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
                await _proveedorRep.ActualizarProveedor(proveedor.IdProveedor, proveedor.NombreProveedor, proveedor.ApellidoProveedor, proveedor.Telefono.Value);
                return Json(new { success = true, message = "Proveedor actualizado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var proveedor = await _proveedorRep.ConsultarProveedores(id);
            return Json(new { exists = proveedor != null });
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _proveedorRep.EliminarProveedor(id);
                return Json(new { success = true, message = "Proveedor eliminado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "No se puede eliminar el proveedor ya que hay facturas asociadas." });
            }
        }
    }
}
