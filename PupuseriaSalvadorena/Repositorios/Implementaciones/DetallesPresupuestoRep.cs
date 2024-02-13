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

        public async Task EliminarDetallesPresupuesto(string IdPresupuesto, int IdTransaccion)
        {
            var IdPresupuestoParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            await _context.Database.ExecuteSqlRawAsync("EliminarDetallesPresupuesto @IdPresupuesto, @IdTransaccion", IdPresupuestoParam, IdTransaccionParam);
        }

        public async Task<List<DetallePresupuesto>> MostrarDetallesPresupuesto()
        {
            var detallesPresupuesto = await _context.DetallePresupuesto
                                                    .FromSqlRaw("EXEC MostrarDetallesPresupuesto")
                                                    .ToListAsync();
            return detallesPresupuesto;
        }

        public async Task<DetallePresupuesto> ConsultarDetallesPresupuestos(string IdPresupuesto, int IdTransaccion)
        {
            var IdPresupuestoParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            var Transaccion = await _context.DetallePresupuesto
                                          .FromSqlRaw("EXEC ConsultarDetallesPresupuestos @IdPresupuesto, @IdTransaccion", IdPresupuestoParam, IdTransaccionParam)
                                          .ToListAsync();

            return Transaccion.FirstOrDefault();
        }

        public async Task<List<DetallePresupuesto>> ConsultarTransacPresupuestos(string IdPresupuesto)
        {
            var IdPresupuestoParam = new SqlParameter("@IdPresupuesto", IdPresupuesto);
            var resultado = await _context.DetallePresupuesto
                                          .FromSqlRaw("EXEC ConsultarTransacPresupuestos @IdPresupuesto", IdPresupuestoParam)
                                          .ToListAsync();

            return resultado;
        }
    }
}
