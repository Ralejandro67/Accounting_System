using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class CuentaPagar
    {
        [Key]
        public string? IdCuentaPagar { get; set; }

        [Required(ErrorMessage = "La fecha de creacion de la cuenta por pagar es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento de la cuenta por pagar es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El monto pagado es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "El monto pagado no es válido")]
        public decimal TotalPagado { get; set; }

        [Required(ErrorMessage = "La factura asociada es obligatoria")]
        public string? IdFacturaCompra { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public string? IdProveedor { get; set; }
    }
}
