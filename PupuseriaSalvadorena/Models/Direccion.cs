using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Direccion
    {
        [Key]
        public int IdDireccion { get; set; }

        public bool Estado { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "La dirección solo puede contener letras y números")]
        public string? Detalles { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio")]
        public int IdDistrito { get; set; }

        public string? NombreDistrito { get; set; }

    }
}
