using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IProvinciasRep
    {
        Task<List<Provincia>> MostrarProvincias();
    }
}
