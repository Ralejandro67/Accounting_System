using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IPronosticoRep
    {
        Task CrearPronostico(int IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta, string PronosticoDoc);
        Task ActualizarPronosticos(int IdPronostico, int IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta, string PronosticoDoc);
        Task EliminarPronostico(int IdPronostico);
        Task<List<Pronostico>> MostrarPronostico();
        Task<Pronostico> ConsultarPronosticos(int IdPronostico);
    }
}
