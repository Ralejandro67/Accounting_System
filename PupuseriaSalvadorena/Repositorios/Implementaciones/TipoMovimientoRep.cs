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

        public async Task CrearTipoMovimiento(string NombreMov)
        {
            var nombreParam = new SqlParameter("@NombreMov", NombreMov);
            await _context.Database.ExecuteSqlRawAsync("CrearTipoMovimiento @NombreMov", nombreParam);
        }

        public async Task ActualizarTipoMovimiento(int IdMovimiento, string NombreMov)
        {
            var idTipoMovimientoParam = new SqlParameter("@IdMovimiento", IdMovimiento);
            var nombreParam = new SqlParameter("@NombreMov", NombreMov);
            await _context.Database.ExecuteSqlRawAsync("ActualizarTipoMovimiento @IdMovimiento, @NombreMov", idTipoMovimientoParam, nombreParam);
        }

        public async Task EliminarTipoMovimiento(int IdMovimiento)
        {
            var idTipoMovimientoParam = new SqlParameter("@IdMovimiento", IdMovimiento);
            await _context.Database.ExecuteSqlRawAsync("EliminarTipoMovimiento @IdMovimiento", idTipoMovimientoParam);
        }

        public async Task<List<TipoMovimiento>> MostrarTipoMovimiento()
        {
            var tipoMovimiento = await _context.TipoMovimiento
                                        .FromSqlRaw("EXEC MostrarTipoMovimiento")
                                        .ToListAsync();
            return tipoMovimiento;
        }

        public async Task<TipoMovimiento> ConsultarTipoMovimiento(int IdMovimiento)
        {
            var idTipoMovimientoParam = new SqlParameter("@IdMovimiento", IdMovimiento);
            var resultado = await _context.TipoMovimiento
                                          .FromSqlRaw("EXEC ConsultarTipoMovimiento @IdMovimiento", idTipoMovimientoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
