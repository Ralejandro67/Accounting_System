using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class CorreoElectronico
    {
        [Key]
        public int IdCorreoElectronico { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo no es válido")]
        public string? Correo { get; set; }
    }
}
