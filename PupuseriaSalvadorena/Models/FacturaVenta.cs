using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class FacturaVenta
    {
        [Key]
        public int IdFacturaVenta { get; set; }

        [Required(ErrorMessage = "La cedula jurídica es obligatoria")]
        public long CedulaJuridica { get; set; }

        [Required(ErrorMessage = "El consecutivo de la factura es obligatorio")]
        public int Consecutivo { get; set; }

        [Required(ErrorMessage = "La clave de la factura es obligatorio")]
        public int Clave { get; set; }

        [Required(ErrorMessage = "La fecha de la factura es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFactura { get; set; }

        [Required(ErrorMessage = "El sub total de la factura es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El sub total de la factura solo puede contener números")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "El monto total de la factura es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El monto total de la factura solo puede contener números")]
        public decimal TotalVenta { get; set; }

        [Required(ErrorMessage = "El tipo de pago de la factura es obligatorio")]
        public int IdTipoPago { get; set; }

        [Required(ErrorMessage = "El tipo de factura es obligatorio")]
        public int IdTipoFactura { get; set; }
    }
}
