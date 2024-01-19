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

        public async Task CrearEnvioFactura(DateTime FechaEnvio, int IdFacturaVenta)
        {
            var FechaEnvioParam = new SqlParameter("@FechaEnvio", FechaEnvio);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            await _context.Database.ExecuteSqlRawAsync("CrearEnvioFactura @FechaEnvio, @IdFacturaVenta", FechaEnvioParam, IdFacturaVentaParam);
        }

        public async Task ActualizarEnvioFactura(int IdEnvioFactura, DateTime FechaEnvio, int IdFacturaVenta)
        {
            var IdEnvioFacturaParam = new SqlParameter("@IdEnvioFactura", IdEnvioFactura);
            var FechaEnvioParam = new SqlParameter("@FechaEnvio", FechaEnvio);
            var IdFacturaVentaParam = new SqlParameter("@IdFacturaVenta", IdFacturaVenta);
            await _context.Database.ExecuteSqlRawAsync("ActualizarEnvioFactura @IdEnvioFactura, @FechaEnvio, @IdFacturaVenta", IdEnvioFacturaParam, FechaEnvioParam, IdFacturaVentaParam);
        }

        public async Task EliminarEnvioFactura(int IdEnvioFactura)
        {
            var IdEnvioFacturaParam = new SqlParameter("@IdEnvioFactura", IdEnvioFactura);
            await _context.Database.ExecuteSqlRawAsync("EliminarEnvioFactura @IdEnvioFactura", IdEnvioFacturaParam);
        }

        public async Task<List<EnvioFactura>> MostrarEnvioFactura()
        {
            var envioFacturas = await _context.EnvioFactura
                                              .FromSqlRaw("EXEC MostrarEnvioFactura")
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
    }
}
