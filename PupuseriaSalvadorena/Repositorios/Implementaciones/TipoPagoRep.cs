using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TipoPagoRep : ITipoPagoRep
    {
        private readonly MiDbContext _context;

        public TipoPagoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearTipoPago(string nombre, bool Estado)
        {
            var nombreParam = new SqlParameter("@NombrePago", nombre);
            var activoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("CrearTipoPago @NombrePago, @Estado", nombreParam, activoParam);
        }

        public async Task ActualizarTipoPago(int IdTipoPago, string NombrePago, bool Estado)
        {
            var idTipoPagoParam = new SqlParameter("@IdTipoPago", IdTipoPago);
            var nombrePagoParam = new SqlParameter("@NombrePago", NombrePago);
            var estadoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("ActualizarTipoPagos @IdTipoPago, @NombrePago, @Estado", idTipoPagoParam, nombrePagoParam, estadoParam);
        }

        public async Task EliminarTipoPago(int IdTipoPago)
        {
            var idTipoPagoParam = new SqlParameter("@IdTipoPago", IdTipoPago);
            await _context.Database.ExecuteSqlRawAsync("EliminarTipoPago @IdTipoPago", idTipoPagoParam);
        }

        public async Task<List<TipoPago>> MostrarTipoPagos()
        {
            var tipoPagos = await _context.TipoPago
                                        .FromSqlRaw("EXEC MostrarTipoPagos")
                                        .ToListAsync();
            return tipoPagos;
        }

        public async Task<TipoPago> ConsultarTipoPagos(int IdTipoPago)
        {
            var nombrePagoParam = new SqlParameter("@IdTipoPago", IdTipoPago);
            var resultado = await _context.TipoPago
                                          .FromSqlRaw("EXEC ConsultarTipoPagos @IdTipoPago", nombrePagoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
