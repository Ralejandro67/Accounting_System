﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using System.Data;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class NegociosRep : INegociosRep
    {
        private readonly MiDbContext _context;

        public NegociosRep(MiDbContext context)
        {
            _context = context;
        }

        public async Task CrearNegocio(long CedulaJuridica, string NombreEmpresa, int IdDireccion, int IdTelefono)
        {
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            var NombreEmpresaParam = new SqlParameter("@NombreEmpresa", NombreEmpresa);
            var IdDireccionParam = new SqlParameter("@IdDireccion", IdDireccion);
            var IdTelefonoParam = new SqlParameter("@IdTelefono", IdTelefono);
            await _context.Database.ExecuteSqlRawAsync("CrearNegocio @CedulaJuridica, @NombreEmpresa, @IdDireccion, @IdTelefono", CedulaJuridicaParam, NombreEmpresaParam, IdDireccionParam, IdTelefonoParam);
        }

        public async Task ActualizarNegocio(long CedulaJuridica, string NombreEmpresa)
        {
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            var NombreEmpresaParam = new SqlParameter("@NombreEmpresa", NombreEmpresa);
            await _context.Database.ExecuteSqlRawAsync("ActualizarNegocio @CedulaJuridica, @NombreEmpresa", CedulaJuridicaParam, NombreEmpresaParam);
        }

        public async Task EliminarNegocio(long CedulaJuridica)
        {
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            await _context.Database.ExecuteSqlRawAsync("EliminarNegocio @CedulaJuridica", CedulaJuridicaParam);
        }

        public async Task<Negocio> MostrarNegocio()
        {
            var negocios = await _context.Negocio
                                        .FromSqlRaw("EXEC MostrarNegocio")
                                        .ToListAsync();

            return negocios.FirstOrDefault();
        }

        public async Task<Negocio> ConsultarNegocio(long CedulaJuridica)
        {
            var CedulaJuridicaParam = new SqlParameter("@CedulaJuridica", CedulaJuridica);
            var negocio = await _context.Negocio
                                        .FromSqlRaw("EXEC ConsultarNegocio @CedulaJuridica", CedulaJuridicaParam)
                                        .ToListAsync();

            return negocio.FirstOrDefault();
        }

        public async Task<long> ConsultarNegocio()
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "ConsultarNegocioDetalles";
                command.CommandType = CommandType.StoredProcedure;

                var CedulaJuridica = new SqlParameter("@CedulaJuridica", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(CedulaJuridica);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();

                return (long)CedulaJuridica.Value;
            }
        }
    }
}
