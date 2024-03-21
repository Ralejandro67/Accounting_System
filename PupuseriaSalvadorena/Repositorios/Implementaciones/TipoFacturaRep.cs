using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TipoFacturaRep : ITipoFacturaRep
    {
        private readonly MiDbContext _context;

        public TipoFacturaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoFactura>> MostrarTipoFacturas()
        {
            var tipoFacturas = await _context.TipoFactura
                                        .FromSqlRaw("EXEC MostrarTipoFacturas")
                                        .ToListAsync();
            return tipoFacturas;
        }
    }
}
