﻿using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IRegistrosBancariosRep
    {
        Task CrearRegistroBancario(byte[] EstadoBancario, DateTime FechaRegistro, decimal SaldoInicial, int NumeroCuenta, string Observaciones, long CedulaJuridica);
        Task ActualizarRegistroBancario(string IdRegistro, byte[] EstadoBancario, DateTime FechaRegistro, decimal SaldoInicial, int NumeroCuenta, string Observaciones);
        Task EliminarRegistroBancario(string IdRegistro);
        Task<List<RegistroBancario>> MostrarRegistrosBancarios();
        Task<RegistroBancario> ConsultarRegistrosBancarios(string IdRegistro);
    }
}