using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Impuesto
    {
        [Key]
        public string? IdImpuesto { get; set; }

        [Required(ErrorMessage = "El nombre del impuesto es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del impuesto solo puede contener letras")]
        public string? NombreImpuesto { get; set; }

        [Required(ErrorMessage = "La tasa del impuesto es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "La tasa del impuesto debe ser un número que puede contener decimales")]
        public decimal Tasa { get; set; }

        [Required(ErrorMessage = "El estado del impuesto es obligatorio")]
        public bool Estado { get; set; }

        public string? Descripcion { get; set; }
    }
}
