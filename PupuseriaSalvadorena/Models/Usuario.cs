using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupuseriaSalvadorena.Models
{
    public class Usuario
    {
        [Key]
        public string? IdUsuario { get; set; }

        public int IdCorreoElectronico { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string? CorreoElectronico { get; set; }

        public string? IdPersona { get; set; }

        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Contrasena { get; set; }

        public bool Estado { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int? IdRol { get; set; }

        public string? NombreRol { get; set; }

        public int IdTelefono { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        public long? Cedula { get; set; }

        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido del usuario es obligatorio.")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNac { get; set; }

        public int IdDireccion { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string? Detalles { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public int? IdDistrito { get; set; }

        public string? NombreDistrito { get; set; }

        public int? IdCanton { get; set; }

        public string? NombreCanton { get; set; }

        public int? IdProvincia { get; set; }

        public string? NombreProvincia { get; set; }

        [NotMapped]
        public string? NuevaContrasena { get; set; }
    }
}
