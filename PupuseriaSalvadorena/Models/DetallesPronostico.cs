using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class DetallesPronostico
    {
        public int IdDetallePronostico { get; set; }

        public int IdPronostico { get; set; }

        public DateTime FechaPronostico { get; set; }

        public int PCantVenta { get; set; }

        public decimal PValorVenta { get; set; }
    }
}
