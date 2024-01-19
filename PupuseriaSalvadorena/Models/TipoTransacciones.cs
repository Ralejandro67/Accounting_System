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
    }
}
