using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class RolRep : IRolRep
    {
        private readonly MiDbContext _context;

        public RolRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearRol(string nombre, bool activo)
        {
            var nombreParam = new SqlParameter("@NombreRol", nombre);
            var activoParam = new SqlParameter("@Activo", activo);
            await _context.Database.ExecuteSqlRawAsync("CrearRol @NombreRol, @Activo", nombreParam, activoParam);
        }

        public async Task ActualizarRol(int IdRol, string NombreRol, bool Activo)
        {
            var idRolParam = new SqlParameter("@IdRol", IdRol);
            var nombreRolParam = new SqlParameter("@NombreRol", NombreRol);
            var activoParam = new SqlParameter("@Activo", Activo);
            await _context.Database.ExecuteSqlRawAsync("ActualizarRol @IdRol, @NombreRol, @Activo", idRolParam, nombreRolParam, activoParam);
        }

        public async Task EliminarRol(int IdRol)
        {
            var idRolParam = new SqlParameter("@IdRol", IdRol);
            await _context.Database.ExecuteSqlRawAsync("EliminarRol @IdRol", idRolParam);
        }

        public async Task<List<Rol>> MostrarRoles()
        {
            var roles = await _context.Rol
                                        .FromSqlRaw("EXEC MostrarRoles")
                                        .ToListAsync();
            return roles;
        }

        public async Task<Rol> ConsultarRoles(int IdRol)
        {
            var idRolParam = new SqlParameter("@IdRol", IdRol);
            var resultado = await _context.Rol
                                          .FromSqlRaw("EXEC ConsultarRoles @IdRol", idRolParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
