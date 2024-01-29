using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DetallesCuentaRep : IDetallesCuentaRep
    {
        private readonly MiDbContext _context;

        public DetallesCuentaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearDetallesCuenta(decimal Pago, DateTime FechaIngreso, string IdCuentaPagar)
        {
            var PagoParam = new SqlParameter("@Pago", Pago);
            var FechaIngresoParam = new SqlParameter("@FechaIngreso", FechaIngreso);
            var IdCuentaPagarParam = new SqlParameter("@IdCuentaPagar", IdCuentaPagar);
            await _context.Database.ExecuteSqlRawAsync("CrearDetallesCuenta @Pago, @FechaIngreso, @IdCuentaPagar", PagoParam, FechaIngresoParam, IdCuentaPagarParam);
        }

        public async Task ActualizarDetallesCuentas(string IdDetallesCuenta, decimal Pago, DateTime FechaIngreso)
        {
            var IdDetallesCuentaParam = new SqlParameter("@IdDetallesCuenta", IdDetallesCuenta);
            var PagoParam = new SqlParameter("@Pago", Pago);
            var FechaIngresoParam = new SqlParameter("@FechaIngreso", FechaIngreso);
            await _context.Database.ExecuteSqlRawAsync("ActualizarDetallesCuentas @IdDetallesCuenta, @Pago, @FechaIngreso", IdDetallesCuentaParam, PagoParam, FechaIngresoParam);
        }

        public async Task EliminarDetallesCuenta(string IdDetallesCuenta)
        {
            var IdDetallesCuentaParam = new SqlParameter("@IdDetallesCuenta", IdDetallesCuenta);
            await _context.Database.ExecuteSqlRawAsync("EliminarDetallesCuenta @IdDetallesCuenta", IdDetallesCuentaParam);
        }

        public async Task<List<DetalleCuenta>> MostrarDetallesCuenta()
        {
            var detallesCuenta = await _context.DetalleCuenta
                                        .FromSqlRaw("EXEC MostrarDetallesCuentas")
                                        .ToListAsync();
            return detallesCuenta;
        }

        public async Task<DetalleCuenta> ConsultarDetallesCuentas(string IdDetallesCuenta)
        {
            var IdDetallesCuentaParam = new SqlParameter("@IdDetallesCuenta", IdDetallesCuenta);
            var resultado = await _context.DetalleCuenta
                                          .FromSqlRaw("EXEC ConsultarDetallesCuentas @IdDetallesCuenta", IdDetallesCuentaParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<DetalleCuenta>> ConsultarCuentaDetalles(string IdCuentaPagar)
        {
            var IdCuentaPagarParam = new SqlParameter("@IdCuentaPagar", IdCuentaPagar);
            var resultado = await _context.DetalleCuenta
                                          .FromSqlRaw("EXEC ConsultarCuentaDetalles @IdCuentaPagar", IdCuentaPagarParam)
                                          .ToListAsync();

            return resultado;
        }
    }
}
