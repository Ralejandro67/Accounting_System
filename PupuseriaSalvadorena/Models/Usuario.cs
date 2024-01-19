using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Usuario
    {
        [Key]
        public string? IdUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "La contraseña debe tener entre 8 y 15 caracteres, al menos un dígito, al menos una minúscula, al menos una mayúscula y al menos un caracter especial")]
        public string? Contrasena { get; set; }

        public bool Estado { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }

        public string? IdPersona { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int IdRol { get; set; }

    }
}
