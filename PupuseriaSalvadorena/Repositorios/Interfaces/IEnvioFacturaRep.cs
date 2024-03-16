using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IEnvioFacturaRep
    {
        Task CrearEnvioFactura(DateTime FechaEnvio, int IdFacturaVenta, long Identificacion, string NombreCliente, string CorreoElectronico, int Telefono);
        Task ActualizarEnvioFactura(int IdEnvioFactura, DateTime FechaEnvio, int IdFacturaVenta, long Identificacion, string NombreCliente, string CorreoElectronico, int Telefono);
        Task EliminarEnvioFactura(int IdEnvioFactura);
        Task<List<EnvioFactura>> MostrarEnvioFactura();
        Task<EnvioFactura> ConsultarEnvioFacturas(int IdEnvioFactura);
        Task<EnvioFactura> ConsultarEnvioFacturasPorID(int IdFacturaVenta);
    }
}
