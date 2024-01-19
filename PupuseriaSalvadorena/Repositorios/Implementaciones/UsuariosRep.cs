using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class UsuariosRep : IUsuariosRep
    {
        private readonly MiDbContext _context;

        public UsuariosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearUsuario(string Contrasena, bool Estado, DateTime FechaCreacion, string IdPersona, int IdRol)
        {
            var ContrasenaParam = new SqlParameter("@Contrasena", Contrasena);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            var FechaCreacionParam = new SqlParameter("@FechaCreacion", FechaCreacion);
            var IdPersonaParam = new SqlParameter("@IdPersona", IdPersona);
            var IdRolParam = new SqlParameter("@IdRol", IdRol);
            await _context.Database.ExecuteSqlRawAsync("CrearUsuario @Contrasena, @Estado, @FechaCreacion, @IdPersona, @IdRol", ContrasenaParam, EstadoParam, FechaCreacionParam, IdPersonaParam, IdRolParam);
        }

        public async Task ActualizarUsuario(string IdUsuario, string Contrasena, bool Estado, int IdRol)
        {
            var IdUsuarioParam = new SqlParameter("@IdUsuario", IdUsuario);
            var ContrasenaParam = new SqlParameter("@Contrasena", Contrasena);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            var IdRolParam = new SqlParameter("@IdRol", IdRol);
            await _context.Database.ExecuteSqlRawAsync("ActualizarUsuario @IdUsuario, @Contrasena, @Estado, @IdRol", IdUsuarioParam, ContrasenaParam, EstadoParam, IdRolParam);
        }

        public async Task EliminarUsuario(string IdUsuario)
        {
            var IdUsuarioParam = new SqlParameter("@IdUsuario", IdUsuario);
            await _context.Database.ExecuteSqlRawAsync("EliminarUsuario @IdUsuario", IdUsuarioParam);
        }

        public async Task<List<Usuario>> MostrarUsuarios()
        {
            var usuarios = await _context.Usuario
                                        .FromSqlRaw("EXEC MostrarUsuarios")
                                        .ToListAsync();
            return usuarios;
        }

        public async Task<Usuario> ConsultarUsuarios(string IdUsuario)
        {
            var CorreoParam = new SqlParameter("@IdUsuario", IdUsuario);
            var resultado = await _context.Usuario
                                          .FromSqlRaw("EXEC ConsultarUsuarios @IdUsuario", CorreoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
