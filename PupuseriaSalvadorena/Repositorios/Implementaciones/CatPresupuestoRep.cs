using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class CatPresupuestoRep : ICatPresupuestoRep
    {
        private readonly MiDbContext _context;

        public CatPresupuestoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearCatPresupuesto(string nombre, bool Estado)
        {
            var nombreParam = new SqlParameter("@Nombre", nombre);
            var activoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("CrearCatPresupuesto @Nombre, @Estado", nombreParam, activoParam);
        }

        public async Task ActualizarCatPresupuestos(int IdCategoriaP, string Nombre, bool Estado)
        {
            var IdCategoriaPParam = new SqlParameter("@IdCategoriaP", IdCategoriaP);
            var nombreRolParam = new SqlParameter("@Nombre", Nombre);
            var activoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("ActualizarCatPresupuestos @IdCategoriaP, @Nombre, @Estado", IdCategoriaPParam, nombreRolParam, activoParam);
        }

        public async Task EliminarCatPresupuestos(int IdCategoriaP)
        {
            var IdCategoriaPParam = new SqlParameter("@IdCategoriaP", IdCategoriaP);
            await _context.Database.ExecuteSqlRawAsync("EliminarCatPresupuestos @IdCategoriaP", IdCategoriaPParam);
        }

        public async Task<List<CategoriaPresupuesto>> MostrarCatPresupuestos()
        {
            var catPresupuestos = await _context.CategoriaPresupuesto
                                        .FromSqlRaw("EXEC MostrarCatPresupuestos")
                                        .ToListAsync();
            return catPresupuestos;
        }

        public async Task<CategoriaPresupuesto> ConsultarCatPresupuestos(int IdCategoriaP)
        {
            var IdCategoriaPParam = new SqlParameter("@IdCategoriaP", IdCategoriaP);
            var resultado = await _context.CategoriaPresupuesto
                                          .FromSqlRaw("EXEC ConsultarCatPresupuestos @IdCategoriaP", IdCategoriaPParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
