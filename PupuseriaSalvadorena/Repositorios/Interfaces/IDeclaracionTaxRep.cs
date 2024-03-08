using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDeclaracionTaxRep
    {
        Task<string> CrearDeclaracionTax(long CedulaJuridica, DateTime FechaInicio, string Trimestre, decimal MontoRenta, decimal MontoIVA, decimal MontoTotalImpuestos, decimal MontoTotal, string Observaciones);
        Task ActualizarDeclaracionTax(string IdDeclaracionImpuesto, string Observaciones);
        Task EliminarDeclaracionTax(string IdDeclaracionImpuesto);
        Task<List<DeclaracionImpuesto>> MostrarDeclaracionesImpuestos();
        Task<DeclaracionImpuesto> ConsultarDeclaracionesImpuestos(string IdDeclaracionImpuesto);
    }
}
