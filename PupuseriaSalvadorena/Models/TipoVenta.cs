using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class TipoVenta
    {
        [Key]
        public int IdTipoVenta { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de venta es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del tipo de venta solo puede contener letras")]
        public string? NombreVenta { get; set; }

        [Required(ErrorMessage = "El estado del tipo de venta es obligatorio")]
        public bool Estado { get; set; }
    }
}
