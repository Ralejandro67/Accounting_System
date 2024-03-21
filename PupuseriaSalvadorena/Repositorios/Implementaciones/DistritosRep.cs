using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DistritosRep : IDistritosRep
    {
        private readonly MiDbContext _context;

        public DistritosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Distrito>> ConsultarDistritos(int IdCanton)
        {
            var nombreParam = new SqlParameter("@IdCanton", IdCanton);
            var resultado = await _context.Distrito
                                          .FromSqlRaw("EXEC ConsultarDistritos @IdCanton", nombreParam)
                                          .ToListAsync();

            return resultado;
        }
    }
}
