using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class PronosticoRep : IPronosticoRep
    {
        private readonly MiDbContext _context;

        public PronosticoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearPronostico(int IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CrearPronostico";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@IdPlatillo", IdPlatillo));
                command.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                command.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                command.Parameters.Add(new SqlParameter("@CantTotalProd", CantTotalProd));
                command.Parameters.Add(new SqlParameter("@TotalVentas", TotalVenta));
                
                var IdPronostico = new SqlParameter("@IdPronostico", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdPronostico);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (int)IdPronostico.Value;
            }
        }

        public async Task ActualizarPronosticos(int IdPronostico, int IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta)
        {
            var IdPronosticoParam = new SqlParameter("@IdPronostico", IdPronostico);
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var FechaFinalParam = new SqlParameter("@FechaFinal", FechaFinal);
            var CantTotalProdParam = new SqlParameter("@CantTotalProd", CantTotalProd);
            var TotalVentaParam = new SqlParameter("@TotalVenta", TotalVenta);
            await _context.Database.ExecuteSqlRawAsync("ActualizarPronosticos @IdPronostico, @IdPlatillo, @FechaInicio, @FechaFinal, @CantTotalProd, @TotalVenta, @PronosticoDoc", IdPronosticoParam, IdPlatilloParam, FechaInicioParam, FechaFinalParam, CantTotalProdParam, TotalVentaParam);
        }

        public async Task EliminarPronostico(int IdPronostico)
        {
            var IdPronosticoParam = new SqlParameter("@IdPronostico", IdPronostico);
            await _context.Database.ExecuteSqlRawAsync("EliminarPronostico @IdPronostico", IdPronosticoParam);
        }

        public async Task<List<Pronostico>> MostrarPronosticos()
        {
            var pronosticos = await _context.Pronostico
                                        .FromSqlRaw("EXEC MostrarPronosticos")
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

        public async Task<int> ObtenerIdPronostico()
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "ObtenerIdPronostico";
                command.CommandType = CommandType.StoredProcedure;

                var IdPronostico = new SqlParameter("@IdPronostico", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdPronostico);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (int)IdPronostico.Value;
            }
        }
    }
}
