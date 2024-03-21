using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDetallesCuentaRep
    {
        Task CrearDetallesCuenta(decimal Pago, DateTime FechaIngreso, string IdCuentaPagar);
        Task ActualizarDetallesCuentas(string IdDetallesCuenta, decimal Pago, DateTime FechaIngreso);
        Task EliminarDetallesCuenta(string IdDetallesCuenta);
        Task<DetalleCuenta> ConsultarDetallesCuentas(string IdDetallesCuenta);
        Task<List<DetalleCuenta>> ConsultarCuentaDetalles(string IdCuentaPagar);
    }
}
