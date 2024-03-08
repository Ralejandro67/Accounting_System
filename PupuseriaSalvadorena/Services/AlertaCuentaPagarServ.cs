using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Services
{
    public class AlertaCuentaPagarServ : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public AlertaCuentaPagarServ(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var cuentaPagarRep = scope.ServiceProvider.GetRequiredService<ICuentaPagarRep>();
                var alertaCuentaPagarRep = scope.ServiceProvider.GetRequiredService<IAlertaCuentaPagarRep>();
                var cuentaPagar = cuentaPagarRep.MostrarCuentasPagar().Result;

                foreach (var cuenta in cuentaPagar)
                {
                    if (cuenta.Estado && DateTime.Now < cuenta.FechaVencimiento && cuenta.FechaVencimiento <= DateTime.Now.AddDays(2))
                    {
                        var mensaje = "Cuenta: " + cuenta.IdCuentaPagar + " esta proxima a vencer.";
                        alertaCuentaPagarRep.CrearAlertaCuentaPagar(mensaje, DateTime.Now, cuenta.IdCuentaPagar, false).Wait();
                    }

                    if (cuenta.Estado && DateTime.Now > cuenta.FechaVencimiento)
                    {
                        var mensaje = "Cuenta: " + cuenta.IdCuentaPagar + " aun no se ha pagado.";
                        alertaCuentaPagarRep.CrearAlertaCuentaPagar(mensaje, DateTime.Now, cuenta.IdCuentaPagar, false).Wait();
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
