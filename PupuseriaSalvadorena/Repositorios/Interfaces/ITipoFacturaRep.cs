using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoFacturaRep
    {
        Task<List<TipoFactura>> MostrarTipoFacturas();
    }
}
