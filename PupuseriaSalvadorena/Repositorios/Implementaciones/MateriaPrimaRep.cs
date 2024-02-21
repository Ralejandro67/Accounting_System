using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class MateriaPrimaRep : IMateriaPrimaRep
    {
        private readonly MiDbContext _context;

        public MateriaPrimaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearMateriaPrima(string NombreMateriaPrima, string IdProveedor)
        {
            var NombreMateriaPrimaParam = new SqlParameter("@NombreMateriaPrima", NombreMateriaPrima);
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            await _context.Database.ExecuteSqlRawAsync("CrearMateriaPrima @NombreMateriaPrima, @IdProveedor", NombreMateriaPrimaParam, IdProveedorParam);
        }

        public async Task ActualizarMateriaPrima(int IdMateriaPrima, string NombreMateriaPrima, string IdProveedor)
        {
            var IdMateriaPrimaParam = new SqlParameter("@IdMateriaPrima", IdMateriaPrima);
            var NombreMateriaPrimaParam = new SqlParameter("@NombreMateriaPrima", NombreMateriaPrima);
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            await _context.Database.ExecuteSqlRawAsync("ActualizarMateriaPrima @IdMateriaPrima, @NombreMateriaPrima, @IdProveedor", IdMateriaPrimaParam, NombreMateriaPrimaParam, IdProveedorParam);
        }

        public async Task EliminarMateriaPrima(int IdMateriaPrima)
        {
            var IdMateriaPrimaParam = new SqlParameter("@IdMateriaPrima", IdMateriaPrima);
            await _context.Database.ExecuteSqlRawAsync("EliminarMateriaPrima @IdMateriaPrima", IdMateriaPrimaParam);
        }

        public async Task<List<MateriaPrima>> MostrarMateriaPrima()
        {
            var materiasPrimas = await _context.MateriaPrima
                                        .FromSqlRaw("EXEC MostrarMateriasPrimas")
                                        .ToListAsync();
            return materiasPrimas;
        }

        public async Task<MateriaPrima> ConsultarMateriasPrimas(int IdMateriaPrima)
        {
            var NombreMateriaPrimaParam = new SqlParameter("@IdMateriaPrima", IdMateriaPrima);
            var resultado = await _context.MateriaPrima
                                          .FromSqlRaw("EXEC ConsultarMateriasPrimas @IdMateriaPrima", NombreMateriaPrimaParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<MateriaPrima>> ConsultarMateriasPrimasProveedor(string IdProveedor)
        {
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            var resultado = await _context.MateriaPrima
                                          .FromSqlRaw("EXEC ConsultarMateriasPrimasProveedor @IdProveedor", IdProveedorParam)
                                          .ToListAsync();

            return resultado;
        }

        public async Task<List<MateriaPrima>> ConsultarConteoMateriasPrimas(string IdProveedor)
        {
            var IdProveedorParam = new SqlParameter("@IdProveedor", IdProveedor);
            var resultado = await _context.MateriaPrima
                                          .FromSqlRaw("EXEC ConsultarConteoMateriasPrimas @IdProveedor", IdProveedorParam)
                                          .ToListAsync();

            return resultado;
        }
    }
}
