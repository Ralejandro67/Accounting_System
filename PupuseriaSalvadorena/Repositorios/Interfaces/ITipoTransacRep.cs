using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoTransacRep
    {
        Task CrearTipoTransac(string TipoTransac, int IdMovimiento, string IdImpuesto);
        Task ActualizarTipoTransac(int IdTipo, string TipoTransac, int IdMovimiento, string IdImpuesto);
        Task EliminarTipoTransac(int IdTipo);
        Task<List<TipoTransacciones>> MostrarTipoTransaccion();
        Task<TipoTransacciones> ConsultarTipoTransaccion(int IdTipo);
        Task<List<TipoTransacciones>> ConsultarImpuestosporTransaccion(string IdImpuesto);
    }
}
