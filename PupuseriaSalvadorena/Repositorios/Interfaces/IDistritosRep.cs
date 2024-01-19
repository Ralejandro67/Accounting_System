using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDistritosRep
    {
        Task CrearDistrito(string nombre, int canton);
        Task ActualizarDistrito(int IdDistrito, string NombreDistrito, int IdCanton);
        Task EliminarDistrito(int IdDistrito);
        Task<List<Distrito>> MostrarDistritos();
        Task<Distrito> ConsultarDistritos(int IdDistrito);
    }
}
