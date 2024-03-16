using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class EnvioFacturaRep : IEnvioFacturaRep
    {
        private readonly MiDbContext _context;

        public EnvioFacturaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearEnvioFactura(DateTime FechaEnvio, int IdFacturaVenta, long Identificacion, string NombreCliente, string CorreoElectronico, int Telefono)
        {
            var FechaEnvioParam = new SqlParameter("@FechaEnvio", FechaEnvio);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var IdentificacionParam = new SqlParameter("@Identificacion", Identificacion);
            var NombreClienteParam = new SqlParameter("@NombreCliente", NombreCliente);
            var CorreoElectronicoParam = new SqlParameter("@CorreoElectronico", CorreoElectronico);
            var TelefonoParam = new SqlParameter("@Telefono", Telefono);
            await _context.Database.ExecuteSqlRawAsync("CrearEnvioFactura @FechaEnvio, @IdFacturaVenta, @Identificacion, @NombreCliente, @CorreoElectronico, @Telefono", FechaEnvioParam, IdFacturaVentaParam, IdentificacionParam, NombreClienteParam, CorreoElectronicoParam, TelefonoParam);
        }

        public async Task ActualizarEnvioFactura(int IdEnvioFactura, DateTime FechaEnvio, int IdFacturaVenta, long Identificacion, string NombreCliente, string CorreoElectronico, int Telefono)
        {
            var IdEnvioFacturaParam = new SqlParameter("@IdEnvioFactura", IdEnvioFactura);
            var FechaEnvioParam = new SqlParameter("@FechaEnvio", FechaEnvio);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var IdentificacionParam = new SqlParameter("@Identificacion", Identificacion);
            var NombreClienteParam = new SqlParameter("@NombreCliente", NombreCliente);
            var CorreoElectronicoParam = new SqlParameter("@CorreoElectronico", CorreoElectronico);
            var TelefonoParam = new SqlParameter("@Telefono", Telefono);
            await _context.Database.ExecuteSqlRawAsync("ActualizarEnvioFactura @IdEnvioFactura, @FechaEnvio, @IdFacturaVenta, @Identificacion, @NombreCliente, @CorreoElectronico, @Telefono", IdEnvioFacturaParam, FechaEnvioParam, IdFacturaVentaParam, IdentificacionParam, NombreClienteParam, CorreoElectronicoParam, TelefonoParam);
        }

        public async Task EliminarEnvioFactura(int IdEnvioFactura)
        {
            var IdEnvioFacturaParam = new SqlParameter("@IdEnvioFactura", IdEnvioFactura);
            await _context.Database.ExecuteSqlRawAsync("EliminarEnvioFactura @IdEnvioFactura", IdEnvioFacturaParam);
        }

        public async Task<List<EnvioFactura>> MostrarEnvioFactura()
        {
            var envioFacturas = await _context.EnvioFactura
                                              .FromSqlRaw("EXEC MostrarEnvioFacturas")
                                              .ToListAsync();
            return envioFacturas;
        }

        public async Task<EnvioFactura> ConsultarEnvioFacturas(int IdEnvioFactura)
        {
            var IdEnvioFacturaParam = new SqlParameter("@IdEnvioFactura", IdEnvioFactura);
            var resultado = await _context.EnvioFactura
                                            .FromSqlRaw("EXEC ConsultarEnvioFacturas @IdEnvioFactura", IdEnvioFacturaParam)
                                            .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<EnvioFactura> ConsultarEnvioFacturasPorID(int IdFacturaVenta)
        {
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            var resultado = await _context.EnvioFactura
                                            .FromSqlRaw("EXEC ConsultarEnvioFacturasPorID @IdFacturaVenta", IdFacturaVentaParam)
                                            .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
