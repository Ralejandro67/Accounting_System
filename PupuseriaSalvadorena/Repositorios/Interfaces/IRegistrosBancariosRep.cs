using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IRegistrosBancariosRep
    {
        Task CrearRegistroBancario(string EstadoBancario, DateTime FechaRegistro, int NumeroCuenta, string Observaciones, int CedulaJuridica);
        Task ActualizarRegistroBancario(string IdRegistro, string EstadoBancario, DateTime FechaRegistro, int NumeroCuenta, string Observaciones);
        Task EliminarRegistroBancario(string IdRegistro);
        Task<List<RegistroBancario>> MostrarRegistrosBancarios();
        Task<RegistroBancario> ConsultarRegistrosBancarios(string IdRegistro);
    }
}
