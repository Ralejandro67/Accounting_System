using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDistritosRep
    {
        Task<List<Distrito>> ConsultarDistritos(int IdDistrito);
    }
}
