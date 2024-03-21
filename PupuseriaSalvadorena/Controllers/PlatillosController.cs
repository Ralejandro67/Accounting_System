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
using PupuseriaSalvadorena.Filtros;

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
        [FiltroAutentificacion(RolAcceso = new[] { "Administrador", "Contador" })]
        public async Task<IActionResult> Index()
        {
            var platillos = await _platilloRep.MostrarPlatillos();
            return View(platillos);
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
                try
                {
                    await _platilloRep.CrearPlatillo(platillo.NombrePlatillo, platillo.CostoProduccion, platillo.PrecioVenta);

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

                    return Json(new { success = true, message = "Platillo agregado correctamente." });
                }
                catch
                {
                    return Json(new { success = false, message = $"Ya existe un platillo llamado '{platillo.NombrePlatillo}'." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
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
                var producto = await _platilloRep.ConsultarPlatillos(platillo.IdPlatillo);
                
                if(producto.NombrePlatillo != platillo.NombrePlatillo)
                {
                    var platillos = await _platilloRep.MostrarPlatillos();
                    var existe = platillos.Where(p => p.NombrePlatillo == platillo.NombrePlatillo).ToList();

                    if(existe.Count > 0)
                    {
                        return Json(new { success = false, message = $"Ya existe un platillo llamado '{platillo.NombrePlatillo}'." });
                    }
                    else
                    {
                        string requestUri = $"https://api.alegra.com/api/v1/items/{platillo.IdPlatillo}";

                        var client = new HttpClient();
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Put,
                            RequestUri = new Uri(requestUri),
                            Headers =
                            {
                                { "accept", "application/json" },
                                { "authorization", "Basic cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==" },
                            },
                            Content = new StringContent("{\"name\":\""+platillo.NombrePlatillo+"\"}")
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
                    }
                }

                await _platilloRep.ActualizarPlatillo(platillo.IdPlatillo, platillo.NombrePlatillo, platillo.CostoProduccion, platillo.PrecioVenta);
                return Json(new { success = true, message = "Platillo editado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
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

                string requestUri = $"https://api.alegra.com/api/v1/items/{id}";

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(requestUri),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic cmFsZWphbmRybzY3QGdtYWlsLmNvbToxOTU4ZjIwZTBhNTJjMTc1YjM3ZQ==" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }

                return Json(new { success = true, message = "Platillo eliminado correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Error al eliminar el platillo hay facturas asociadas a el platillo." });
            }
        }
    }
}
