using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDeclaracionTaxRep
    {
        Task CrearDeclaracionTax(long CedulaJuridica, DateTime FechaInicio, DateTime FechaFinal, decimal MontoTotalIngresos, decimal MontoTotalEgresos, decimal MontoTotalImpuestos, string Observaciones);
        Task ActualizarDeclaracionTax(string IdDeclaracionImpuesto, DateTime FechaInicio, DateTime FechaFinal, decimal MontoTotalIngresos, decimal MontoTotalEgresos, decimal MontoTotalImpuestos, string Observaciones);
        Task EliminarDeclaracionTax(string IdDeclaracionImpuesto);
        Task<List<DeclaracionImpuesto>> MostrarDeclaracionesImpuestos();
        Task<DeclaracionImpuesto> ConsultarDeclaracionesImpuestos(string IdDeclaracionImpuesto);
    }
}
