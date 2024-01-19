using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IPresupuestoRep
    {
        Task CrearPresupuesto(string NombreP, DateTime FechaInicio, DateTime FechaFin, string Descripcion, decimal SaldoUsado, decimal SaldoPresupuesto, bool Estado, int IdCategoriaP);
        Task ActualizarPresupuesto(string IdPresupuesto, string NombreP, DateTime FechaInicio, DateTime FechaFin, string Descripcion, decimal SaldoUsado, decimal SaldoPresupuesto, bool Estado, int IdCategoriaP);
        Task EliminarPresupuesto(string IdPresupuesto);
        Task<List<Presupuesto>> MostrarPresupuestos();
        Task<Presupuesto> ConsultarPresupuestos(string IdPresupuesto);
    }
}
