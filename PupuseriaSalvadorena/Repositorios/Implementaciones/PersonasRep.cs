using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class PersonasRep : IPersonasRep
    {
        private readonly MiDbContext _context;

        public PersonasRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<string> CrearPersona(long Cedula, string nombre, string apellido, DateTime FechaNac, int correo, int direccion, int telefono)
        {
            using(var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CrearPersona";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Cedula", Cedula));
                command.Parameters.Add(new SqlParameter("@Nombre", nombre));
                command.Parameters.Add(new SqlParameter("@Apellido", apellido));
                command.Parameters.Add(new SqlParameter("@FechaNac", FechaNac));
                command.Parameters.Add(new SqlParameter("@IdCorreoElectronico", correo));
                command.Parameters.Add(new SqlParameter("@IdDireccion", direccion));
                command.Parameters.Add(new SqlParameter("@IdTelefono", telefono));

                var IdPersona = new SqlParameter("@IdPersona", SqlDbType.VarChar, 10)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdPersona);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (string)IdPersona.Value;
            }
        }

        public async Task ActualizarPersona(string IdPersona, string Nombre, string Apellido)
        {
            var IdPersonaParam = new SqlParameter("@IdPersona", IdPersona);
            var NombreParam = new SqlParameter("@Nombre", Nombre);
            var ApellidoParam = new SqlParameter("@Apellido", Apellido);
            await _context.Database.ExecuteSqlRawAsync("ActualizarPersona @IdPersona, @Nombre, @Apellido", IdPersonaParam, NombreParam, ApellidoParam);
        }

        public async Task EliminarPersona(string IdPersona)
        {
            var IdPersonaParam = new SqlParameter("@IdPersona", IdPersona);
            await _context.Database.ExecuteSqlRawAsync("EliminarPersona @IdPersona", IdPersonaParam);
        }

        public async Task<Persona> ConsultarPersonas(string IdPersona)
        {
            try
            {
                var IdPersonaParam = new SqlParameter("@IdPersona", IdPersona);
                var resultado = await _context.Persona
                                              .FromSqlRaw("EXEC ConsultarPersonas @IdPersona", IdPersonaParam)
                                              .ToListAsync();

                return resultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error al consultar la persona: {ex.Message}");
                throw ex;
            }
        }
    }
}
