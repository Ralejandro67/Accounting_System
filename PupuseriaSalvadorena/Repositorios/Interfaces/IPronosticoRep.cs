using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IPronosticoRep
    {
        Task<int> CrearPronostico(int IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta);
        Task ActualizarPronosticos(int IdPronostico, int IdPlatillo, DateTime FechaInicio, DateTime FechaFinal, int CantTotalProd, decimal TotalVenta);
        Task EliminarPronostico(int IdPronostico);
        Task<List<Pronostico>> MostrarPronosticos();
        Task<Pronostico> ConsultarPronosticos(int IdPronostico);
        Task<int> ObtenerIdPronostico();
    }
}
