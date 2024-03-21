using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ICorreosRep
    {
        Task<int> CrearCorreo(string correo);
        Task EliminarCorreo(int id);
        Task<List<CorreoElectronico>> MostrarCorreos();
    }
}
