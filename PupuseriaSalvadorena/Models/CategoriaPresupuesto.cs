using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class CategoriaPresupuesto
    {
        [Key]
        public int IdCategoriaP { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre de la categoría solo puede contener letras")]
        public string? NombreCategoriaP { get; set; }

        [Required(ErrorMessage = "El estado de la categoría es obligatorio")]
        public bool Estado { get; set; }
    }
}
