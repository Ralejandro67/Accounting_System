using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Models;
using PupuseriaSalvadorena.Repositorios.Interfaces;

namespace PupuseriaSalvadorena.Repositorios.Implementaciones
{
    public class AlertaCuentaPagarRep : IAlertaCuentaPagarRep
    {
        private readonly MiDbContext _context;
        
        public AlertaCuentaPagarRep(MiDbContext context)
        {
            _context = context;
        }
        
        public async Task CrearAlertaCuentaPagar(string Mensaje, DateTime FechaMensaje, string IdCuentaPagar, bool Leido)
        {
            var MensajeParam = new SqlParameter("@Mensaje", Mensaje);
            var FechaMensajeParam = new SqlParameter("@FechaMensaje", FechaMensaje);
            var IdCuentaPagarParam = new SqlParameter("@IdCuentaPagar", IdCuentaPagar);
            var LeidoParam = new SqlParameter("@Leido", Leido);
            await _context.Database.ExecuteSqlRawAsync("CrearAlertaCuentaPagar @Mensaje, @FechaMensaje, @IdCuentaPagar, @Leido", MensajeParam, FechaMensajeParam, IdCuentaPagarParam, LeidoParam);
        }
        
        public async Task ActualizarAlertaCuentaPagarID(string IdCuentaPagar, bool Leido)
        {
            var IdCuentaPagarParam = new SqlParameter("@IdCuentaPagar", IdCuentaPagar);
            var LeidoParam = new SqlParameter("@Leido", Leido);
            await _context.Database.ExecuteSqlRawAsync("ActualizarAlertaCuentaPagarID @IdCuentaPagar, @Leido", IdCuentaPagarParam, LeidoParam);
        }
        
        public async Task EliminarAlertaCuentaPagar(int IdAlerta)
        {
            var IdAlertaParam = new SqlParameter("@IdAlerta", IdAlerta);
            await _context.Database.ExecuteSqlRawAsync("EliminarAlertaCuentaPagar @IdAlerta", IdAlertaParam);
        }

        public async Task<List<AlertaCuentaPagar>> MostrarAlertaCuentaPagar()
        {
            var alertasCuentaPagar = await _context.AlertaCuentaPagar.FromSqlRaw("MostrarAlertaCuentaPagar").ToListAsync();
            return alertasCuentaPagar;
        }

        public async Task<AlertaCuentaPagar> ConsultarAlertaCuentaPagar(int IdAlerta)
        {
            var IdAlertaParam = new SqlParameter("@IdAlerta", IdAlerta);
            var alertaCuentaPagar = await _context.AlertaCuentaPagar.FromSqlRaw("ConsultarAlertaCuentaPagar @IdAlerta", IdAlertaParam).ToListAsync();

            return alertaCuentaPagar.FirstOrDefault();
        }

        public async Task<List<AlertaCuentaPagar>> MostrarAlertasNoLeidas()
        {
            var alertasCuentaPagar = await _context.AlertaCuentaPagar.FromSqlRaw("MostrarAlertasNoLeidas").ToListAsync();
            return alertasCuentaPagar;
        }

        public async Task<List<AlertaCuentaPagar>> MostrarAlertasLeidas()
        {
            var alertasCuentaPagar = await _context.AlertaCuentaPagar.FromSqlRaw("MostrarAlertasLeidas").ToListAsync();
            return alertasCuentaPagar;
        }

        public async Task ActualizarAlertasNoLeidas()
        {
            await _context.Database.ExecuteSqlRawAsync("ActualizarAlertasNoLeidas");
        }

        public async Task EliminarAlertasLeidas()
        {
            await _context.Database.ExecuteSqlRawAsync("EliminarAlertasLeidas");
        }
    }
}
