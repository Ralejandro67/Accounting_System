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

        public async Task CrearHistorialVenta(int IdVenta, int CantVenta, DateTime FechaVenta, int IdPlatillo, int IdFacturaVenta, int IdTipoVenta)
        {
            var IdVentaParam = new SqlParameter("@IdVenta", IdVenta);
            var CantVentaParam = new SqlParameter("@CantVenta", CantVenta);
            var FechaVentaParam = new SqlParameter("@FechaVenta", FechaVenta);
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var IdTipoVentaParam = new SqlParameter("@IdTipoVenta", IdTipoVenta);
            await _context.Database.ExecuteSqlRawAsync("CrearHistorialVenta @IdVenta, @CantVenta, @FechaVenta, @IdPlatillo, @IdFacturaVenta, @IdTipoVenta", IdVentaParam, CantVentaParam, FechaVentaParam, IdPlatilloParam, IdFacturaVentaParam, IdTipoVentaParam);
        }

        public async Task ActualizarHistorialVenta(int IdVenta, int CantVenta, int IdPlatillo, int IdFacturaVenta, int IdTipoVenta)
        {
            var IdVentaParam = new SqlParameter("@IdVenta", IdVenta);
            var CantVentaParam = new SqlParameter("@CantVenta", CantVenta);
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var IdTipoVentaParam = new SqlParameter("@IdTipoVenta", IdTipoVenta);
            await _context.Database.ExecuteSqlRawAsync("ActualizarHistorialVenta @IdVenta, @CantVenta, @IdPlatillo, @IdFacturaVenta, @IdTipoVenta", IdVentaParam, CantVentaParam, IdPlatilloParam, IdFacturaVentaParam, IdTipoVentaParam);
        }

        public async Task EliminarHistorialVenta(int IdVenta)
        {
            var IdVentaParam = new SqlParameter("@IdVenta", IdVenta);
            await _context.Database.ExecuteSqlRawAsync("EliminarHistorialVenta @IdVenta", IdVentaParam);
        }

        public async Task<List<HistorialVenta>> MostrarHistorialVenta()
        {
            var historialVenta = await _context.HistorialVenta
                                        .FromSqlRaw("EXEC MostrarHistorialVentas")
                                        .ToListAsync();
            return historialVenta;
        }

        public async Task<HistorialVenta> ConsultarHistorialVentas(int IdVenta)
        {
            var NombrePlatilloParam = new SqlParameter("@IdVenta", IdVenta);
            var resultado = await _context.HistorialVenta
                                          .FromSqlRaw("EXEC ConsultarHistorialVentas @IdVenta", NombrePlatilloParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<HistorialVenta>> ConsultarHistorialVentasPlatillos(int IdPlatillo)
        {
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var resultado = await _context.HistorialVenta
                                          .FromSqlRaw("EXEC ConsultarHistorialVentasPlatillos @IdPlatillo", IdPlatilloParam)
                                          .ToListAsync();

            return resultado;
        }

        public async Task<HistorialVenta> ConsultarHistorialVentasFactura(int IdFacturaVenta)
        {
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var resultado = await _context.HistorialVenta
                                          .FromSqlRaw("EXEC ConsultarHistorialVentasFactura @IdFacturaVenta", IdFacturaVentaParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<HistorialVenta>> ConsultarDetallesVentas(int IdFacturaVenta)
        {
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var resultado = await _context.HistorialVenta
                                          .FromSqlRaw("EXEC ConsultarDetallesVentas @IdFacturaVenta", IdFacturaVentaParam)
                                          .ToListAsync();

            return resultado;
        }
    }
}
