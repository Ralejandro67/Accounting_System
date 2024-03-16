using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Impuesto
    {
        [Key]
        public string? IdImpuesto { get; set; }

        [Required(ErrorMessage = "El nombre del impuesto es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del impuesto solo puede contener letras")]
        [Display(Name = "Nombre del impuesto")]
        public string? NombreImpuesto { get; set; }

        [Required(ErrorMessage = "La tasa del impuesto es obligatorio")]
        [Display(Name = "Tasa de impuesto")]
        public decimal? Tasa { get; set; }

        [Required(ErrorMessage = "El estado del impuesto es obligatorio")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "Es necesaria una desccripcion para el impuesto")]
        public string? Descripcion { get; set; }
    }
}
