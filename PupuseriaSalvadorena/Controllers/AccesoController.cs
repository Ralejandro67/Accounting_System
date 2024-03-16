using Microsoft.AspNetCore.Mvc;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using Scrypt;
using System.Security.Cryptography;
using System.Text.Json;
using System.Data;
using System.Linq;

namespace PupuseriaSalvadorena.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuariosRep _usuariosRep;

        public AccesoController(IUsuariosRep usuariosRep, IPersonasRep personasRep, ICorreosRep correosRep)
        {
            _usuariosRep = usuariosRep;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("CorreoElectronico,Contrasena")] Usuario usuario)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var user = await _usuariosRep.ConsultarUsuarioCorreo(usuario.CorreoElectronico);

            if (user != null)
            {                
                if(encoder.Compare(usuario.Contrasena, user.Contrasena))
                {
                    var userSession = new { IdUsuario = user.IdUsuario, Nombre = user.Nombre, NombreUsuario = user.NombreUsuario, IdRol = user.IdRol, NombreRol = user.NombreRol };
                    var userBytes = JsonSerializer.SerializeToUtf8Bytes(userSession, options);
                    HttpContext.Session.SetString("NombreUsuario", user.Nombre);
                    HttpContext.Session.SetString("RolUsuario", user.NombreRol);
                    HttpContext.Session.SetString("IdUsuario", user.IdUsuario);
                    HttpContext.Session.Set("Usuario", userBytes);
                    return Json(new { success = true, url = Url.Action("Index", "Home") });
                }
                else
                {
                    return Json(new { success = false, message = "La contraseña es incorrecta." });
                }
            }
            else
            {
                return Json(new { success = false, message = "El correo electrónico es incorrecto." });
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("IniciarSesion", "Home");
        }
    }
}
