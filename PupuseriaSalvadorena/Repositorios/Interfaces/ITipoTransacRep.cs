using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoTransacRep
    {
        Task CrearTipoTransac(string TipoTransac);
        Task ActualizarTipoTransac(int IdTipo, string TipoTransac);
        Task EliminarTipoTransac(int IdTipo);
        Task<List<TipoTransacciones>> MostrarTipoTransaccion();
        Task<TipoTransacciones> ConsultarTipoTransaccion(int IdTipo);
    }
}
