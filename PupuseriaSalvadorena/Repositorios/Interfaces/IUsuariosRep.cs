using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IUsuariosRep
    {
        Task CrearUsuario(string Contrasena, bool Estado, DateTime FechaCreacion, string IdPersona, int IdRol);
        Task ActualizarUsuario(string IdUsuario, string Contrasena, bool Estado, int IdRol);
        Task EliminarUsuario(string IdUsuario);
        Task<List<Usuario>> MostrarUsuarios();
        Task<Usuario> ConsultarUsuarios(string IdUsuario);
    }
}
