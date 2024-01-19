using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Telefonos
    {
        [Key]
        public int IdTelefono { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El número de teléfono solo puede contener números")]
        public int Telefono { get; set; }

        public bool Estado { get; set; }
    }
}
