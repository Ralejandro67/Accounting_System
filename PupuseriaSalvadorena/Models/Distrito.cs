using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Distrito
    {
        [Key]
        public int IdDistrito { get; set; }

        [Required(ErrorMessage = "El nombre del distrito es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del distrito solo puede contener letras")]
        public string? NombreDistrito { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio")]
        public int IdCanton { get; set; }

        public string? NombreCanton { get; set; }
    }
}
