using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TelefonosRep : ITelefonosRep
    {
        private readonly MiDbContext _context;

        public TelefonosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearTelefono(int Telefono, bool Estado)
        {
            using(var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CrearTelefono";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Telefono", Telefono));
                command.Parameters.Add(new SqlParameter("@Estado", Estado));

                var IdTelefono = new SqlParameter("@IdTelefono", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdTelefono);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (int)IdTelefono.Value;
            }
        }

        public async Task ActualizarTelefono(int IdTelefono, int Telefono, bool Estado)
        {
            var idTelefonoParam = new SqlParameter("@IdTelefono", IdTelefono);
            var telefonoParam = new SqlParameter("@Telefono", Telefono);
            var estadoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("ActualizarTelefono @IdTelefono, @Telefono, @Estado", idTelefonoParam, telefonoParam, estadoParam);
        }

        public async Task EliminarTelefono(int IdTelefono)
        {
            try
            {
                var idTelefonoParam = new SqlParameter("@IdTelefono", IdTelefono);
                await _context.Database.ExecuteSqlRawAsync("EliminarTelefono @IdTelefono", idTelefonoParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<Telefonos>> MostrarTelefonos()
        {
            var telefonos = await _context.Telefonos
                                        .FromSqlRaw("EXEC MostrarTelefonos")
                                        .ToListAsync();
            return telefonos;
        }

        public async Task<Telefonos> ConsultarTelefonos(int IdTelefono)
        {
            var telefonoParam = new SqlParameter("@IdTelefono", IdTelefono);
            var resultado = await _context.Telefonos
                                          .FromSqlRaw("EXEC ConsultarTelefonos @IdTelefono", telefonoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
