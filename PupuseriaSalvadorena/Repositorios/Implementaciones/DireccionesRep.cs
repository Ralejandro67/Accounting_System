using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DireccionesRep : IDireccionesRep
    {
        private readonly MiDbContext _context;

        public DireccionesRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearDireccion(bool estado, string nombre, int distrito)
        {
            var estadoParam = new SqlParameter("@Estado", estado);
            var nombreParam = new SqlParameter("@Detalles", nombre);
            var distritoParam = new SqlParameter("@IdDistrito", distrito);
            await _context.Database.ExecuteSqlRawAsync("CrearDireccion @Estado, @Detalles, @IdDistrito", estadoParam, nombreParam, distritoParam);
        }

        public async Task ActualizarDireccion(int IdDireccion, bool Estado, string Detalles, int IdDistrito)
        {
            var idDireccionParam = new SqlParameter("@IdDireccion", IdDireccion);
            var estadoParam = new SqlParameter("@Estado", Estado);
            var detallesParam = new SqlParameter("@Detalles", Detalles);
            var idDistritoParam = new SqlParameter("@IdDistrito", IdDistrito);
            await _context.Database.ExecuteSqlRawAsync("ActualizarDireccion @IdDireccion, @Estado, @Detalles, @IdDistrito", idDireccionParam, estadoParam, detallesParam, idDistritoParam);
        }

        public async Task EliminarDireccion(int IdDireccion)
        {
            var idDireccionParam = new SqlParameter("@IdDireccion", IdDireccion);
            await _context.Database.ExecuteSqlRawAsync("EliminarDireccion @IdDireccion", idDireccionParam);
        }

        public async Task<List<Direccion>> MostrarDirecciones()
        {
            var direcciones = await _context.Direccion
                                        .FromSqlRaw("EXEC MostrarDirecciones")
                                        .ToListAsync();
            return direcciones;
        }

        public async Task<Direccion> ConsultarDirecciones(int IdDireccion)
        {
            var idDireccionParam = new SqlParameter("@IdDireccion", IdDireccion);
            var resultado = await _context.Direccion
                                          .FromSqlRaw("EXEC ConsultarDirecciones @IdDireccion", idDireccionParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
