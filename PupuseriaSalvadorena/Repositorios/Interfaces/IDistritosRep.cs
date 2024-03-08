using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDistritosRep
    {
        Task CrearDistrito(string NombreDistrito, int IdCanton);
        Task ActualizarDistrito(int IdDistrito, string NombreDistrito, int IdCanton);
        Task EliminarDistrito(int IdDistrito);
        Task<List<Distrito>> MostrarDistritos();
        Task<List<Distrito>> ConsultarDistritos(int IdDistrito);
        Task<Distrito> ConsultarDistrito(int IdDistrito);
    }
}
