using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class ProvinciasRep : IProvinciasRep
    {
        private readonly MiDbContext _context;

        public ProvinciasRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Provincia>> MostrarProvincias()
        {
            var provincias = await _context.Provincia
                                          .FromSqlRaw("EXEC MostrarProvincias")
                                          .ToListAsync();
            return provincias;
        }
    }
}
