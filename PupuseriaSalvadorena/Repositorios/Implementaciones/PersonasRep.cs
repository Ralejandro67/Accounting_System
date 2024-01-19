using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class PersonasRep : IPersonasRep
    {
        private readonly MiDbContext _context;

        public PersonasRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearPersona(int Cedula, string nombre, string apellido, DateTime FechaNac, int correo, int direccion, int telefono)
        {
            var cedulaParam = new SqlParameter("@Cedula", Cedula);
            var nombreParam = new SqlParameter("@Nombre", nombre);
            var apellidoParam = new SqlParameter("@Apellido", apellido);
            var fechaNacParam = new SqlParameter("@FechaNac", FechaNac);
            var correoParam = new SqlParameter("@IdCorreoElectronico", correo);
            var telefonoParam = new SqlParameter("@IdTelefono", telefono);
            var direccionParam = new SqlParameter("@IdDireccion", direccion);
            await _context.Database.ExecuteSqlRawAsync("CrearPersona @Cedula, @Nombre, @Apellido, @FechaNac, @IdCorreoElectronico, @IdDireccion, @IdTelefono", cedulaParam, nombreParam, apellidoParam, fechaNacParam, correoParam, direccionParam, telefonoParam);
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

        public async Task<List<Persona>> MostrarPersonas()
        {
            var personas = await _context.Persona
                                        .FromSqlRaw("EXEC MostrarPersonas")
                                        .ToListAsync();
            return personas;
        }

        public async Task<Persona> ConsultarPersonas(string IdPersona)
        {
            var IdPersonaParam = new SqlParameter("@IdPersona", IdPersona);
            var resultado = await _context.Persona
                                          .FromSqlRaw("EXEC ConsultarPersonas @IdPersona", IdPersonaParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
