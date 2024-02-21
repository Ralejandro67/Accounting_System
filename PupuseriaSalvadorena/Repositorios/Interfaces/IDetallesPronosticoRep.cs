using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDetallesPronosticoRep
    {
        Task CrearDetallesPronostico(int IdPronostico, DateTime FechaPronostico, int PCantVenta, decimal PValorVenta);
        Task EliminarDetallesPronostico(int IdDetallePronostico);
        Task<List<DetallesPronostico>> MostrarDetallesPronosticos();
        Task<DetallesPronostico> ConsultarDetallesPronosticos(int IdDetallePronostico);
        Task<List<DetallesPronostico>> ConsultarDetallesPorPronosticos(int IdPronostico);
    }
}
