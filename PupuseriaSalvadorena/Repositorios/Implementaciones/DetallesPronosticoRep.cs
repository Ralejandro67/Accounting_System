using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DetallesPronosticoRep : IDetallesPronosticoRep
    {
        private readonly MiDbContext _context;

        public DetallesPronosticoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearDetallesPronostico(int IdPronostico, DateTime FechaPronostico, int PCantVenta, decimal PValorVenta)
        {
            var IdPronosticoParam = new SqlParameter("@IdPronostico", IdPronostico);
            var FechaPronosticoParam = new SqlParameter("@FechaPronostico", FechaPronostico);
            var PCantVentaParam = new SqlParameter("@PCantVenta", PCantVenta);
            var PValorVentaParam = new SqlParameter("@PValorVenta", PValorVenta);
            await _context.Database.ExecuteSqlRawAsync("CrearDetallesPronostico @IdPronostico, @FechaPronostico, @PCantVenta, @PValorVenta", IdPronosticoParam, FechaPronosticoParam, PCantVentaParam, PValorVentaParam);
        }

        public async Task EliminarDetallesPronostico(int IdDetallePronostico)
        {
            var IdDetallePronosticoParam = new SqlParameter("@IdDetallePronostico", IdDetallePronostico);
            await _context.Database.ExecuteSqlRawAsync("EliminarDetallesPronostico @IdDetallePronostico", IdDetallePronosticoParam);
        }

        public async Task<List<DetallesPronostico>> MostrarDetallesPronosticos()
        {
            var detallesPronosticos = await _context.DetallesPronostico
                                            .FromSqlRaw("EXEC MostrarDetallesPronosticos")
                                            .ToListAsync();

            return detallesPronosticos;
        }

        public async Task<DetallesPronostico> ConsultarDetallesPronosticos(int IdDetallePronostico)
        {
            var IdDetallePronosticoParam = new SqlParameter("@IdDetallePronostico", IdDetallePronostico);
            var resultado = await _context.DetallesPronostico
                                          .FromSqlRaw("EXEC ConsultarDetallesPronosticos @IdDetallePronostico", IdDetallePronosticoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<DetallesPronostico>> ConsultarDetallesPorPronosticos(int IdPronostico)
        {
            var IdPronosticoParam = new SqlParameter("@IdPronostico", IdPronostico);
            var resultado = await _context.DetallesPronostico
                                          .FromSqlRaw("EXEC ConsultarDetallesPorPronosticos @IdPronostico", IdPronosticoParam)
                                          .ToListAsync();

            return resultado;
        }
    }
}
