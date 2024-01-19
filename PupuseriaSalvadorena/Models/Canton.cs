using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Canton
    {
        [Key]
        public int IdCanton { get; set; }

        [Required(ErrorMessage = "El nombre del cantón es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del cantón solo puede contener letras")]
        public string? NombreCanton { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria")]
        public int IdProvincia { get; set; }
    }
}
