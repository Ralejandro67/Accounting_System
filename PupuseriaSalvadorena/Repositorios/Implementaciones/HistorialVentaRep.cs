using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class HistorialVentaRep : IHistorialVentaRep
    {
        private readonly MiDbContext _context;

        public HistorialVentaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearHistorialVenta(int CantVenta, DateTime FechaVenta, string IdPlatillo, int IdFacturaVenta, int IdTipoVenta)
        {
            var CantVentaParam = new SqlParameter("@CantVenta", CantVenta);
            var FechaVentaParam = new SqlParameter("@FechaVenta", FechaVenta);
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var IdTipoVentaParam = new SqlParameter("@IdTipoVenta", IdTipoVenta);
            await _context.Database.ExecuteSqlRawAsync("CrearHistorialVenta @CantVenta, @FechaVenta, @IdPlatillo, @IdFacturaVenta, @IdTipoVenta", CantVentaParam, FechaVentaParam, IdPlatilloParam, IdFacturaVentaParam, IdTipoVentaParam);
        }

        public async Task ActualizarHistorialVenta(string IdVenta, int CantVenta, string IdPlatillo, int IdFacturaVenta, int IdTipoVenta)
        {
            var IdVentaParam = new SqlParameter("@IdVenta", IdVenta);
            var CantVentaParam = new SqlParameter("@CantVenta", CantVenta);
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var IdTipoVentaParam = new SqlParameter("@IdTipoVenta", IdTipoVenta);
            await _context.Database.ExecuteSqlRawAsync("ActualizarHistorialVenta @IdVenta, @CantVenta, @IdPlatillo, @IdFacturaVenta, @IdTipoVenta", IdVentaParam, CantVentaParam, IdPlatilloParam, IdFacturaVentaParam, IdTipoVentaParam);
        }

        public async Task EliminarHistorialVenta(string IdVenta)
        {
            var IdVentaParam = new SqlParameter("@IdVenta", IdVenta);
            await _context.Database.ExecuteSqlRawAsync("EliminarHistorialVenta @IdVenta", IdVentaParam);
        }

        public async Task<List<HistorialVenta>> MostrarHistorialVenta()
        {
            var historialVenta = await _context.HistorialVenta
                                        .FromSqlRaw("EXEC MostrarHistorialVenta")
                                        .ToListAsync();
            return historialVenta;
        }

        public async Task<HistorialVenta> ConsultarHistorialVentas(string IdVenta)
        {
            var NombrePlatilloParam = new SqlParameter("@IdVenta", IdVenta);
            var resultado = await _context.HistorialVenta
                                          .FromSqlRaw("EXEC ConsultarHistorialVentas @IdVenta", NombrePlatilloParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
