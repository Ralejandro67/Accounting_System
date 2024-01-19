using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IImpuestosRep
    {
        Task CrearImpuesto(string NombreImpuesto, decimal Tasa, bool Estado, string Descripcion);
        Task ActualizarImpuesto(string IdImpuesto, string NombreImpuesto, decimal Tasa, bool Estado, string Descripcion);
        Task EliminarImpuesto(string IdImpuesto);
        Task<List<Impuesto>> MostrarImpuestos();
        Task<Impuesto> ConsultarImpuestos(string IdImpuesto);
    }
}
