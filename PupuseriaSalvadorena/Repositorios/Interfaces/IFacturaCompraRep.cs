using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IFacturaCompraRep
    {
        Task<string> CrearFacturaCompra(DateTime FechaFactura, decimal TotalCompra, string DetallesCompra, int IdTipoPago, int IdTipoFactura, int IdMateriaPrima);
        Task ActualizarFacturaCompra(string IdFacturaCompra, byte[] FacturaCom, DateTime FechaFactura, decimal TotalCompra, string DetallesCompra, int IdTipoPago, int IdTipoFactura, int IdMateriaPrima);
        Task EliminarFacturaCompra(string IdFacturaCompra);
        Task<List<FacturaCompra>> MostrarFacturasCompras();
        Task<FacturaCompra> ConsultarFacturasCompras(string IdFacturaCompra);
        Task<string> CrearFacturaId(byte[] FacturaCom, DateTime FechaFactura, decimal TotalCompra, string DetallesCompra, int IdTipoPago, int IdTipoFactura, int IdMateriaPrima);
        Task<List<FacturaCompra>> MostrarFacturasCExisteHistorial();
    }
}
