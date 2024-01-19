using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del rol solo puede contener letras")]
        public string? NombreRol { get; set; }

        [Required(ErrorMessage = "El estado del rol es obligatorio")]
        public bool Estado { get; set; }
    }
}
