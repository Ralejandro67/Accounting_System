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

        public async Task CrearDeclaracionTax(long CedulaJuridica, DateTime FechaInicio, DateTime FechaFinal, decimal MontoTotalIngresos, decimal MontoTotalEgresos, decimal MontoTotalImpuestos, string Observaciones)
        {
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var FechaFinalParam = new SqlParameter("@FechaFinal", FechaFinal);
            var MontoTotalIngresosParam = new SqlParameter("@MontoTotalIngresos", MontoTotalIngresos);
            var MontoTotalEgresosParam = new SqlParameter("@MontoTotalEgresos", MontoTotalEgresos);
            var MontoTotalImpuestosParam = new SqlParameter("@MontoTotalImpuestos", MontoTotalImpuestos);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            await _context.Database.ExecuteSqlRawAsync("CrearDeclaracionTax @CedulaJuridica, @FechaInicio, @FechaFinal, @MontoTotalIngresos, @MontoTotalEgresos, @MontoTotalImpuestos, @Observaciones", CedulaJuridicaParam, FechaInicioParam, FechaFinalParam, MontoTotalIngresosParam, MontoTotalEgresosParam, MontoTotalImpuestosParam, ObservacionesParam);
        }

        public async Task ActualizarDeclaracionTax(string IdDeclaracionImpuesto, DateTime FechaInicio, DateTime FechaFinal, decimal MontoTotalIngresos, decimal MontoTotalEgresos, decimal MontoTotalImpuestos, string Observaciones)
        {
            var IdDeclaracionImpuestoParam = new SqlParameter("@IdDeclaracionImpuesto", IdDeclaracionImpuesto);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var FechaFinalParam = new SqlParameter("@FechaFinal", FechaFinal);
            var MontoTotalIngresosParam = new SqlParameter("@MontoTotalIngresos", MontoTotalIngresos);
            var MontoTotalEgresosParam = new SqlParameter("@MontoTotalEgresos", MontoTotalEgresos);
            var MontoTotalImpuestosParam = new SqlParameter("@MontoTotalImpuestos", MontoTotalImpuestos);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            await _context.Database.ExecuteSqlRawAsync("ActualizarDeclaracionesTax @IdDeclaracionImpuesto, @FechaInicio, @FechaFinal, @MontoTotalIngresos, @MontoTotalEgresos, @MontoTotalImpuestos, @Observaciones", IdDeclaracionImpuestoParam, FechaInicioParam, FechaFinalParam, MontoTotalIngresosParam, MontoTotalEgresosParam, MontoTotalImpuestosParam, ObservacionesParam);
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
