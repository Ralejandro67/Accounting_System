using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IRolRep
    {
        Task CrearRol(string nombre, bool activo);
        Task ActualizarRol(int IdRol, string NombreRol, bool Activo);
        Task EliminarRol(int IdRol);
        Task<List<Rol>> MostrarRoles();
        Task<Rol> ConsultarRoles(int IdRol);
    }
}
