using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Pronostico
    {
        [Key]
        public int IdPronostico { get; set; }

        [Required(ErrorMessage = "El platillo asociado al pronóstico es obligatorio")]
        public string? IdPlatillo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio del pronostico es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin del pronostico es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { get; set; }

        [Required(ErrorMessage = "La cantidad de platillos producidos es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cantidad de platillos producidos solo puede contener números")]
        public int CantTotalProd { get; set; }

        [Required(ErrorMessage = "El monto total de platillos vendidos es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El monto total de platillos vendidos solo puede contener números")]
        public decimal TotalVentas { get; set; }

        public string? PronosticoDoc { get; set; }
    }
}
