using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoVentaRep
    {
        Task CrearTipoVenta(string NombreVenta, bool Estado);
        Task ActualizarTipoVentas(int IdTipoVenta, string NombreVenta, bool Estado);
        Task EliminarTipoVenta(int IdTipoVenta);
        Task<List<TipoVenta>> MostrarTipoVentas();
        Task<TipoVenta> ConsultarTipoVentas(int IdTipoVenta);
    }
}
