using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class HistorialCompraRep : IHistorialCompraRep
    {
        private readonly MiDbContext _context;

        public HistorialCompraRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearHistorialCompra(int IdMateriaPrima, int CantCompra, decimal Precio, decimal Peso, DateTime FechaCompra, string IdFacturaCompra)
        {
            var IdMateriaPrimaParam = new SqlParameter("@IdMateriaPrima", IdMateriaPrima);
            var CantCompraParam = new SqlParameter("@CantCompra", CantCompra);
            var PrecioParam = new SqlParameter("@Precio", Precio);
            var PesoParam = new SqlParameter("@Peso", Peso);
            var FechaCompraParam = new SqlParameter("@FechaCompra", FechaCompra);
            var IdFacturaCompraParam = new SqlParameter("@IdFacturaCompra", IdFacturaCompra);
            await _context.Database.ExecuteSqlRawAsync("CrearHistorialCompra @IdMateriaPrima, @CantCompra, @Precio, @Peso, @FechaCompra, @IdFacturaCompra", IdMateriaPrimaParam, CantCompraParam, PrecioParam, PesoParam, FechaCompraParam, IdFacturaCompraParam);
        }

        public async Task<List<HistorialCompra>> MostrarHistorialCompras()
        {
            var historialCompras = await _context.HistorialCompra
                                        .FromSqlRaw("EXEC MostrarHistorialCompras")
                                        .ToListAsync();
            return historialCompras;
        }

        public async Task<HistorialCompra> ConsultarHistorialCompras(string IdCompra)
        {
            var IdCompraParam = new SqlParameter("@IdCompra", IdCompra);
            var resultado = await _context.HistorialCompra
                                          .FromSqlRaw("EXEC ConsultarHistorialCompras @IdCompra", IdCompraParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task EliminarHistorialCompraFactura(string IdFacturaCompra)
        {
            var IdFacturaCompraParam = new SqlParameter("@IdFacturaCompra", IdFacturaCompra);
            await _context.Database.ExecuteSqlRawAsync("EliminarHistorialCompraFactura @IdFacturaCompra", IdFacturaCompraParam);
        }
    }
}
