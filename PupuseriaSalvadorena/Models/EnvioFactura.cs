using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class EnvioFactura
    {
        [Key]
        public int IdEnvioFactura { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaEnvio { get; set; }

        [Required(ErrorMessage = "La factura asociada es obligatoria")]
        public int IdFacturaVenta { get; set; }

        [Required(ErrorMessage = "La cedula del receptor es obligatoria")]
        public long Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre del receptor es obligatorio")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El correo del receptor es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo del receptor no es válido")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El teléfono del receptor es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El teléfono del receptor solo puede contener números")]
        public int Telefono { get; set; }
    }
}
