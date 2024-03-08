using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Persona
    {
        [Key]
        public string? IdPersona { get; set; }

        public long Cedula { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public DateTime FechaNac { get; set; }

        public int IdCorreoElectronico { get; set; }

        public int IdDireccion { get; set; }

        public int IdTelefono { get; set; }
    }
}
