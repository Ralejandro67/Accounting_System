using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class CantonesRep : ICantonesRep
    {
        private readonly MiDbContext _context;

        public CantonesRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearCanton(string nombre, int provincia)
        {
            var nombreParam = new SqlParameter("@NombreCanton", nombre);
            var provinciaParam = new SqlParameter("@IdProvincia", provincia);
            await _context.Database.ExecuteSqlRawAsync("CrearCanton @NombreCanton, @IdProvincia", nombreParam, provinciaParam);
        }

        public async Task ActualizarCanton(int id, string nombre, int provincia)
        {
            var idCantonParam = new SqlParameter("@IdCanton", id);
            var nombreParam = new SqlParameter("@NombreCanton", nombre);
            var provinciaParam = new SqlParameter("@IdProvincia", provincia);
            await _context.Database.ExecuteSqlRawAsync("ActualizarCanton @IdCanton, @NombreCanton, IdProvincia", idCantonParam, nombreParam, provinciaParam);
        }

        public async Task EliminarCanton(int id)
        {
            var idCantonParam = new SqlParameter("@IdCanton", id);
            await _context.Database.ExecuteSqlRawAsync("EliminarCanton @IdCanton", idCantonParam);
        }

        public async Task<List<Canton>> MostrarCantones()
        {
            var cantones = await _context.Canton
                                        .FromSqlRaw("EXEC MostrarCantones")
                                        .ToListAsync();
            return cantones;
        }

        public async Task<List<Canton>> ConsultarCantones(int id)
        {
            var nombreParam = new SqlParameter("@IdCanton", id);
            var cantones = await _context.Canton
                                        .FromSqlRaw("EXEC ConsultarCantones @IdCanton", nombreParam)
                                        .ToListAsync();
            return cantones;
        }

        public async Task<Canton> ConsultarCanton(int id)
        {
            var nombreParam = new SqlParameter("@IdCanton", id);
            var resultado = await _context.Canton
                                          .FromSqlRaw("EXEC ConsultarCanton @IdCanton", nombreParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
