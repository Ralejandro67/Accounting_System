using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDetallesTransacRep
    {
        Task CrearDetalleTransaccion(string IdRegistroLibros, string DescripcionTransaccion, int Cantidad, decimal Monto, DateTime FechaRegistro, int IdTipo, string IdImpuesto, bool Recurrencia, DateTime FechaRecurrencia, string Frecuencia);
        Task ActualizarDetalleTransaccion(int IdTransaccion, string DescripcionTransaccion, int Cantidad, decimal Monto, int IdTipo, string IdImpuesto);
        Task EliminarDetallesTransaccion(int IdTransaccion);
        Task<List<DetalleTransaccion>> MostrarDetallesTransacciones();
        Task<DetalleTransaccion> ConsultarDetallesTransacciones(int IdTransaccion);
        Task<int> CrearTransaccionRecurrente(string IdRegistroLibros, string DescripcionTransaccion, int Cantidad, decimal Monto, DateTime FechaRegistro, int IdTipo, string IdImpuesto, bool Recurrencia, DateTime FechaRecurrencia, string Frecuencia);
        Task<List<DetalleTransaccion>> ConsultarTransacciones(string IdRegistroLibros);
    }
}
