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

        public async Task CrearDistrito(string nombre, int canton)
        {
            var nombreParam = new SqlParameter("@NombreDistrito", nombre);
            var cantonParam = new SqlParameter("@IdCanton", canton);
            await _context.Database.ExecuteSqlRawAsync("CrearCanton @NombreDistrito, @IdCanton", nombreParam, cantonParam);
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

        public async Task<Distrito> ConsultarDistritos(int IdDistrito)
        {
            var nombreParam = new SqlParameter("@IdDistrito", IdDistrito);
            var resultado = await _context.Distrito
                                          .FromSqlRaw("EXEC ConsultarDistritos @IdDistrito", nombreParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
