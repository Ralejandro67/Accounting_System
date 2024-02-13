using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Pronostico
    {
        [Key]
        public int IdPronostico { get; set; }

        public int IdPlatillo { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public int CantTotalProd { get; set; }

        public decimal TotalVentas { get; set; }

        public string? PronosticoDoc { get; set; }
    }
}
