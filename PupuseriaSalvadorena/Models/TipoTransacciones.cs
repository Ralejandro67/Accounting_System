using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class TipoTransacciones
    {
        [Key]
        public int IdTipo { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de transacción es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del tipo de transacción solo puede contener letras")]
        public string? TipoTransac { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        public int? IdMovimiento { get; set; }

        public string? NombreMov { get; set; }

        [Required(ErrorMessage = "El impuesto asociado es obligatorio")]
        public string? IdImpuesto { get; set; }

        public string? NombreImpuesto { get; set; }
    }
}
