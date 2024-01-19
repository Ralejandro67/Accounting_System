using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Persona
    {
        [Key]
        public string? IdPersona { get; set; }

        [Required(ErrorMessage = "La cedula es obligatoria")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cedula solo puede contener números")]
        public int Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El apellido solo puede contener letras")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNac { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        public int IdCorreoElectronico { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public int IdDireccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public int IdTelefono { get; set; }
    }
}
