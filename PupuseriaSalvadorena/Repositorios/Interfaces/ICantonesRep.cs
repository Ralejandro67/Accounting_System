using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ICantonesRep
    {
        Task<List<Canton>> ConsultarCantones(int id);
    }
}
