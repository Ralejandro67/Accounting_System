using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDetallesPronosticoRep
    {
        Task CrearDetallesPronostico(int IdPronostico, DateTime FechaPronostico, int PCantVenta, decimal PValorVenta);
        Task EliminarDetallesPronostico(int IdPronostico);
        Task<List<DetallesPronostico>> ConsultarDetallesPorPronosticos(int IdPronostico);
    }
}
