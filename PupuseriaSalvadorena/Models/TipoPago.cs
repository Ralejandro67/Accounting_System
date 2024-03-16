using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class TipoPago
    {
        [Key]
        public int IdTipoPago { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de pago es obligatorio")]
        public string? NombrePago { get; set; }

        public bool Estado { get; set; }
    }
}
