using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IFacturaCompraRep
    {
        Task CrearFacturaCompra(string FacturaCompra, DateTime FechaFactura, decimal TotalCompra, string DetallesCompra, int IdTipoPago, int IdTipoFactura);
        Task ActualizarFacturaCompra(string IdFacturaCompra, string FacturaCompra, DateTime FechaFactura, decimal TotalCompra, string DetallesCompra, int IdTipoPago, int IdTipoFactura);
        Task EliminarFacturaCompra(string IdFacturaCompra);
        Task<List<FacturaCompra>> MostrarFacturasCompras();
        Task<FacturaCompra> ConsultarFacturasCompras(string IdFacturaCompra);
    }
}
