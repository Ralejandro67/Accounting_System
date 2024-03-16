using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Negocio
    {
        [Key]   
        public long CedulaJuridica { get; set; }

        [Required(ErrorMessage = "El nombre del negocio es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del negocio solo puede contener letras")]
        public string? NombreEmpresa { get; set; }

        [Required(ErrorMessage = "El teléfono del negocio es obligatorio")]
        public int IdTelefono { get; set; }

        public int Telefono { get; set; }

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

        public bool Estado { get; set; }
    }
}
