using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class PresupuestoRep : IPresupuestoRep
    {
        private readonly MiDbContext _context;

        public PresupuestoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearPresupuesto(string NombreP, DateTime FechaInicio, DateTime FechaFin, string Descripcion, decimal SaldoUsado, decimal SaldoPresupuesto, bool Estado, int IdCategoriaP)
        {
            var NombrePParam = new SqlParameter("@NombreP", NombreP);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var FechaFinParam = new SqlParameter("@FechaFin", FechaFin);
            var DescripcionParam = new SqlParameter("@Descripcion", Descripcion);
            var SaldoUsadoParam = new SqlParameter("@SaldoUsado", SaldoUsado);
            var SaldoPresupuestoParam = new SqlParameter("@SaldoPresupuesto", SaldoPresupuesto);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            var IdCategoriaPParam = new SqlParameter("@IdCategoriaP", IdCategoriaP);
            await _context.Database.ExecuteSqlRawAsync("CrearPresupuesto @NombreP, @FechaInicio, @FechaFin, @Descripcion, @SaldoUsado, @SaldoPresupuesto, @Estado, @IdCategoriaP", NombrePParam, FechaInicioParam, FechaFinParam, DescripcionParam, SaldoUsadoParam, SaldoPresupuestoParam, EstadoParam, IdCategoriaPParam);
        }

        public async Task ActualizarPresupuesto(string IdPresupuesto, string NombreP, DateTime FechaInicio, DateTime FechaFin, string Descripcion, decimal SaldoUsado, decimal SaldoPresupuesto, bool Estado, int IdCategoriaP)
        {
            var IdPresupuestoParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            var NombrePParam = new SqlParameter("@NombreP", NombreP);
            var FechaInicioParam = new SqlParameter("@FechaInicio", FechaInicio);
            var FechaFinParam = new SqlParameter("@FechaFin", FechaFin);
            var DescripcionParam = new SqlParameter("@Descripcion", Descripcion);
            var SaldoUsadoParam = new SqlParameter("@SaldoUsado", SaldoUsado);
            var SaldoPresupuestoParam = new SqlParameter("@SaldoPresupuesto", SaldoPresupuesto);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            var IdCategoriaPParam = new SqlParameter("@IdCategoriaP", IdCategoriaP);
            await _context.Database.ExecuteSqlRawAsync("ActualizarPresupuesto @IdPresupuesto, @NombreP, @FechaInicio, @FechaFin, @Descripcion, @SaldoUsado, @SaldoPresupuesto, @Estado, @IdCategoriaP", IdPresupuestoParam, NombrePParam, FechaInicioParam, FechaFinParam, DescripcionParam, SaldoUsadoParam, SaldoPresupuestoParam, EstadoParam, IdCategoriaPParam);
        }

        public async Task EliminarPresupuesto(string IdPresupuesto)
        {
            var IdPresupuestoParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            await _context.Database.ExecuteSqlRawAsync("EliminarPresupuesto @IdPresupuesto", IdPresupuestoParam);
        }

        public async Task<List<Presupuesto>> MostrarPresupuestos()
        {
            var presupuestos = await _context.Presupuesto
                                        .FromSqlRaw("EXEC MostrarPresupuestos")
                                        .ToListAsync();
            return presupuestos;
        }

        public async Task<Presupuesto> ConsultarPresupuestos(string IdPresupuesto)
        {
            var NombrePParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            var resultado = await _context.Presupuesto
                                          .FromSqlRaw("EXEC ConsultarPresupuestos @IdPresupuesto", NombrePParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
