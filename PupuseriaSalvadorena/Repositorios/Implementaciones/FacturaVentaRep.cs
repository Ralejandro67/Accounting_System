using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class FacturaVentaRep : IFacturaVentaRep
    {
        private readonly MiDbContext _context;

        public FacturaVentaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearFacturaVenta(long CedulaJuridica, int Consecutivo, int Clave, DateTime FechaFactura, decimal SubTotal, decimal TotalVenta, int IdTipoPago, int IdTipoFactura)
        {
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            var ConsecutivoParam = new SqlParameter("@Consecutivo", Consecutivo);
            var ClaveParam = new SqlParameter("@Clave", Clave);
            var FechaFacturaParam = new SqlParameter("@FechaFactura", FechaFactura);
            var SubTotalParam = new SqlParameter("@SubTotal", SubTotal);
            var TotalVentaParam = new SqlParameter("@TotalVenta", TotalVenta);
            var IdTipoPagoParam = new SqlParameter("@IdTipoPago", IdTipoPago);
            var IdTipoFacturaParam = new SqlParameter("@IdTipoFactura", IdTipoFactura);
            await _context.Database.ExecuteSqlRawAsync("CrearFacturaVenta @CedulaJuridica, @Consecutivo, @Clave, @FechaFactura, @SubTotal, @TotalVenta, @IdTipoPago, @IdTipoFactura", CedulaJuridicaParam, ConsecutivoParam, ClaveParam, FechaFacturaParam, SubTotalParam, TotalVentaParam, IdTipoPagoParam, IdTipoFacturaParam);
        }

        public async Task ActualizarFacturaVenta(int IdFacturaVenta, int IdTipoPago, int IdTipoFactura)
        {
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var IdTipoPagoParam = new SqlParameter("@IdTipoPago", IdTipoPago);
            var IdTipoFacturaParam = new SqlParameter("@IdTipoFactura", IdTipoFactura);
            await _context.Database.ExecuteSqlRawAsync("ActualizarFacturaVenta @IdFacturaVenta, @IdTipoPago, @IdTipoFactura", IdFacturaVentaParam, IdTipoPagoParam, IdTipoFacturaParam);
        }

        public async Task EliminarFacturaVenta(int IdFacturaVenta)
        {
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            await _context.Database.ExecuteSqlRawAsync("EliminarFacturaVenta @IdFacturaVenta", IdFacturaVentaParam);
        }
        public async Task<List<FacturaVenta>> MostrarFacturasVentas()
        {
            var FacturasVentas = await _context.FacturaVenta
                                        .FromSqlRaw("EXEC MostrarFacturasVentas")
                                        .ToListAsync();
            return FacturasVentas;
        }

        public async Task<FacturaVenta> ConsultarFacturasVentas(int IdFacturaVenta)
        {
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var resultado = await _context.FacturaVenta
                                          .FromSqlRaw("EXEC ConsultarFacturasVentas @IdFacturaVenta", IdFacturaVentaParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
