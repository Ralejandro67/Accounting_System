using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class TipoPago
    {
        [Key]
        public int IdTipoPago { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de pago es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del tipo de pago solo puede contener letras")]
        public string? NombrePago { get; set; }

        [Required(ErrorMessage = "El estado del tipo de pago es obligatorio")]
        public bool Estado { get; set; }
    }
}
