using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DetallesPresupuestoRep : IDetallesPresupuestoRep
    {
        private readonly MiDbContext _context;

        public DetallesPresupuestoRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearDetallesPresupuesto(string IdPresupuesto, string IdRegistroLibros, int IdTransaccion, DateTime FechaIngreso, string Observaciones)
        {
            var IdPresupuestoParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            var IdRegistroLibrosParam = new SqlParameter("@IdRegistroLibros", IdRegistroLibros);
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            var FechaIngresoParam = new SqlParameter("@FechaIngreso", FechaIngreso);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            await _context.Database.ExecuteSqlRawAsync("CrearDetallesPresupuesto @IdPresupuesto, @IdRegistroLibros, @IdTransaccion, @FechaIngreso, @Observaciones", IdPresupuestoParam, IdRegistroLibrosParam, IdTransaccionParam, FechaIngresoParam, ObservacionesParam);
        }

        public async Task ActualizarDetallesPresupuesto(string IdPresupuesto, int IdTransaccion, DateTime FechaIngreso, string Observaciones)
        {
            var IdPresupuestoParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            var FechaIngresoParam = new SqlParameter("@FechaIngreso", FechaIngreso);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            await _context.Database.ExecuteSqlRawAsync("ActualizarDetallesPresupuesto @IdPresupuesto, @IdTransaccion, @FechaIngreso, @Observaciones", IdPresupuestoParam, IdTransaccionParam, FechaIngresoParam, ObservacionesParam);
        }

        public async Task EliminarDetallesPresupuesto(int IdTransaccion)
        {
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            await _context.Database.ExecuteSqlRawAsync("EliminarDetallesPresupuesto @IdTransaccion", IdTransaccionParam);
        }

        public async Task<List<DetallePresupuesto>> MostrarDetallesPresupuesto()
        {
            var detallesPresupuesto = await _context.DetallePresupuesto
                                                    .FromSqlRaw("EXEC MostrarDetallesPresupuesto")
                                                    .ToListAsync();
            return detallesPresupuesto;
        }

        public async Task<DetallePresupuesto> ConsultarDetallesPresupuesto(int IdTransaccion)
        {
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            var resultado = await _context.DetallePresupuesto
                                          .FromSqlRaw("EXEC ConsultarDetallesPresupuesto @IdTransaccion", IdTransaccionParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
