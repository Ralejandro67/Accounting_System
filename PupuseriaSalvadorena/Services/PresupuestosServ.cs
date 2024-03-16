using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Services
{
    public class PresupuestosServ : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public PresupuestosServ(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(EstadoPresupuesto, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void EstadoPresupuesto(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var presupuestosRep = scope.ServiceProvider.GetRequiredService<IPresupuestoRep>();
                var presupuestos = presupuestosRep.MostrarPresupuestos().Result;

                foreach (var presupuesto in presupuestos)
                {
                    if (presupuesto.Estado && presupuesto.FechaInicio >= presupuesto.FechaFin)
                    {
                        presupuestosRep.ActualizarPresupuesto(presupuesto.IdPresupuesto, presupuesto.NombreP, presupuesto.FechaInicio, presupuesto.FechaFin, presupuesto.Descripcion, presupuesto.SaldoUsado, presupuesto.SaldoPresupuesto, false, presupuesto.IdCategoriaP.Value);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
