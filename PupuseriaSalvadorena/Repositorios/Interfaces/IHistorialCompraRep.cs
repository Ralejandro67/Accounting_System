using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IHistorialCompraRep
    {
        Task CrearHistorialCompra(int IdMateriaPrima, int CantCompra, decimal Precio, decimal Peso, DateTime FechaCompra, string IdFacturaCompra);
        Task<List<HistorialCompra>> MostrarHistorialCompras();
        Task<HistorialCompra> ConsultarHistorialCompras(string IdCompra);
        Task EliminarHistorialCompraFactura(string IdFacturaCompra);
    }
}
