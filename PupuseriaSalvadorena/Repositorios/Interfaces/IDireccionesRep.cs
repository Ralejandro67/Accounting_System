using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDireccionesRep
    {
        Task CrearDireccion(bool estado, string nombre, int distrito);
        Task ActualizarDireccion(int IdDireccion, bool Estado, string Detalles, int IdDistrito);
        Task EliminarDireccion(int IdDireccion);
        Task<List<Direccion>> MostrarDirecciones();
        Task<Direccion> ConsultarDirecciones(int IdDireccion);
    }
}
