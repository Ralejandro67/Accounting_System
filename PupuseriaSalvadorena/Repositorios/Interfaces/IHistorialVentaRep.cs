using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IHistorialVentaRep
    {
        Task CrearHistorialVenta(int IdVenta, int CantVenta, DateTime FechaVenta, int IdPlatillo, int IdFacturaVenta, int IdTipoVenta);
        Task ActualizarHistorialVenta(int IdVenta, int CantVenta, int IdPlatillo, int IdFacturaVenta, int IdTipoVenta);
        Task EliminarHistorialVenta(int IdVenta);
        Task<List<HistorialVenta>> MostrarHistorialVenta();
        Task<HistorialVenta> ConsultarHistorialVentas(int IdVenta);
        Task<List<HistorialVenta>> ConsultarHistorialVentasPlatillos(int IdPlatillo);
        Task<HistorialVenta> ConsultarHistorialVentasFactura(int IdFacturaVenta);
    }
}
