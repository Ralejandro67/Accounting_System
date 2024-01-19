﻿using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Repositorios.Interfaces
{
    public interface IRegistroLibrosRep
    {
        Task CrearRegistroLibros(DateTime FechaRegistro, decimal MontoTotal, string Descripcion);
        Task ActualizarRegistroLibros(string IdRegistroLibros, decimal MontoTotal, string Descripcion);
        Task EliminarRegistroLibros(string IdRegistroLibros);
        Task<List<RegistroLibro>> MostrarRegistrosLibros();
        Task<RegistroLibro> ConsultarRegistrosLibros(string IdRegistroLibros);
        Task<int> CrearLibroRecurrente(DateTime FechaRegistro, decimal MontoTotal, string Descripcion);
    }
}
