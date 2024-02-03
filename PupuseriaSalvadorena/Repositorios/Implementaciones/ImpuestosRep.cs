using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class ImpuestosRep : IImpuestosRep
    {
        private readonly MiDbContext _context;

        public ImpuestosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearImpuesto(string NombreImpuesto, decimal Tasa, bool Estado, string Descripcion)
        {
            var NombreImpuestoParam = new SqlParameter("@NombreImpuesto", NombreImpuesto);
            var TasaParam = new SqlParameter("@Tasa", Tasa);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            var DescripcionParam = new SqlParameter("@Descripcion", Descripcion);
            await _context.Database.ExecuteSqlRawAsync("CrearImpuesto @NombreImpuesto, @Tasa, @Estado, @Descripcion", NombreImpuestoParam, TasaParam, EstadoParam, DescripcionParam);
        }
        
        public async Task ActualizarImpuesto(string IdImpuesto, string NombreImpuesto, decimal Tasa, bool Estado, string Descripcion)
        {
            var IdImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            var NombreImpuestoParam = new SqlParameter("@NombreImpuesto", NombreImpuesto);
            var TasaParam = new SqlParameter("@Tasa", Tasa);
            var EstadoParam = new SqlParameter("@Estado", Estado);
            var DescripcionParam = new SqlParameter("@Descripcion", Descripcion);
            await _context.Database.ExecuteSqlRawAsync("ActualizarImpuesto @IdImpuesto, @NombreImpuesto, @Tasa, @Estado, @Descripcion", IdImpuestoParam, NombreImpuestoParam, TasaParam, EstadoParam, DescripcionParam);
        }
        
        public async Task EliminarImpuesto(string IdImpuesto)
        {
            var IdImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            await _context.Database.ExecuteSqlRawAsync("EliminarImpuesto @IdImpuesto", IdImpuestoParam);
        }

        public async Task<List<Impuesto>> MostrarImpuestos()
        {
            var impuestos = await _context.Impuesto
                                        .FromSqlRaw("EXEC MostrarImpuestos")
                                        .ToListAsync();
            return impuestos;
        }

        public async Task<Impuesto> ConsultarImpuestos(string IdImpuesto)
        {
            var IdImpuestoParam = new SqlParameter("@IdImpuesto", IdImpuesto);
            var resultado = await _context.Impuesto
                                          .FromSqlRaw("EXEC ConsultarImpuestos @IdImpuesto", IdImpuestoParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
