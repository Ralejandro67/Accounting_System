using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IAlertaCuentaPagarRep
    {
        Task CrearAlertaCuentaPagar(string Mensaje, DateTime FechaMensaje, string IdCuentaPagar, bool Leido);
        Task ActualizarAlertaCuentaPagar(int IdAlerta, string Mensaje, DateTime FechaMensaje, string IdCuentaPagar, bool Leido);
        Task EliminarAlertaCuentaPagar(int IdAlerta);
        Task<List<AlertaCuentaPagar>> MostrarAlertaCuentaPagar();
        Task<AlertaCuentaPagar> ConsultarAlertaCuentaPagar(int IdAlerta);
        Task<List<AlertaCuentaPagar>> MostrarAlertasNoLeidas();
    }
}
