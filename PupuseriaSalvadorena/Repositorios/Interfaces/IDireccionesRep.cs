using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDireccionesRep
    {
        Task<int> CrearDireccion(bool Estado, string Detalles, int IdDistrito);
        Task ActualizarDireccion(int IdDireccion, bool Estado, string Detalles, int IdDistrito);
        Task EliminarDireccion(int IdDireccion);
    }
}
