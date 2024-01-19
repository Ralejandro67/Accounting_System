using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface INegociosRep
    {
        Task CrearNegocio(int CedulaJuridica, string NombreEmpresa, int IdDireccion, int IdTelefono);
        Task ActualizarNegocio(int CedulaJuridica, string NombreEmpresa);
        Task EliminarNegocio(int CedulaJuridica);
        Task<List<Negocio>> MostrarNegocio();
        Task<Negocio> ConsultarNegocio(int CedulaJuridica);
    }
}
