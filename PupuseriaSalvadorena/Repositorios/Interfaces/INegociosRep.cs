using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface INegociosRep
    {
        Task ActualizarNegocio(long CedulaJuridica, string NombreEmpresa);
        Task<Negocio> MostrarNegocio();
        Task<Negocio> ConsultarNegocio(long CedulaJuridica);
        Task<long> ConsultarNegocio();
    }
}
