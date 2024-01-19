using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class FacturaCompraRep : IFacturaCompraRep
    {
        private readonly MiDbContext _context;

        public FacturaCompraRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearFacturaCompra(string FacturaCompra, DateTime FechaFactura, decimal TotalCompra, string DetallesCompra, int IdTipoPago, int IdTipoFactura)
        {
            var FacturaCompraParam = new SqlParameter("@FacturaCompra", FacturaCompra);
            var FechaFacturaParam = new SqlParameter("@FechaFactura", FechaFactura);
            var TotalCompraParam = new SqlParameter("@TotalCompra", TotalCompra);
            var DetallesCompraParam = new SqlParameter("@DetallesCompra", DetallesCompra);
            var IdTipoPagoParam = new SqlParameter("@IdTipoPago", IdTipoPago);
            var IdTipoFacturaParam = new SqlParameter("@IdTipoFactura", IdTipoFactura);
            await _context.Database.ExecuteSqlRawAsync("CrearFacturaCompra @FacturaCompra, @FechaFactura, @TotalCompra, @DetallesCompra, @IdTipoPago, @IdTipoFactura", FacturaCompraParam, FechaFacturaParam, TotalCompraParam, DetallesCompraParam, IdTipoPagoParam, IdTipoFacturaParam);
        }

        public async Task ActualizarFacturaCompra(string IdFacturaCompra, string FacturaCompra, DateTime FechaFactura, decimal TotalCompra, string DetallesCompra, int IdTipoPago, int IdTipoFactura)
        {
            var IdFacturaCompraParam = new SqlParameter("@IdFacturaCompra", IdFacturaCompra);
            var FacturaCompraParam = new SqlParameter("@FacturaCompra", FacturaCompra);
            var FechaFacturaParam = new SqlParameter("@FechaFactura", FechaFactura);
            var TotalCompraParam = new SqlParameter("@TotalCompra", TotalCompra);
            var DetallesCompraParam = new SqlParameter("@DetallesCompra", DetallesCompra);
            var IdTipoPagoParam = new SqlParameter("@IdTipoPago", IdTipoPago);
            var IdTipoFacturaParam = new SqlParameter("@IdTipoFactura", IdTipoFactura);
            await _context.Database.ExecuteSqlRawAsync("ActualizarFacturaCompra @IdFacturaCompra, @FacturaCompra, @FechaFactura, @TotalCompra, @DetallesCompra, @IdTipoPago, @IdTipoFactura", IdFacturaCompraParam, FacturaCompraParam, FechaFacturaParam, TotalCompraParam, DetallesCompraParam, IdTipoPagoParam, IdTipoFacturaParam);
        }

        public async Task EliminarFacturaCompra(string IdFacturaCompra)
        {
            var IdFacturaCompraParam = new SqlParameter("@IdFacturaCompra", IdFacturaCompra);
            await _context.Database.ExecuteSqlRawAsync("EliminarFacturaCompra @IdFacturaCompra", IdFacturaCompraParam);
        }

        public async Task<List<FacturaCompra>> MostrarFacturasCompras()
        {
            var FacturasCompras = await _context.FacturaCompra
                                        .FromSqlRaw("EXEC MostrarFacturasCompras")
                                        .ToListAsync();
            return FacturasCompras;
        }

        public async Task<FacturaCompra> ConsultarFacturasCompras(string IdFacturaCompra)
        {
            var IdFacturaCompraParam = new SqlParameter("@IdFacturaCompra", IdFacturaCompra);
            var resultado = await _context.FacturaCompra
                                          .FromSqlRaw("EXEC ConsultarFacturasCompras @IdFacturaCompra", IdFacturaCompraParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
