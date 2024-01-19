using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Provincia
    {
        [Key]
        public int IdProvincia { get; set; }

        [Required(ErrorMessage = "El nombre de la provincia es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre de la provincia solo puede contener letras")]
        public string? NombreProvincia { get; set; }
    }
}
