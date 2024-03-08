using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ICuentaPagarRep
    {
        Task CrearCuentaPagar(DateTime FechaCreacion, DateTime FechaVencimiento, decimal TotalPagado, string IdFacturaCompra, string IdProveedor, bool Estado);
        Task ActualizarCuentaPagar(string IdCuentaPagar, DateTime FechaCreacion, DateTime FechaVencimiento, decimal TotalPagado, string IdFacturaCompra, string IdProveedor, bool Estado);
        Task EliminarCuentaPagar(string IdCuentaPagar);
        Task<List<CuentaPagar>> MostrarCuentasPagar();
        Task<CuentaPagar> ConsultarCuentasPagar(string IdCuentaPagar);
        Task<CuentaPagar> ConsultarCuentasPagarporFactura(string IdFacturaCompra);
    }
}
