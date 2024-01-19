using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class EnvioFactura
    {
        [Key]
        public int IdEnvioFactura { get; set; }

        [Required(ErrorMessage = "La fecha de envío es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaEnvio { get; set; }

        [Required(ErrorMessage = "La factura asociada es obligatorio")]
        public int IdFacturaVenta { get; set; }
    }
}
