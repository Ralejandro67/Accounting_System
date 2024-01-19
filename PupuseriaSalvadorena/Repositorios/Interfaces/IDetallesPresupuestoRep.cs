using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDetallesPresupuestoRep
    {
        Task CrearDetallesPresupuesto(string IdPresupuesto, string IdRegistroLibros, int IdTransaccion, DateTime FechaIngreso, string Observaciones);
        Task ActualizarDetallesPresupuesto(string IdPresupuesto, int IdTransaccion, DateTime FechaIngreso, string Observaciones);
        Task EliminarDetallesPresupuesto(int IdTransaccion);
        Task<List<DetallePresupuesto>> MostrarDetallesPresupuesto();
        Task<DetallePresupuesto> ConsultarDetallesPresupuesto(int IdTransaccion);
    }
}
