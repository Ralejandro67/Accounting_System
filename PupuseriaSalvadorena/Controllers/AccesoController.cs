using Microsoft.AspNetCore.Mvc;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace PupuseriaSalvadorena.Controllers
{
    public class AccesoController : Controller
    {
        private readonly MiDbContext _context;

        public AccesoController(MiDbContext context)
        {
            _context = context;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string ecorreo, string contrasena)
        {
            JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            if (contrasena != null && ecorreo != null)
            {
                byte[] ByteContrasena = System.Text.Encoding.UTF8.GetBytes(contrasena);
                SHA256 sha256 = SHA256.Create();
                byte[] Password = sha256.ComputeHash(ByteContrasena);
                contrasena = Convert.ToBase64String(Password);

                var usuario = _context.Usuario
                    .Join(_context.Persona,
                          usuario => usuario.IdPersona, 
                          persona => persona.IdPersona, 
                          (usuario, persona) => new { Usuario = usuario, Persona = persona }) 
                    .Join(_context.CorreoElectronico, 
                          up => up.Persona.IdCorreoElectronico, 
                          correo => correo.IdCorreoElectronico, 
                          (up, correo) => new { up.Usuario, up.Persona, Correo = correo }) 
                    .Where(x => x.Correo.Correo == ecorreo && x.Usuario.Contrasena == contrasena) 
                    .Select(x => x.Usuario)
                    .FirstOrDefault();

                if (usuario != null)
                {
                    usuario.Contrasena = "Vacio";
                    var UsuarioByte = JsonSerializer.SerializeToUtf8Bytes(usuario, options);
                    HttpContext.Session.Set("Usuario", UsuarioByte);

                    return RedirectToAction("Principal", "Contabilidad");
                }
                else
                {
                    ViewData["Error"] = "El correo o contraseña incorrectos";
                    return View();
                }
            }
            else
            {
                ViewData["Error"] = "El correo o contraseña incorrectos";
                return View();
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("Login", "Acceso");
        }
    }
}
