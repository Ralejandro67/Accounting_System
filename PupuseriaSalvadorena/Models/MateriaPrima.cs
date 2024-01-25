using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class MateriaPrima
    {
        [Key]
        public int IdMateriaPrima { get; set; }

        [Required(ErrorMessage = "El nombre de la materia prima es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre de la materia prima solo puede contener letras")]
        public string? NombreMateriaPrima { get; set; }

        [Required(ErrorMessage = "El proveedor de la materia prima es obligatorio")]
        public string? IdProveedor { get; set; }

        public string? Proveedor { get; set; }
    }
}
