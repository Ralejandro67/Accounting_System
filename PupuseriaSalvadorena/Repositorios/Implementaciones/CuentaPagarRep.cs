using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class CuentaPagarRep : ICuentaPagarRep
    {
        private readonly MiDbContext _context;

        public CuentaPagarRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearCuentaPagar(DateTime FechaCreacion, DateTime FechaVencimiento, decimal TotalPagado, string IdFacturaCompra, string IdProveedor)
        {
            var FechaCreacionParam = new SqlParameter("@FechaCreacion", FechaCreacion);
            var FechaVencimientoParam = new SqlParameter("@FechaVencimiento", FechaVencimiento);
            var TotalPagadoParam = new SqlParameter("@TotalPagado", TotalPagado);
            var IdFacturaCompraParam = new SqlParameter("@IdFacturaCompra", IdFacturaCompra);
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            await _context.Database.ExecuteSqlRawAsync("CrearCuentaPagar @FechaCreacion, @FechaVencimiento, @TotalPagado, @IdFacturaCompra, @IdProveedor", FechaCreacionParam, FechaVencimientoParam, TotalPagadoParam, IdFacturaCompraParam, IdProveedorParam);
        }

        public async Task ActualizarCuentaPagar(string IdCuentaPagar, decimal TotalPagado, string IdFacturaCompra, string IdProveedor)
        {
            var IdCuentaPagarParam = new SqlParameter("@IdCuentaPagar", IdCuentaPagar);
            var TotalPagadoParam = new SqlParameter("@TotalPagado", TotalPagado);
            var IdFacturaCompraParam = new SqlParameter("@IdFacturaCompra", IdFacturaCompra);
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            await _context.Database.ExecuteSqlRawAsync("ActualizarCuentaPagar @IdCuentaPagar, @TotalPagado, @IdFacturaCompra, @IdProveedor", IdCuentaPagarParam, TotalPagadoParam, IdFacturaCompraParam, IdProveedorParam);
        }

        public async Task EliminarCuentaPagar(string IdCuentaPagar)
        {
            var IdCuentaPagarParam = new SqlParameter("@IdCuentaPagar", IdCuentaPagar);
            await _context.Database.ExecuteSqlRawAsync("EliminarCuentaPagar @IdCuentaPagar", IdCuentaPagarParam);
        }

        public async Task<List<CuentaPagar>> MostrarCuentasPagar()
        {
            var cuentasPagar = await _context.CuentaPagar
                                            .FromSqlRaw("EXEC MostrarCuentasPagar")
                                            .ToListAsync();
            return cuentasPagar;
        }

        public async Task<CuentaPagar> ConsultarCuentasPagar(string NombreProveedor)
        {
            var NombreProveedorParam = new SqlParameter("@NombreProveedor", NombreProveedor);
            var resultado = await _context.CuentaPagar
                                          .FromSqlRaw("EXEC ConsultarCuentasPagar @NombreProveedor", NombreProveedorParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
