using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class TipoMovimiento
    {
        [Key]
        public int IdMovimiento { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de movimiento es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del tipo de movimiento solo puede contener letras")]
        public string? NombreMov { get; set; }
    }
}
