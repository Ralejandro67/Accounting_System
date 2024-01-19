using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class RegistrosBancariosRep : IRegistrosBancariosRep
    {
        private readonly MiDbContext _context;

        public RegistrosBancariosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearRegistroBancario(string EstadoBancario, DateTime FechaRegistro, int NumeroCuenta, string Observaciones, int CedulaJuridica)
        {
            var EstadoBancarioParam = new SqlParameter("@EstadoBancario", EstadoBancario);
            var FechaRegistroParam = new SqlParameter("@FechaRegistro", FechaRegistro);
            var NumeroCuentaParam = new SqlParameter("@NumeroCuenta", NumeroCuenta);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            await _context.Database.ExecuteSqlRawAsync("CrearRegistroBancario @EstadoBancario, @FechaRegistro, @NumeroCuenta, @Observaciones, @CedulaJuridica", EstadoBancarioParam, FechaRegistroParam, NumeroCuentaParam, ObservacionesParam, CedulaJuridicaParam);
        }

        public async Task ActualizarRegistroBancario(string IdRegistro, string EstadoBancario, DateTime FechaRegistro, int NumeroCuenta, string Observaciones)
        {
            var IdRegistroParam = new SqlParameter("@IdRegistro", IdRegistro);
            var EstadoBancarioParam = new SqlParameter("@EstadoBancario", EstadoBancario);
            var FechaRegistroParam = new SqlParameter("@FechaRegistro", FechaRegistro);
            var NumeroCuentaParam = new SqlParameter("@NumeroCuenta", NumeroCuenta);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            await _context.Database.ExecuteSqlRawAsync("ActualizarRegistroBancario @IdRegistro, @EstadoBancario, @FechaRegistro, @NumeroCuenta, @Observaciones", IdRegistroParam, EstadoBancarioParam, FechaRegistroParam, NumeroCuentaParam, ObservacionesParam);
        }

        public async Task EliminarRegistroBancario(string IdRegistro)
        {
            var IdRegistroParam = new SqlParameter("@IdRegistro", IdRegistro);
            await _context.Database.ExecuteSqlRawAsync("EliminarRegistroBancario @IdRegistro", IdRegistroParam);
        }

        public async Task<List<RegistroBancario>> MostrarRegistrosBancarios()
        {
            var registrosBancarios = await _context.RegistroBancario
                                        .FromSqlRaw("EXEC MostrarRegistrosBancarios")
                                        .ToListAsync();
            return registrosBancarios;
        }

        public async Task<RegistroBancario> ConsultarRegistrosBancarios(string IdRegistro)
        {
            var IdRegistroParam = new SqlParameter("@IdRegistro", IdRegistro);
            var resultado = await _context.RegistroBancario
                                          .FromSqlRaw("EXEC ConsultarRegistrosBancarios @IdRegistro", IdRegistroParam)
                                          .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
