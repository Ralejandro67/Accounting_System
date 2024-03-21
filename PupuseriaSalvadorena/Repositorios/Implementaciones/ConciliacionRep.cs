using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class ConciliacionRep : IConciliacionRep
    {
        private readonly MiDbContext _context;

        public ConciliacionRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearConciliacion(DateTime FechaConciliacion, decimal SaldoBancario, decimal SaldoLibro, decimal Diferencia, string Observaciones, string IdRegistro, string IdRegistroLibros)
        {
            var FechaConciliacionParam = new SqlParameter("@FechaConciliacion", FechaConciliacion);
            var SaldoBancarioParam = new SqlParameter("@SaldoBancario", SaldoBancario);
            var SaldoLibroParam = new SqlParameter("@SaldoLibro", SaldoLibro);
            var DiferenciaParam = new SqlParameter("@Diferencia", Diferencia);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            var IdRegistroParam = new SqlParameter("@IdRegistro", IdRegistro);
            var IdRegistroLibrosParam = new SqlParameter("@IdRegistroLibros", IdRegistroLibros);
            await _context.Database.ExecuteSqlRawAsync("CrearConciliacion @FechaConciliacion, @SaldoBancario, @SaldoLibro, @Diferencia, @Observaciones, @IdRegistro, @IdRegistroLibros", FechaConciliacionParam, SaldoBancarioParam, SaldoLibroParam, DiferenciaParam, ObservacionesParam, IdRegistroParam, IdRegistroLibrosParam);
        }

        public async Task EliminarConciliacion(string IdConciliacion)
        {
            var IdConciliacionParam = new SqlParameter("@IdConciliacion", IdConciliacion);
            await _context.Database.ExecuteSqlRawAsync("EliminarConciliacion @IdConciliacion", IdConciliacionParam);
        }

        public async Task<List<ConciliacionBancaria>> MostrarConciliacionesBancarias()
        {
            var ConciliacionesBancarias = await _context.ConciliacionBancaria
                                                        .FromSqlRaw("MostrarConciliacionesBancarias")
                                                        .ToListAsync();
            return ConciliacionesBancarias;
        }

        public async Task<ConciliacionBancaria> ConsultarConciliacionesBancarias(string IdConciliacion)
        {
            var IdConciliacionParam = new SqlParameter("@IdConciliacion", IdConciliacion);
            var ConciliacionBancaria = await _context.ConciliacionBancaria
                                                    .FromSqlRaw("ConsultarConciliacionesBancarias @IdConciliacion", IdConciliacionParam)
                                                    .ToListAsync();

            return ConciliacionBancaria.FirstOrDefault();
        }
    }
}
