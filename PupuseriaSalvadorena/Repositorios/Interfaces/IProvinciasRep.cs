using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IProvinciasRep
    {
        Task CrearProvincia(string nombre);
        Task ActualizarProvincia(int id, string nombre);
        Task EliminarProvincia(int id);
        Task<List<Provincia>> MostrarProvincias();
        Task<Provincia> ConsultarProvincias(int id);
    }
}
