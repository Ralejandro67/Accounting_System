using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IEnvioFacturaRep
    {
        Task CrearEnvioFactura(DateTime FechaEnvio, int IdFacturaVenta);
        Task ActualizarEnvioFactura(int IdEnvioFactura, DateTime FechaEnvio, int IdFacturaVenta);
        Task EliminarEnvioFactura(int IdEnvioFactura);
        Task<List<EnvioFactura>> MostrarEnvioFactura();
        Task<EnvioFactura> ConsultarEnvioFacturas(int IdEnvioFactura);
    }
}
