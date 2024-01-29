using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Proveedor
    {
        [Key]
        public string? IdProveedor { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del proveedor solo puede contener letras")]
        public string? NombreProveedor { get; set; }

        [Required(ErrorMessage = "El apellido del proveedor es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El apellido del proveedor solo puede contener letras")]
        public string? ApellidoProveedor { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El número de teléfono solo puede contener números")]
        public int Telefono { get; set; }

        public string? ProveedorCompleto { get; set; }
    }
}
