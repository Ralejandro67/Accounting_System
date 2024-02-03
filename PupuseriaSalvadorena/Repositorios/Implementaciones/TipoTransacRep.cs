using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TipoTransacRep : ITipoTransacRep
    {
        private readonly MiDbContext _context;

        public TipoTransacRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearTipoTransac(string TipoTransac, int IdMovimiento, string IdImpuesto)
        {
            var nombreParam = new SqlParameter("@TipoTransac", TipoTransac);
            var idMovimientoParam = new SqlParameter("@IdMovimiento", IdMovimiento);
            var idImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            await _context.Database.ExecuteSqlRawAsync("CrearTipoTransac @TipoTransac, @IdMovimiento, @IdImpuesto", nombreParam, idMovimientoParam, idImpuestoParam);
        }

        public async Task ActualizarTipoTransac(int IdTipo, string TipoTransac, int IdMovimiento, string IdImpuesto)
        {
            var idTipoTransacParam = new SqlParameter("@IdTipo", IdTipo);
            var nombreParam = new SqlParameter("@TipoTransac", TipoTransac);
            var idMovimientoParam = new SqlParameter("@IdMovimiento", IdMovimiento);
            var idImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            await _context.Database.ExecuteSqlRawAsync("ActualizarTipoTransac @IdTipo, @TipoTransac, @IdMovimiento, @IdImpuesto", idTipoTransacParam, nombreParam, idMovimientoParam, idImpuestoParam);
        }

        public async Task EliminarTipoTransac(int IdTipo)
        {
            var idTipoTransacParam = new SqlParameter("@IdTipo", IdTipo);
            await _context.Database.ExecuteSqlRawAsync("EliminarTipoTransaccion @IdTipo", idTipoTransacParam);
        }

        public async Task<List<TipoTransacciones>> MostrarTipoTransaccion()
        {
            var tipoTransac = await _context.TipoTransacciones
                                        .FromSqlRaw("EXEC MostrarTipoTransaccion")
                                        .ToListAsync();
            return tipoTransac;
        }

        public async Task<TipoTransacciones> ConsultarTipoTransaccion(int IdTipo)
        {
            var nombreParam = new SqlParameter("@IdTipo", IdTipo);
            var resultado = await _context.TipoTransacciones
                                          .FromSqlRaw("EXEC ConsultarTipoTransaccion @IdTipo", nombreParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<TipoTransacciones>> ConsultarImpuestosporTransaccion(string IdImpuesto)
        {
            var idImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            var resultado = await _context.TipoTransacciones
                                          .FromSqlRaw("EXEC ConsultarImpuestosporTransaccion @IdImpuesto", idImpuestoParam)
                                          .ToListAsync();

            return resultado;
        }
    }
}
