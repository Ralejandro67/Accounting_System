using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class PronosticoRep : IPronosticoRep
    {
        private readonly MiDbContext _context;

        public PronosticoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearPronostico(string IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta, string PronosticoDoc)
        {
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var FechaFinalParam = new SqlParameter("@FechaFinal", FechaFinal);
            var CantTotalProdParam = new SqlParameter("@CantTotalProd", CantTotalProd);
            var TotalVentaParam = new SqlParameter("@TotalVenta", TotalVenta);
            var PronosticoDocParam = new SqlParameter("@PronosticoDoc", PronosticoDoc);
            await _context.Database.ExecuteSqlRawAsync("CrearPronostico @IdPlatillo, @FechaInicio, @FechaFinal, @CantTotalProd, @TotalVenta, @PronosticoDoc", IdPlatilloParam, FechaInicioParam, FechaFinalParam, CantTotalProdParam, TotalVentaParam, PronosticoDocParam);
        }

        public async Task ActualizarPronosticos(int IdPronostico, string IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta, string PronosticoDoc)
        {
            var IdPronosticoParam = new SqlParameter("@IdPronostico", IdPronostico);
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var FechaFinalParam = new SqlParameter("@FechaFinal", FechaFinal);
            var CantTotalProdParam = new SqlParameter("@CantTotalProd", CantTotalProd);
            var TotalVentaParam = new SqlParameter("@TotalVenta", TotalVenta);
            var PronosticoDocParam = new SqlParameter("@PronosticoDoc", PronosticoDoc);
            await _context.Database.ExecuteSqlRawAsync("ActualizarPronosticos @IdPronostico, @IdPlatillo, @FechaInicio, @FechaFinal, @CantTotalProd, @TotalVenta, @PronosticoDoc", IdPronosticoParam, IdPlatilloParam, FechaInicioParam, FechaFinalParam, CantTotalProdParam, TotalVentaParam, PronosticoDocParam);
        }

        public async Task EliminarPronostico(int IdPronostico)
        {
            var IdPronosticoParam = new SqlParameter("@IdPronostico", IdPronostico);
            await _context.Database.ExecuteSqlRawAsync("EliminarPronostico @IdPronostico", IdPronosticoParam);
        }

        public async Task<List<Pronostico>> MostrarPronostico()
        {
            var pronosticos = await _context.Pronostico
                                        .FromSqlRaw("EXEC MostrarPronostico")
                                        .ToListAsync();
            return pronosticos;
        }

        public async Task<Pronostico> ConsultarPronosticos(int IdPronostico)
        {
            var IdPronosticoParam = new SqlParameter("@IdPronostico", IdPronostico);
            var resultado = await _context.Pronostico
                                          .FromSqlRaw("EXEC ConsultarPronosticos @IdPronostico", IdPronosticoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
