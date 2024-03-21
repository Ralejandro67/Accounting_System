using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TipoMovimientoRep : ITipoMovimientoRep
    {
        private readonly MiDbContext _context;

        public TipoMovimientoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoMovimiento>> MostrarTipoMovimiento()
        {
            var tipoMovimiento = await _context.TipoMovimiento
                                        .FromSqlRaw("EXEC MostrarTipoMovimiento")
                                        .ToListAsync();
            return tipoMovimiento;
        }
    }
}
