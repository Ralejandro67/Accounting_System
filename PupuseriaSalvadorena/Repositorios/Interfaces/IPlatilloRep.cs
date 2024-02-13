using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IPlatilloRep
    {
        Task CrearPlatillo(string NombrePlatillo, decimal CostoProduccion, decimal PrecioVenta);
        Task ActualizarPlatillo(int IdPlatillo, string NombrePlatillo, decimal CostoProduccion, decimal PrecioVenta);
        Task EliminarPlatillo(int IdPlatillo);
        Task<List<Platillo>> MostrarPlatillos();
        Task<Platillo> ConsultarPlatillos(int IdPlatillo);
    }
}
