using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IMateriaPrimaRep
    {
        Task CrearMateriaPrima(string NombreMateriaPrima, string IdProveedor);
        Task ActualizarMateriaPrima(int IdMateriaPrima, string NombreMateriaPrima, string IdProveedor);
        Task EliminarMateriaPrima(int IdMateriaPrima);
        Task<List<MateriaPrima>> MostrarMateriaPrima();
        Task<MateriaPrima> ConsultarMateriasPrimas(int IdMateriaPrima);
        Task<List<MateriaPrima>> ConsultarMateriasPrimasProveedor(string IdProveedor);
        Task<List<MateriaPrima>> ConsultarConteoMateriasPrimas(string IdProveedor);
    }
}
