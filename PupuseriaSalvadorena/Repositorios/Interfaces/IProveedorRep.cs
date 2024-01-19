using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IProveedorRep
    {
        Task CrearProveedor(string NombreProveedor, string ApellidoProveedor, int Telefono);
        Task ActualizarProveedor(string IdProveedor, string NombreProveedor, string ApellidoProveedor, int Telefono);
        Task EliminarProveedor(string IdProveedor);
        Task<List<Proveedor>> MostrarProveedores();
        Task<Proveedor> ConsultarProveedores(string IdProveedor);
    }
}
