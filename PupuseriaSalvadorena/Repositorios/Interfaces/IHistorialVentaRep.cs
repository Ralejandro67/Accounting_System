using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IHistorialVentaRep
    {
        Task CrearHistorialVenta(int IdVenta, int CantVenta, DateTime FechaVenta, int IdPlatillo, int IdFacturaVenta, int IdTipoVenta);
        Task<List<HistorialVenta>> MostrarHistorialVenta();
        Task<List<HistorialVenta>> ConsultarHistorialVentasPlatillos(int IdPlatillo);
        Task<HistorialVenta> ConsultarHistorialVentasFactura(int IdFacturaVenta);
        Task<List<HistorialVenta>> ConsultarDetallesVentas(int IdFacturaVenta);
    }
}
