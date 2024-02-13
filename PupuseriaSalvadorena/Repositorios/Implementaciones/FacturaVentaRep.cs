using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class FacturaVentaRep : IFacturaVentaRep
    {
        private readonly MiDbContext _context;

        public FacturaVentaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearFacturaVenta(long CedulaJuridica, decimal Consecutivo, DateTime FechaFactura, decimal SubTotal, decimal TotalVenta, int IdTipoPago, int IdTipoFactura)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CrearFacturaVenta";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@CedulaJuridica", CedulaJuridica));
                command.Parameters.Add(new SqlParameter("@Consecutivo", Consecutivo));
                command.Parameters.Add(new SqlParameter("@FechaFactura", FechaFactura));
                command.Parameters.Add(new SqlParameter("@SubTotal", SubTotal));
                command.Parameters.Add(new SqlParameter("@TotalVenta", TotalVenta));
                command.Parameters.Add(new SqlParameter("@IdTipoPago", IdTipoPago));
                command.Parameters.Add(new SqlParameter("@IdTipoFactura", IdTipoFactura));
                
                var IdFactura = new SqlParameter("@IdFactura", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdFactura);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (int)IdFactura.Value;
            }
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
