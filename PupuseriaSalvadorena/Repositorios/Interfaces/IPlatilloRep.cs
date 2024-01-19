using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IPlatilloRep
    {
        Task CrearPlatillo(string NombrePlatillo, decimal CostoProduccion, decimal PrecioVenta);
        Task ActualizarPlatillo(string IdPlatillo, string NombrePlatillo, decimal CostoProduccion, decimal PrecioVenta);
        Task EliminarPlatillo(string IdPlatillo);
        Task<List<Platillo>> MostrarPlatillos();
        Task<Platillo> ConsultarPlatillos(string IdPlatillo);
    }
}
