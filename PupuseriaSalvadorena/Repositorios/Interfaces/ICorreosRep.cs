using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ICorreosRep
    {
        Task CrearCorreo(string correo);
        Task ActualizarCorreo(int id, string correo);
        Task EliminarCorreo(int id);
        Task<List<CorreoElectronico>> MostrarCorreos();
        Task<CorreoElectronico> ConsultarCorreos(int id);
    }
}
