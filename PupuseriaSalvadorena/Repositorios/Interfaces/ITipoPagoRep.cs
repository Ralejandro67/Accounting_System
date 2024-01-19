using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoPagoRep
    {
        Task CrearTipoPago(string NombrePago, bool Estado);
        Task ActualizarTipoPago(int IdTipoPago, string NombrePago, bool Estado);
        Task EliminarTipoPago(int IdTipoPago);
        Task<List<TipoPago>> MostrarTipoPagos();
        Task<TipoPago> ConsultarTipoPagos(int IdTipoPago);
    }
}
