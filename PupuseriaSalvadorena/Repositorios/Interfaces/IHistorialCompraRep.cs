using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IHistorialCompraRep
    {
        Task CrearHistorialCompra(int IdMateriaPrima, int CantCompra, decimal Precio, decimal Peso, DateTime FechaCompra, string IdFacturaCompra);
        Task ActualizarHistorialCompra(string IdCompra, int IdMateriaPrima, int CantCompra, decimal Precio, decimal Peso, DateTime FechaCompra, string IdFacturaCompra);
        Task EliminarHistorialCompra(string IdCompra);
        Task<List<HistorialCompra>> MostrarHistorialCompras();
        Task<HistorialCompra> ConsultarHistorialCompras(string IdCompra);
        Task<HistorialCompra> ConsultarHistorialComprasporFactura(string IdFacturaCompra);
        Task EliminarHistorialCompraFactura(string IdFacturaCompra);
    }
}
