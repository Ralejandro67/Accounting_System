using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class RegistroBancario
    {
        [Key]
        public string? IdRegistroBancario { get; set; }

        [Required(ErrorMessage = "El estado bancario es obligatorio")]
        public string? EstadoBancario { get; set; }

        [Required(ErrorMessage = "La fecha de registro es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El número de cuenta solo puede contener números")]
        public int NumeroCuenta { get; set; }

        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "La cedula jurídica es obligatoria")]
        public int CedulaJuridica { get; set; }
    }
}
