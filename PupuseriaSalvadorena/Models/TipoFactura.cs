using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class TipoFactura
    {
        [Key]
        public int IdTipoFactura { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de factura es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del tipo de factura solo puede contener letras")]
        public string? NombreFactura { get; set; }

        [Required(ErrorMessage = "El estado del tipo de factura es obligatorio")]
        public bool Estado { get; set; }
    }
}
