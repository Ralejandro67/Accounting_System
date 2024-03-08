using DocumentFormat.OpenXml.Presentation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class CorreosRep : ICorreosRep
    {
        private readonly MiDbContext _context;

        public CorreosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearCorreo(string correo)
        {
            using(var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CrearCorreo";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Correo", correo));

                var IdCorreoElectronico = new SqlParameter("@IdCorreoElectronico", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdCorreoElectronico);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (int)IdCorreoElectronico.Value;
            }   
        }

        public async Task ActualizarCorreo(int id, string correo)
        {
            var idCorreoParam = new SqlParameter("@IdCorreoElectronico", id);
            var correoParam = new SqlParameter("@Correo", correo);
            await _context.Database.ExecuteSqlRawAsync("ActualizarCorreo @IdCorreoElectronico, @Correo", idCorreoParam, correoParam);
        }

        public async Task EliminarCorreo(int id)
        {
            var idCorreoParam = new SqlParameter("@IdCorreoElectronico", id);
            await _context.Database.ExecuteSqlRawAsync("EliminarCorreo @IdCorreoElectronico", idCorreoParam);
        }

        public async Task<List<CorreoElectronico>> MostrarCorreos()
        {
            var correos = await _context.CorreoElectronico
                                        .FromSqlRaw("EXEC MostrarCorreos")
                                        .ToListAsync();
            return correos;
        }

        public async Task<CorreoElectronico> ConsultarCorreos(int id)
        {
            var idCorreoParam = new SqlParameter("@IdCorreoElectronico", id);
            var resultado = await _context.CorreoElectronico
                                          .FromSqlRaw("EXEC ConsultarCorreos @IdCorreoElectronico", idCorreoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
