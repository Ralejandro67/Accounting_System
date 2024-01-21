using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface INegociosRep
    {
        Task CrearNegocio(long CedulaJuridica, string NombreEmpresa, int IdDireccion, int IdTelefono);
        Task ActualizarNegocio(long CedulaJuridica, string NombreEmpresa);
        Task EliminarNegocio(long CedulaJuridica);
        Task<List<Negocio>> MostrarNegocio();
        Task<Negocio> ConsultarNegocio(long CedulaJuridica);
    }
}
