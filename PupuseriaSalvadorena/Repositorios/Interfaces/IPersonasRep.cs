using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IPersonasRep
    {
        Task CrearPersona(int Cedula, string Nombre, string Apellido, DateTime FechaNac, int Correo, int IdDireccion, int IdTelefono);
        Task ActualizarPersona(string IdPersona, string Nombre, string Apellido);
        Task EliminarPersona(string IdPersona);
        Task<List<Persona>> MostrarPersonas();
        Task<Persona> ConsultarPersonas(string IdPersona);
    }
}
