using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface ICatPresupuestoRep
    {
        Task CrearCatPresupuesto(string nombre, bool Estado);
        Task ActualizarCatPresupuestos(int IdCategoriaP, string Nombre, bool Estado);
        Task EliminarCatPresupuestos(int IdCategoriaP);
        Task<List<CategoriaPresupuesto>> MostrarCatPresupuestos();
        Task<CategoriaPresupuesto> ConsultarCatPresupuestos(int IdCategoriaP);
    }
}
