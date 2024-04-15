using System;
using System.Linq;
using System.Collections.Generic;
using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Services
{
    public class ServicioPronosticos
    {
        public List<PronosticoDiario> CalcularPronosticoHoltWinters(HistorialVenta[] ventasHistoricas)
        {
            DateTime fechaInicio = DateTime.Now.AddDays(1);
            double alpha = 0.9;
            double beta = 0.3;
            double gamma = 0.9;
            int estacionalidad = 7;

            var ventasPorDia = ventasHistoricas
                            .GroupBy(v => v.FechaVenta.Date)
                            .Select(g => new { Fecha = g.Key, TotalVentas = g.Sum(v => v.CantVenta)})
                            .OrderBy(v => v.Fecha)
                            .ToList();
            
            var pronosticosDiarios = new List<PronosticoDiario>();

            if (ventasPorDia.Count < estacionalidad)
            {
                return pronosticosDiarios;
            }

            double nivel = ventasPorDia[0].TotalVentas;
            double tendencia = ventasPorDia[1].TotalVentas - ventasPorDia[0].TotalVentas;

            var estacionalidades = new double[estacionalidad];
            for (int i = 0; i < estacionalidad; i++)
            {
                estacionalidades[i] = ventasPorDia[i].TotalVentas / nivel;
            }

            DayOfWeek startDayOfWeek = fechaInicio.DayOfWeek;
            DayOfWeek firstDataDayOfWeek = ventasPorDia[0].Fecha.DayOfWeek;
            int desplazamientoInicio = (int)(startDayOfWeek - firstDataDayOfWeek + 7) % 7;

            for (int i = estacionalidad; i < ventasPorDia.Count; i++)
            {
                var ventaActual = ventasPorDia[i].TotalVentas;

                double valorDesestacionalizado = ventaActual / estacionalidades[(i + desplazamientoInicio) % estacionalidad];
                double nivelAnterior = nivel;

                nivel = alpha * valorDesestacionalizado + (1 - alpha) * (nivel + tendencia);
                tendencia = beta * (nivel - nivelAnterior) + (1 - beta) * tendencia;
                estacionalidades[(i + desplazamientoInicio) % estacionalidad] = gamma * (ventaActual / nivel) + (1 - gamma) * estacionalidades[(i + desplazamientoInicio) % estacionalidad];

                double pronostico = (nivel + tendencia) * estacionalidades[(i + desplazamientoInicio) % estacionalidad];
                pronosticosDiarios.Add(new PronosticoDiario { CantVenta = (int)Math.Round(pronostico) });
            }

            int DiaInicio = (int)fechaInicio.DayOfWeek;
            pronosticosDiarios.RemoveRange(0, DiaInicio);

            return pronosticosDiarios;
        }
    }

    public class PronosticoDiario
    {
        public int CantVenta { get; set; }
    }
}
