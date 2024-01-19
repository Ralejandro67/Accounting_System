using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class CorreosRep : ICorreosRep
    {
        private readonly MiDbContext _context;

        public CorreosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearCorreo(string correo)
        {
            var correoParam = new SqlParameter("@Correo", correo);
            await _context.Database.ExecuteSqlRawAsync("CrearCorreo @Correo", correoParam);
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
