using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ICuentaPagarRep
    {
        Task CrearCuentaPagar(DateTime FechaCreacion, DateTime FechaVencimiento, decimal TotalPagado, string IdFacturaCompra, string IdProveedor);
        Task ActualizarCuentaPagar(string IdCuentaPagar, decimal TotalPagado, string IdFacturaCompra, string IdProveedor);
        Task EliminarCuentaPagar(string IdCuentaPagar);
        Task<List<CuentaPagar>> MostrarCuentasPagar();
        Task<CuentaPagar> ConsultarCuentasPagar(string IdCuentaPagar);
    }
}
