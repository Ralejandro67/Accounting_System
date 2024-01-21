using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IFacturaVentaRep
    {
        Task CrearFacturaVenta(long CedulaJuridica, int Consecutivo, int Clave, DateTime FechaFactura, decimal SubTotal, decimal TotalVenta, int IdTipoPago, int IdTipoFactura);
        Task ActualizarFacturaVenta(int IdFacturaVenta, int IdTipoPago, int IdTipoFactura);
        Task EliminarFacturaVenta(int IdFacturaVenta);
        Task<List<FacturaVenta>> MostrarFacturasVentas();
        Task<FacturaVenta> ConsultarFacturasVentas(int IdFacturaVenta);
    }
}
