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

        public async Task CrearRegistroBancario(byte[] EstadoBancario, DateTime FechaRegistro, decimal SaldoInicial, int NumeroCuenta, string Observaciones, long CedulaJuridica)
        {
            var EstadoBancarioParam = new SqlParameter("@EstadoBancario", EstadoBancario);
            var FechaRegistroParam = new SqlParameter("@FechaRegistro", FechaRegistro);
            var SaldoInicialParam = new SqlParameter("@SaldoInicial", SaldoInicial);
            var NumeroCuentaParam = new SqlParameter("@NumeroCuenta", NumeroCuenta);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            await _context.Database.ExecuteSqlRawAsync("CrearRegistroBancario @EstadoBancario, @FechaRegistro, @SaldoInicial, @NumeroCuenta, @Observaciones, @CedulaJuridica", EstadoBancarioParam, FechaRegistroParam, SaldoInicialParam, NumeroCuentaParam, ObservacionesParam, CedulaJuridicaParam);
        }

        public async Task ActualizarRegistroBancario(string IdRegistro, byte[] EstadoBancario, DateTime FechaRegistro, decimal SaldoInicial, int NumeroCuenta, string Observaciones)
        {
            var IdRegistroParam = new SqlParameter("@IdRegistro", IdRegistro);
            var EstadoBancarioParam = new SqlParameter("@EstadoBancario", EstadoBancario);
            var FechaRegistroParam = new SqlParameter("@FechaRegistro", FechaRegistro);
            var SaldoInicialParam = new SqlParameter("@SaldoInicial", SaldoInicial);
            var NumeroCuentaParam = new SqlParameter("@NumeroCuenta", NumeroCuenta);
            var ObservacionesParam = new SqlParameter("@Observaciones", Observaciones);
            await _context.Database.ExecuteSqlRawAsync("ActualizarRegistroBancario @IdRegistro, @EstadoBancario, @FechaRegistro, @SaldoInicial, @NumeroCuenta, @Observaciones", IdRegistroParam, EstadoBancarioParam, FechaRegistroParam, SaldoInicialParam, NumeroCuentaParam, ObservacionesParam);
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
