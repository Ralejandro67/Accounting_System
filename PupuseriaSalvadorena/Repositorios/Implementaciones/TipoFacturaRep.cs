using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class TipoFacturaRep : ITipoFacturaRep
    {
        private readonly MiDbContext _context;

        public TipoFacturaRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearTipoFactura(string nombre, bool activo)
        {
            var nombreParam = new SqlParameter("@NombreFactura", nombre);
            var activoParam = new SqlParameter("@Estado", activo);
            await _context.Database.ExecuteSqlRawAsync("CrearTipoFactura @NombreFactura, @Estado", nombreParam, activoParam);
        }

        public async Task ActualizarTipoFactura(int IdTipoFactura, string NombreFactura, bool Estado)
        {
            var idTipoFacturaParam = new SqlParameter("@IdTipoFactura", IdTipoFactura);
            var nombreFacturaParam = new SqlParameter("@NombreFactura", NombreFactura);
            var estadoParam = new SqlParameter("@Estado", Estado);
            await _context.Database.ExecuteSqlRawAsync("ActualizarTipoFacturas @IdTipoFactura, @NombreFactura, @Estado", idTipoFacturaParam, nombreFacturaParam, estadoParam);
        }

        public async Task EliminarTipoFactura(int IdTipoFactura)
        {
            var idTipoFacturaParam = new SqlParameter("@IdTipoFactura", IdTipoFactura);
            await _context.Database.ExecuteSqlRawAsync("EliminarTipoFactura @IdTipoFactura", idTipoFacturaParam);
        }

        public async Task<List<TipoFactura>> MostrarTipoFacturas()
        {
            var tipoFacturas = await _context.TipoFactura
                                        .FromSqlRaw("EXEC MostrarTipoFacturas")
                                        .ToListAsync();
            return tipoFacturas;
        }

        public async Task<TipoFactura> ConsultarTipoFacturas(int IdTipoFactura)
        {
            var nombreFacturaParam = new SqlParameter("@IdTipoFactura", IdTipoFactura);
            var resultado = await _context.TipoFactura
                                          .FromSqlRaw("EXEC ConsultarTipoFacturas @IdTipoFactura", nombreFacturaParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
