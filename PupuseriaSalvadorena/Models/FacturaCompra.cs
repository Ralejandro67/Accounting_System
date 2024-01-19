using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class FacturaCompra
    {
        [Key]
        public string? IdFacturaCompra { get; set; }

        [Required(ErrorMessage = "La factura de compra es obligatoria")]
        public string? FacturaCom { get; set; }

        [Required(ErrorMessage = "La fecha de la factura de compra es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFactura { get; set; }

        [Required(ErrorMessage = "El monto de la factura de compra es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El monto de la factura de compra solo puede contener números")]
        public decimal TotalCompra { get; set; }

        public string? DetallesCompra { get; set; }

        [Required(ErrorMessage = "El tipo de pago es obligatorio")]
        public int IdTipoPago { get; set; }

        [Required(ErrorMessage = "El tipo de factura es obligatorio")]
        public int IdTipoFactura { get; set; }
    }
}
