using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DeclaracionTaxRep : IDeclaracionTaxRep
    {
        private readonly MiDbContext _context;

        public DeclaracionTaxRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<string> CrearDeclaracionTax(long CedulaJuridica, DateTime FechaInicio, string Trimestre, decimal MontoRenta, decimal MontoIVA, decimal MontoTotalImpuestos, decimal MontoTotal, string Observaciones)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CrearDeclaracionTax";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@CedulaJuridica", CedulaJuridica));
                command.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                command.Parameters.Add(new SqlParameter("@Trimestre", Trimestre));
                command.Parameters.Add(new SqlParameter("@MontoRenta", MontoRenta));
                command.Parameters.Add(new SqlParameter("@MontoIVA", MontoIVA));
                command.Parameters.Add(new SqlParameter("@MontoTotalImpuestos", MontoTotalImpuestos));
                command.Parameters.Add(new SqlParameter("@MontoTotal", MontoTotal));
                command.Parameters.Add(new SqlParameter("@Observaciones", Observaciones));

                var IdDeclaracion = new SqlParameter("@IdDeclaracion", System.Data.SqlDbType.VarChar, 10)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdDeclaracion);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (string)IdDeclaracion.Value;
            }   
        }

        public async Task ActualizarDeclaracionTax(string IdDeclaracionImpuesto, DateTime FechaInicio, string Trimestre, decimal MontoRenta, decimal MontoIVA, decimal MontoTotalImpuestos, decimal MontoTotal, string Observaciones)
        {
            var IdDeclaracionImpuestoParam = new SqlParameter("@IdDeclaracionImpuesto", IdDeclaracionImpuesto);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var TrimestreParam = new SqlParameter("@Trimestre", Trimestre);
            var MontoRentaParam = new SqlParameter("@MontoRenta", MontoRenta);
            var MontoIVAParam = new SqlParameter("@MontoIVA", MontoIVA);
            var MontoTotalImpuestosParam = new SqlParameter("@MontoTotalImpuestos", MontoTotalImpuestos);
            var MontoTotalParam = new SqlParameter("@MontoTotal", MontoTotal);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            await _context.Database.ExecuteSqlRawAsync("ActualizarDeclaracionesTax @IdDeclaracionImpuesto, @FechaInicio, @FechaFinal, @MontoTotalIngresos, @MontoTotalEgresos, @MontoTotalImpuestos, @MontoTotal, @Observaciones", IdDeclaracionImpuestoParam, FechaInicioParam, TrimestreParam, MontoRentaParam, MontoIVAParam, MontoTotalImpuestosParam, MontoTotalParam, ObservacionesParam);
        }

        public async Task EliminarDeclaracionTax(string IdDeclaracionImpuesto)
        {
            var IdDeclaracionImpuestoParam = new SqlParameter("@IdDeclaracionImpuesto", IdDeclaracionImpuesto);
            await _context.Database.ExecuteSqlRawAsync("EliminarDeclaracionImpuestos @IdDeclaracionImpuesto", IdDeclaracionImpuestoParam);
        }

        public async Task<List<DeclaracionImpuesto>> MostrarDeclaracionesImpuestos()
        {
            var DeclaracionesImpuestos = await _context.DeclaracionImpuesto.FromSqlRaw("MostrarDeclaracionesImpuestos").ToListAsync();
            return DeclaracionesImpuestos;
        }

        public async Task<DeclaracionImpuesto> ConsultarDeclaracionesImpuestos(string IdDeclaracionImpuesto)
        {
            var IdDeclaracionImpuestoParam = new SqlParameter("@IdDeclaracionImpuesto", IdDeclaracionImpuesto);
            var DeclaracionImpuesto = await _context.DeclaracionImpuesto.FromSqlRaw("ConsultarDeclaracionesImpuestos @IdDeclaracionImpuesto", IdDeclaracionImpuestoParam).ToListAsync();

            return DeclaracionImpuesto.FirstOrDefault();
        }
    }
}
