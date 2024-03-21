using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IRolRep
    {
        Task<List<Rol>> MostrarRoles();
    }
}
