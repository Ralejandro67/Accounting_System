using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TelefonosRep : ITelefonosRep
    {
        private readonly MiDbContext _context;

        public TelefonosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearTelefono(int numero, bool estado)
        {
            var numeroParam = new SqlParameter("@Telefono", numero);
            var estadoParam = new SqlParameter("@Estado", estado);
            await _context.Database.ExecuteSqlRawAsync("CrearTelefono @Telefono, @Estado", numeroParam, estadoParam);
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
            var idTelefonoParam = new SqlParameter("@IdTelefono", IdTelefono);
            await _context.Database.ExecuteSqlRawAsync("EliminarTelefono @IdTelefono", idTelefonoParam);
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
