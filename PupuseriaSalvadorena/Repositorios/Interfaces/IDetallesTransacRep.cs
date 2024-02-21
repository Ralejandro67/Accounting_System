using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IDetallesTransacRep
    {
        Task CrearDetalleTransaccion(string IdRegistroLibros, string DescripcionTransaccion, int Cantidad, decimal Monto, DateTime FechaRegistro, int IdTipo, string IdImpuesto, bool Recurrencia, DateTime FechaRecurrencia, string Frecuencia, bool Conciliado);
        Task ActualizarDetalleTransaccion(int IdTransaccion, string DescripcionTransaccion, int Cantidad, decimal Monto, int IdTipo, string IdImpuesto, bool Conciliado);
        Task EliminarDetallesTransaccion(int IdTransaccion);
        Task<List<DetalleTransaccion>> MostrarDetallesTransacciones();
        Task<DetalleTransaccion> ConsultarDetallesTransacciones(int IdTransaccion);
        Task<int> CrearTransaccionRecurrente(string IdRegistroLibros, string DescripcionTransaccion, int Cantidad, decimal Monto, DateTime FechaRegistro, int IdTipo, string IdImpuesto, bool Recurrencia, DateTime FechaRecurrencia, string Frecuencia, bool Conciliado);
        Task<List<DetalleTransaccion>> ConsultarTransacciones(string IdRegistroLibros);
        Task<string> ObtenerIdLibroMasReciente();
        Task<DetalleTransaccion> ConsultarTransaccionesDetalles(string DescripcionTransaccion);
        Task<List<DetalleTransaccion>> MostrarDetallesTransaccionesYear();
        Task ActualizarConciliado(string IdRegistroLibros, bool Conciliado);
    }
}
