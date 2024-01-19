using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class PlatilloRep : IPlatilloRep
    {
        private readonly MiDbContext _context;

        public PlatilloRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearPlatillo(string NombrePlatillo, decimal CostoProduccion, decimal PrecioVenta)
        {
            var NombrePlatilloParam = new SqlParameter("@NombrePlatillo", NombrePlatillo);
            var CostoProduccionParam = new SqlParameter("@CostoProduccion", CostoProduccion);
            var PrecioVentaParam = new SqlParameter("@PrecioVenta", PrecioVenta);
            await _context.Database.ExecuteSqlRawAsync("CrearPlatillo @NombrePlatillo, @CostoProduccion, @PrecioVenta", NombrePlatilloParam, CostoProduccionParam, PrecioVentaParam);
        }

        public async Task ActualizarPlatillo(string IdPlatillo, string NombrePlatillo, decimal CostoProduccion, decimal PrecioVenta)
        {
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var NombrePlatilloParam = new SqlParameter("@NombrePlatillo", NombrePlatillo);
            var CostoProduccionParam = new SqlParameter("@CostoProduccion", CostoProduccion);
            var PrecioVentaParam = new SqlParameter("@PrecioVenta", PrecioVenta);
            await _context.Database.ExecuteSqlRawAsync("ActualizarPlatillo @IdPlatillo, @NombrePlatillo, @CostoProduccion, @PrecioVenta", IdPlatilloParam, NombrePlatilloParam, CostoProduccionParam, PrecioVentaParam);
        }

        public async Task EliminarPlatillo(string IdPlatillo)
        {
            var IdPlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            await _context.Database.ExecuteSqlRawAsync("EliminarPlatillo @IdPlatillo", IdPlatilloParam);
        }

        public async Task<List<Platillo>> MostrarPlatillos()
        {
            var platillos = await _context.Platillo
                                        .FromSqlRaw("EXEC MostrarPlatillos")
                                        .ToListAsync();
            return platillos;
        }

        public async Task<Platillo> ConsultarPlatillos(string IdPlatillo)
        {
            var NombrePlatilloParam = new SqlParameter("@IdPlatillo", IdPlatillo);
            var resultado = await _context.Platillo
                                          .FromSqlRaw("EXEC ConsultarPlatillos @IdPlatillo", NombrePlatilloParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
