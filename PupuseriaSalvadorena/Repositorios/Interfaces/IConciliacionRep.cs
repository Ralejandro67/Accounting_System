using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IConciliacionRep
    {
        Task CrearConciliacion(DateTime FechaConciliacion, decimal SaldoBancario, decimal SaldoLibro, decimal Diferencia, string Observaciones, string IdRegistro, string IdRegistroLibros);
        Task ActualizarConciliacion(string IdConciliacion, decimal SaldoBancario, decimal SaldoLibro, decimal Diferencia, string Observaciones);
        Task EliminarConciliacion(string IdConciliacion);
        Task<List<ConciliacionBancaria>> MostrarConciliacionesBancarias();
        Task<ConciliacionBancaria> ConsultarConciliacionesBancarias(string IdConciliacion);
    }
}
