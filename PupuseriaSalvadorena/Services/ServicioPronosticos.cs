using System;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.LinearAlgebra;
using PupuseriaSalvadorena.Models;

namespace PupuseriaSalvadorena.Services
{
    public class ServicioPronosticos
    {
        public DescomposicionEstacional DescomposicionMutiplicativa(HistorialVenta[] ventasHistoricas)
        {
            var ventasDiarias = ventasHistoricas
                                .GroupBy(v => v.FechaVenta.Date)
                                .Select(g => new { Fecha = g.Key, TotalVentas = g.Sum(v => v.CantVenta) })
                                .ToList();

            var ventasTotales = ventasDiarias.Select(v => (double)v.TotalVentas).ToArray();

            var trend = Fit.Line(Generate.LinearRange(0, ventasTotales.Length, 1), ventasTotales).Item2;

            var seasonal = new double[ventasTotales.Length];
            for (int i = 0; i < ventasTotales.Length; i++)
            {
                seasonal[i] = ventasTotales[i] / trend;
            }

            var residuals = new double[ventasTotales.Length];
            for (int i = 0; i < ventasTotales.Length; i++)
            {
                residuals [i] = ventasTotales[i] / (trend * seasonal[i]);
            }


            return new DescomposicionEstacional
            {
                Trend = trend,
                Seasonal = seasonal,
                Residuals = residuals
            };
        }
    }

    public class DescomposicionEstacional
    {
        public double Trend { get; set; }
        public double[] Seasonal { get; set; }
        public double[] Residuals { get; set; }
    }
}
