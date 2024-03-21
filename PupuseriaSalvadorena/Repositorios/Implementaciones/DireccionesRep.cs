using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DireccionesRep : IDireccionesRep
    {
        private readonly MiDbContext _context;

        public DireccionesRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearDireccion(bool Estado, string Detalles, int IdDistrito)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "CrearDireccion";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Estado", Estado));
                    command.Parameters.Add(new SqlParameter("@Detalles", Detalles));
                    command.Parameters.Add(new SqlParameter("@IdDistrito", IdDistrito));

                    var IdDireccion = new SqlParameter("@IdDireccion", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(IdDireccion);

                    await _context.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();

                    return (int)IdDireccion.Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
    }
}
