using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoMovimientoRep
    {
        Task CrearTipoMovimiento(string NombreMov);
        Task ActualizarTipoMovimiento(int IdMovimiento, string NombreMov);
        Task EliminarTipoMovimiento(int IdMovimiento);
        Task<List<TipoMovimiento>> MostrarTipoMovimiento();
        Task<TipoMovimiento> ConsultarTipoMovimiento(int IdMovimiento);
    }
}
