using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ITelefonosRep
    {
        Task CrearTelefono(int numero, bool estado);
        Task ActualizarTelefono(int IdTelefono, int Telefono, bool Estado);
        Task EliminarTelefono(int IdTelefono);
        Task<List<Telefonos>> MostrarTelefonos();
        Task<Telefonos> ConsultarTelefonos(int IdTelefono);
    }
}
