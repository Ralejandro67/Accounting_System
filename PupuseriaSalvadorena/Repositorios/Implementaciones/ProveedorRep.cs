using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class ProveedorRep : IProveedorRep
    {
        private readonly MiDbContext _context;

        public ProveedorRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearProveedor(string NombreProveedor, string ApellidoProveedor, int Telefono)
        {
            var NombreProveedorParam = new SqlParameter("@NombreProveedor", NombreProveedor);
            var ApellidoProveedorParam = new SqlParameter("@ApellidoProveedor", ApellidoProveedor);
            var TelefonoParam = new SqlParameter("@Telefono", Telefono);
            await _context.Database.ExecuteSqlRawAsync("CrearProveedor @NombreProveedor, @ApellidoProveedor, @Telefono", NombreProveedorParam, ApellidoProveedorParam, TelefonoParam);
        }

        public async Task ActualizarProveedor(string IdProveedor, string NombreProveedor, string ApellidoProveedor, int Telefono)
        {
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            var NombreProveedorParam = new SqlParameter("@NombreProveedor", NombreProveedor);
            var ApellidoProveedorParam = new SqlParameter("@ApellidoProveedor", ApellidoProveedor);
            var TelefonoParam = new SqlParameter("@Telefono", Telefono);
            await _context.Database.ExecuteSqlRawAsync("ActualizarProveedor @IdProveedor, @NombreProveedor, @ApellidoProveedor, @Telefono", IdProveedorParam, NombreProveedorParam, ApellidoProveedorParam, TelefonoParam);
        }

        public async Task EliminarProveedor(string IdProveedor)
        {
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            await _context.Database.ExecuteSqlRawAsync("EliminarProveedor @IdProveedor", IdProveedorParam);
        }

        public async Task<List<Proveedor>> MostrarProveedores()
        {
            var proveedores = await _context.Proveedor
                                        .FromSqlRaw("EXEC MostrarProveedores")
                                        .ToListAsync();
            return proveedores;
        }

        public async Task<Proveedor> ConsultarProveedores(string IdProveedor)
        {
            var NombreProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            var resultado = await _context.Proveedor
                                          .FromSqlRaw("EXEC ConsultarProveedores @IdProveedor", NombreProveedorParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
