using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DistritosRep : IDistritosRep
    {
        private readonly MiDbContext _context;

        public DistritosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearDistrito(string NombreDistrito, int IdCanton)
        {
            var nombreParam = new SqlParameter("@NombreDistrito", NombreDistrito);
            var cantonParam = new SqlParameter("@IdCanton", IdCanton);
            await _context.Database.ExecuteSqlRawAsync("CrearDistrito @NombreDistrito, @IdCanton", nombreParam, cantonParam);
        }

        public async Task ActualizarDistrito(int IdDistrito, string NombreDistrito, int IdCanton)
        {
            var idDistritoParam = new SqlParameter("@IdDistrito", IdDistrito);
            var nombreDistritoParam = new SqlParameter("@NombreDistrito", NombreDistrito);
            var idCantonParam = new SqlParameter("@IdCanton", IdCanton);
            await _context.Database.ExecuteSqlRawAsync("ActualizarDistrito @IdDistrito, @NombreDistrito, @IdCanton", idDistritoParam, nombreDistritoParam, idCantonParam);
        }

        public async Task EliminarDistrito(int IdDistrito)
        {
            var idDistritoParam = new SqlParameter("@IdDistrito", IdDistrito);
            await _context.Database.ExecuteSqlRawAsync("EliminarDistrito @IdDistrito", idDistritoParam);
        }

        public async Task<List<Distrito>> MostrarDistritos()
        {
            var distritos = await _context.Distrito
                                          .FromSqlRaw("EXEC MostrarDistritos")
                                          .ToListAsync();
            return distritos;
        }

        public async Task<List<Distrito>> ConsultarDistritos(int IdDistrito)
        {
            var nombreParam = new SqlParameter("@IdDistrito", IdDistrito);
            var resultado = await _context.Distrito
                                          .FromSqlRaw("EXEC ConsultarDistritos @IdDistrito", nombreParam)
                                          .ToListAsync();

            return resultado;
        }

        public async Task<Distrito> ConsultarDistrito(int IdDistrito)
        {
            var nombreParam = new SqlParameter("@IdDistrito", IdDistrito);
            var resultado = await _context.Distrito
                                          .FromSqlRaw("EXEC ConsultarDistrito @IdDistrito", nombreParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
