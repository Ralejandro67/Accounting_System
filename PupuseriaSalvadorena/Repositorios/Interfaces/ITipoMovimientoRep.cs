using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoMovimientoRep
    {
        Task<List<TipoMovimiento>> MostrarTipoMovimiento();
    }
}
