using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using PupuseriaSalvadorena.ViewModels;
using Scrypt;

namespace PupuseriaSalvadorena.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuariosRep _usuariosRep;
        private readonly IDireccionesRep _direccionesRep;
        private readonly IDistritosRep _distritosRep;
        private readonly ICantonesRep _cantonesRep;
        private readonly IProvinciasRep _provinciasRep;
        private readonly IPersonasRep _personasRep;
        private readonly ITelefonosRep _telefonosRep;
        private readonly ICorreosRep _correosElectRep;
        private readonly IRolRep _rolesRep;

        public UsuariosController(IUsuariosRep context, IDireccionesRep direccionesRep, IDistritosRep distritosRep, ICantonesRep cantonesRep, IProvinciasRep provinciasRep, IPersonasRep personasRep, ITelefonosRep telefonosRep, ICorreosRep correosElectRep, IRolRep rolesRep)
        {
            _usuariosRep = context;
            _direccionesRep = direccionesRep;
            _distritosRep = distritosRep;
            _cantonesRep = cantonesRep;
            _provinciasRep = provinciasRep;
            _personasRep = personasRep;
            _telefonosRep = telefonosRep;
            _correosElectRep = correosElectRep;
            _rolesRep = rolesRep;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuariosRep.MostrarUsuarios();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuariosRep.ConsultarUsuarios(id);
            var user = new User{ Usuario = usuario };

            if (usuario == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Usuarios/Create
        public async Task<IActionResult> Create()
        {
            var roles = await _rolesRep.MostrarRoles();
            var provincia = await _provinciasRep.MostrarProvincias();

            ViewBag.Roles = new SelectList(roles, "IdRol", "NombreRol");
            ViewBag.Provincias = new SelectList(provincia, "IdProvincia", "NombreProvincia");
            return PartialView("_newUsuarioPartial", new Usuario());
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

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Cedula,Nombre,Apellido,FechaNac,Telefono,CorreoElectronico,Contrasena,Estado,FechaCreacion,IdRol,Detalles,IdDistrito")] Usuario usuario)
        {
            ScryptEncoder encoder = new ScryptEncoder();

            if (ModelState.IsValid)
            {
                string password = encoder.Encode(usuario.Contrasena);

                var correos = await _correosElectRep.MostrarCorreos();
                var telefonos = await _telefonosRep.MostrarTelefonos();
                var telefonosExistentes = telefonos.Where(t => t.Telefono == usuario.Telefono).ToList();
                var correosExistentes = correos.Where(c => c.Correo == usuario.CorreoElectronico).ToList();

                if (correosExistentes.Count > 0)
                {
                    return Json(new { success = false, message = "El correo brindado ya esta registrado." });
                }
                if (telefonosExistentes.Count > 0)
                {
                    return Json(new { success = false, message = "El telefono brindado ya esta registrado." });
                }

                var idCorreo = await _correosElectRep.CrearCorreo(usuario.CorreoElectronico);
                var idTelefono = await _telefonosRep.CrearTelefono(usuario.Telefono, usuario.Estado);
                var idDireccion = await _direccionesRep.CrearDireccion(usuario.Estado, usuario.Detalles, usuario.IdDistrito.Value);
                var idPersona = await _personasRep.CrearPersona(usuario.Cedula.Value, usuario.Nombre, usuario.Apellido, usuario.FechaNac, idCorreo, idDireccion, idTelefono);
                await _usuariosRep.CrearUsuario(password, usuario.Estado, usuario.FechaCreacion, idPersona, usuario.IdRol.Value);

                return Json(new { success = true, message = "Usuario creado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            var usuario = await _usuariosRep.ConsultarUsuarios(id);
            var roles = await _rolesRep.MostrarRoles();
            var provincias = await _provinciasRep.MostrarProvincias();
            var cantones = await _cantonesRep.ConsultarCantones(usuario.IdProvincia.Value);
            var distritos = await _distritosRep.ConsultarDistritos(usuario.IdCanton.Value);

            ViewBag.Roles = new SelectList(roles, "IdRol", "NombreRol");
            ViewBag.Provincias = new SelectList(provincias, "IdProvincia", "NombreProvincia");
            ViewBag.Cantones = new SelectList(cantones, "IdCanton", "NombreCanton");
            ViewBag.Distritos = new SelectList(distritos, "IdDistrito", "NombreDistrito");

            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            return PartialView("_editUsuarioPartial", usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdUsuario,IdPersona,Cedula,Nombre,Apellido,FechaNac,Telefono,CorreoElectronico,Contrasena,Estado,FechaCreacion,IdRol,Detalles,IdDistrito")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var user = await _usuariosRep.ConsultarUsuarios(id);
                var persona = await _personasRep.ConsultarPersonas(usuario.IdPersona);
                var telefonos = await _telefonosRep.MostrarTelefonos();
                var telefonosExistentes = telefonos.Where(t => t.Telefono == usuario.Telefono).ToList();

                if (telefonosExistentes.Count > 0)
                {
                    return Json(new { success = false, message = "El telefono brindado ya esta registrado." });
                }

                await _telefonosRep.ActualizarTelefono(persona.IdTelefono, usuario.Telefono, usuario.Estado);
                await _direccionesRep.ActualizarDireccion(persona.IdDireccion, usuario.Estado, usuario.Detalles, usuario.IdDistrito.Value);
                await _personasRep.ActualizarPersona(usuario.IdPersona, usuario.Nombre, usuario.Apellido);
                await _usuariosRep.ActualizarUsuario(usuario.IdUsuario, usuario.Contrasena, usuario.Estado, usuario.IdRol.Value);
                return Json(new { success = true, message = "Usuario actualizado correctamente." });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var usuario = await _usuariosRep.ConsultarUsuarios(id);
                await _usuariosRep.EliminarUsuario(id);
                await _personasRep.EliminarPersona(usuario.IdPersona);
                await _telefonosRep.EliminarTelefono(usuario.IdTelefono);
                await _direccionesRep.EliminarDireccion(usuario.IdDireccion);
                await _correosElectRep.EliminarCorreo(usuario.IdCorreoElectronico);
                return Json(new { success = true, message = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errors = "Error al eliminar el usuario." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Password(string id)
        {
            return PartialView("_editPasswordPartial", new Usuario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(string id, [Bind("IdUsuario,Contrasena,NuevaContrasena")] Usuario usuario)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            var user = await _usuariosRep.ConsultarUsuarios(id);

            if(user == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            string password = usuario.Contrasena;
            string hashedPassword = user.Contrasena;

            try
            {
                if (encoder.Compare(password, hashedPassword))
                {
                    string newPassword = encoder.Encode(usuario.NuevaContrasena);
                    await _usuariosRep.ActualizarUsuario(id, newPassword, user.Estado, user.IdRol.Value);
                    return Json(new { success = true, message = "Contraseña actualizada correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "La contraseña actual no coincide." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(new { success = false, message = "La contraseña actual no coincide." });
        }
    }
}
