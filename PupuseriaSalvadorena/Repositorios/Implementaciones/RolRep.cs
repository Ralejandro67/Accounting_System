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

        public async Task<List<Rol>> MostrarRoles()
        {
            var roles = await _context.Rol
                                        .FromSqlRaw("EXEC MostrarRoles")
                                        .ToListAsync();
            return roles;
        }
    }
}
