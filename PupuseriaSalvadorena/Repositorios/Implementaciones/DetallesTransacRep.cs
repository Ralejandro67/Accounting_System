using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class DetallesTransacRep : IDetallesTransacRep
    {
        private readonly MiDbContext _context;

        public DetallesTransacRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearDetalleTransaccion(string IdRegistroLibros, string DescripcionTransaccion, int Cantidad, decimal Monto, DateTime FechaRegistro, int IdTipo, string IdImpuesto, bool Recurrencia, DateTime FechaRecurrencia, string Frecuencia)
        {
            var IdRegistroLibrosParam = new SqlParameter("@IdRegistroLibros", IdRegistroLibros);
            var DescripcionTransaccionParam = new SqlParameter("@DescripcionTransaccion", DescripcionTransaccion);
            var CantidadParam = new SqlParameter("@Cantidad", Cantidad);
            var MontoParam = new SqlParameter("@Monto", Monto);
            var FechaRegistroParam = new SqlParameter("@FechaTrans", FechaRegistro);
            var IdTipoParam = new SqlParameter("@IdTipo", IdTipo);
            var IdImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            var RecurrenciaParam = new SqlParameter("@Recurrencia", Recurrencia);
            var FechaRecurrenciaParam = new SqlParameter("@FechaRecurrencia", FechaRecurrencia);
            var FrecuenciaParam = new SqlParameter("@Frecuencia", Frecuencia);
            await _context.Database.ExecuteSqlRawAsync("CrearDetallesTransac @IdRegistroLibros, @DescripcionTransaccion, @Cantidad, @Monto, @FechaTrans, @IdTipo, @IdImpuesto, @Recurrencia, @FechaRecurrencia, @Frecuencia", IdRegistroLibrosParam, DescripcionTransaccionParam, CantidadParam, MontoParam, FechaRegistroParam, IdTipoParam, IdImpuestoParam, RecurrenciaParam, FechaRecurrenciaParam, FrecuenciaParam);
        }

        public async Task ActualizarDetalleTransaccion(int IdTransaccion, string DescripcionTransaccion, int Cantidad, decimal Monto, int IdTipo, string IdImpuesto)
        {
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            var DescripcionTransaccionParam = new SqlParameter("@DescripcionTransaccion", DescripcionTransaccion);
            var CantidadParam = new SqlParameter("@Cantidad", Cantidad);
            var MontoParam = new SqlParameter("@Monto", Monto);
            var IdTipoParam = new SqlParameter("@IdTipo", IdTipo);
            var IdImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            await _context.Database.ExecuteSqlRawAsync("ActualizarDetallesTransac @IdTransaccion, @DescripcionTransaccion, @Cantidad, @Monto, @IdTipo, @IdImpuesto", IdTransaccionParam, DescripcionTransaccionParam, CantidadParam, MontoParam, IdTipoParam, IdImpuestoParam);
        }

        public async Task EliminarDetallesTransaccion(int IdTransaccion)
        {
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            await _context.Database.ExecuteSqlRawAsync("EliminarDetallesTransaccion @IdTransaccion", IdTransaccionParam);
        }

        public async Task<List<DetalleTransaccion>> MostrarDetallesTransacciones()
        {
            var detallesTransaccion = await _context.DetalleTransaccion
                                        .FromSqlRaw("EXEC MostrarDetallesTransacciones")
                                        .ToListAsync();
            return detallesTransaccion;
        }

        public async Task<DetalleTransaccion> ConsultarDetallesTransacciones(int IdTransaccion)
        {
            var IdTransaccionParam = new SqlParameter("@IdTransaccion", IdTransaccion);
            var resultado = await _context.DetalleTransaccion
                                          .FromSqlRaw("EXEC ConsultarDetallesTransacciones @IdTransaccion", IdTransaccionParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<int> CrearTransaccionRecurrente(string IdRegistroLibros, string DescripcionTransaccion, int Cantidad, decimal Monto, DateTime FechaRegistro, int IdTipo, string IdImpuesto, bool Recurrencia, DateTime FechaRecurrencia, string Frecuencia)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "CrearTransaccionRecurrente";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@IdRegistroLibros", IdRegistroLibros));
                command.Parameters.Add(new SqlParameter("@DescripcionTransaccion", DescripcionTransaccion));
                command.Parameters.Add(new SqlParameter("@Cantidad", Cantidad));
                command.Parameters.Add(new SqlParameter("@Monto", Monto));
                command.Parameters.Add(new SqlParameter("@FechaTrans", FechaRegistro));
                command.Parameters.Add(new SqlParameter("@IdTipo", IdTipo));
                command.Parameters.Add(new SqlParameter("@IdImpuesto", IdImpuesto));
                command.Parameters.Add(new SqlParameter("@Recurrencia", Recurrencia));
                command.Parameters.Add(new SqlParameter("@FechaRecurrencia", FechaRecurrencia));
                command.Parameters.Add(new SqlParameter("@Frecuencia", Frecuencia));

                var IdTransaccion = new SqlParameter("@IdTransaccion", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IdTransaccion);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (int)IdTransaccion.Value;
            }
        }
    }
}
