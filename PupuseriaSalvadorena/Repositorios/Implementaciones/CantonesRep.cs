using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class CantonesRep : ICantonesRep
    {
        private readonly MiDbContext _context;

        public CantonesRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Canton>> ConsultarCantones(int id)
        {
            var nombreParam = new SqlParameter("@IdProvincia", id);
            var cantones = await _context.Canton
                                        .FromSqlRaw("EXEC ConsultarCantones @IdProvincia", nombreParam)
                                        .ToListAsync();
            return cantones;
        }
    }
}
