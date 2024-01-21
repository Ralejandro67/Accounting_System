using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class DeclaracionImpuesto
    {
        [Key]
        public string? IdDeclaracionImpuesto { get; set; }

        [Required(ErrorMessage = "La cedula jurídica es obligatoria")]
        public long CedulaJuridica { get; set; }

        [Required(ErrorMessage = "La fecha de inicio de la declaración es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin de la declaración es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { get; set; }

        [Required(ErrorMessage = "El monto de ingresos es obligatorio")]
        public int MontoTotalIngresos { get; set; }

        [Required(ErrorMessage = "El monto de gastos es obligatorio")]
        public int MontoTotalEgresos { get; set; }

        [Required(ErrorMessage = "El monto de impuestos es obligatorio")]
        public int MontoTotalImpuestos { get; set; }

        public string? Observaciones { get; set; }
    }
}
