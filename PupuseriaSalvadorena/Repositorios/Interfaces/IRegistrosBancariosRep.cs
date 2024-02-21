using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IRegistrosBancariosRep
    {
        Task CrearRegistroBancario(DateTime FechaRegistro, decimal SaldoInicial, string NumeroCuenta, string Observaciones, long CedulaJuridica);
        Task ActualizarRegistroBancario(string IdRegistro, DateTime FechaRegistro, decimal SaldoInicial, string NumeroCuenta, string Observaciones);
        Task EliminarRegistroBancario(string IdRegistro);
        Task<List<RegistroBancario>> MostrarRegistrosBancarios();
        Task<RegistroBancario> ConsultarRegistrosBancarios(string IdRegistro);
    }
}
