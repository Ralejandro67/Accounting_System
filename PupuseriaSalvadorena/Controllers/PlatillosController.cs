using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Text;
using PupuseriaSalvadorena.Repositorios.Implementaciones;

namespace PupuseriaSalvadorena.Controllers
{
    public class PlatillosController : Controller
    {
        private readonly IPlatilloRep _platilloRep;

        public PlatillosController(IPlatilloRep context)
        {
            _platilloRep = context;
        }

        // GET: Platillos
        public async Task<IActionResult> Index()
        {
            var platillos = await _platilloRep.MostrarPlatillos();
            return View(platillos);
        }

        // GET: Platillos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platillo = await _platilloRep.ConsultarPlatillos(id.Value);
            if (platillo == null)
            {
                return NotFound();
            }

            return View(platillo);
        }

        // GET: Platillos/Create
        public IActionResult Create()
        {
            return PartialView("_newPlatilloPartial", new Platillo());
        }

        // POST: Platillos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlatillo,NombrePlatillo,CostoProduccion,PrecioVenta")] Platillo platillo)
        {
            if (ModelState.IsValid)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.alegra.com/api/v1/items"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==" },
                    },
                    Content = new StringContent($"{{\"inventory\":{{\"unit\":\"unit\"}},\"name\":\"{platillo.NombrePlatillo}\",\"price\":{platillo.PrecioVenta},\"tax\":1}}", Encoding.UTF8, "application/json")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }

                await _platilloRep.CrearPlatillo(platillo.NombrePlatillo, platillo.CostoProduccion, platillo.PrecioVenta);
                return Json(new { success = true, message = "Platillo agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar el platillo." });
        }

        // GET: Platillos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Error al editar el platillo." });
            }

            var platillo = await _platilloRep.ConsultarPlatillos(id.Value);
            if (platillo == null)
            {
                return Json(new { success = false, message = "No se encontro el platillo seleccionado." });
            }
            return PartialView("_editPlatilloPartial", platillo);
        }

        // POST: Platillos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlatillo,NombrePlatillo,CostoProduccion,PrecioVenta")] Platillo platillo)
        {
            if (id != platillo.IdPlatillo)
            {
                return Json(new { success = false, message = "Error al editar el platillo." });
            }

            if (ModelState.IsValid)
            {
                await _platilloRep.ActualizarPlatillo(platillo.IdPlatillo, platillo.NombrePlatillo, platillo.CostoProduccion, platillo.PrecioVenta);
                return Json(new { success = true, message = "Platillo editado correctamente." });
            }
            return Json(new { success = false, message = "Error al editar el platillo." });
        }

        // GET: Platillos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var platillo = await _platilloRep.ConsultarPlatillos(id.Value);
            return Json(new { exists = platillo != null });
        }

        // POST: Platillos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _platilloRep.EliminarPlatillo(id);
                return Json(new { success = true, message = "Platillo eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el platillo." });
            }
        }
    }
}
