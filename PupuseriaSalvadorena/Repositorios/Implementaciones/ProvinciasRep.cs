using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class ProvinciasRep : IProvinciasRep
    {
        private readonly MiDbContext _context;

        public ProvinciasRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearProvincia(string nombre)
        {
            var nombreParam = new SqlParameter("@NombreProvincia", nombre);
            await _context.Database.ExecuteSqlRawAsync("CrearProvincia @NombreProvincia", nombreParam);
        }

        public async Task ActualizarProvincia(int id, string nombre)
        {
            var idProvinciaParam = new SqlParameter("@IdProvincia", id);
            var nombreParam = new SqlParameter("@NombreProvincia", nombre);
            await _context.Database.ExecuteSqlRawAsync("ActualizarProvincia @IdProvincia, @NombreProvincia", idProvinciaParam, nombreParam);
        }

        public async Task EliminarProvincia(int id)
        {
            var idProvinciaParam = new SqlParameter("@IdProvincia", id);
            await _context.Database.ExecuteSqlRawAsync("EliminarProvincia @IdProvincia", idProvinciaParam);
        }

        public async Task<List<Provincia>> MostrarProvincias()
        {
            var provincias = await _context.Provincia
                                          .FromSqlRaw("EXEC MostrarProvincias")
                                          .ToListAsync();
            return provincias;
        }

        public async Task<Provincia> ConsultarProvincias(int id)
        {
            var nombreParam = new SqlParameter("@IdProvincia", id);
            var resultado = await _context.Provincia
                                          .FromSqlRaw("EXEC ConsultarProvincias @IdProvincia", nombreParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
