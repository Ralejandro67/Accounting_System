using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IFacturaVentaRep
    {
        Task<int> CrearFacturaVenta(long CedulaJuridica, decimal Consecutivo, DateTime FechaFactura, decimal SubTotal, decimal TotalVenta, int IdTipoPago, int IdTipoFactura, bool Estado);
        Task ActualizarFacturaVenta(int IdFacturaVenta, bool Estado);
        Task EliminarFacturaVenta(int IdFacturaVenta);
        Task<List<FacturaVenta>> MostrarFacturasVentas();
        Task<FacturaVenta> ConsultarFacturasVentas(int IdFacturaVenta);
    }
}
