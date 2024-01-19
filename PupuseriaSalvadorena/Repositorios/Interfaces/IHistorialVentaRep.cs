using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IHistorialVentaRep
    {
        Task CrearHistorialVenta(int CantVenta, DateTime FechaVenta, string IdPlatillo, int IdFacturaVenta, int IdTipoVenta);
        Task ActualizarHistorialVenta(string IdVenta, int CantVenta, string IdPlatillo, int IdFacturaVenta, int IdTipoVenta);
        Task EliminarHistorialVenta(string IdVenta);
        Task<List<HistorialVenta>> MostrarHistorialVenta();
        Task<HistorialVenta> ConsultarHistorialVentas(string IdVenta);
    }
}
