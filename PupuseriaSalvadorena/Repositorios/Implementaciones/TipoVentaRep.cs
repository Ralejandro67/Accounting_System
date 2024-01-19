using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TipoVentaRep : ITipoVentaRep
    {
        private readonly MiDbContext _context;

        public TipoVentaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearTipoVenta(string NombreVenta, bool Estado)
        {
            var NombreVentaParam = new SqlParameter("@NombreVenta", NombreVenta);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("CrearTipoVenta @NombreVenta, @Estado", NombreVentaParam, EstadoParam);
        }

        public async Task ActualizarTipoVentas(int IdTipoVenta, string NombreVenta, bool Estado)
        {
            var IdTipoVentaParam = new SqlParameter("@IdTipoVenta", IdTipoVenta);
            var NombreVentaParam = new SqlParameter("@NombreVenta", NombreVenta);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("ActualizarTipoVentas @IdTipoVenta, @NombreVenta, @Estado", IdTipoVentaParam, NombreVentaParam, EstadoParam);
        }

        public async Task EliminarTipoVenta(int IdTipoVenta)
        {
            var IdTipoVentaParam = new SqlParameter("@IdTipoVenta", IdTipoVenta);
            await _context.Database.ExecuteSqlRawAsync("EliminarTipoVenta @IdTipoVenta", IdTipoVentaParam);
        }

        public async Task<List<TipoVenta>> MostrarTipoVentas()
        {
            var TipoVentas = await _context.TipoVenta
                                        .FromSqlRaw("EXEC MostrarTipoVentas")
                                        .ToListAsync();
            return TipoVentas;
        }

        public async Task<TipoVenta> ConsultarTipoVentas(int IdTipoVenta)
        {
            var NombreVentaParam = new SqlParameter("@IdTipoVenta", IdTipoVenta);
            var resultado = await _context.TipoVenta
                                          .FromSqlRaw("EXEC ConsultarTipoVentas @IdTipoVenta", NombreVentaParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
