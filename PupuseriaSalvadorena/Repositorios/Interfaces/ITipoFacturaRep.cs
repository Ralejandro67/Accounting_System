using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITipoFacturaRep
    {
        Task CrearTipoFactura(string nombre, bool activo);
        Task ActualizarTipoFactura(int IdTipoFactura, string NombreFactura, bool Estado);
        Task EliminarTipoFactura(int IdTipoFactura);
        Task<List<TipoFactura>> MostrarTipoFacturas();
        Task<TipoFactura> ConsultarTipoFacturas(int IdTipoFactura);
    }
}
