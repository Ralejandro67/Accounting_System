using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupuseriaSalvadorena.Models
{
    public class FacturaCompra
    {
        [Key]
        public string? IdFacturaCompra { get; set; }

        public byte[]? FacturaCom { get; set; }

        [Required(ErrorMessage = "La fecha de la factura de compra es obligatoria.")]
        public DateTime FechaFactura { get; set; }

        [Required(ErrorMessage = "El monto de la factura de compra es obligatorio.")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El monto de la factura de compra solo puede contener números")]
        public decimal TotalCompra { get; set; }

        [Required(ErrorMessage = "Es necesaria una desccripcion sobre la compra.")]
        public string? DetallesCompra { get; set; }

        [Required(ErrorMessage = "El tipo de pago es obligatorio.")]
        public int? IdTipoPago { get; set; }

        [Required(ErrorMessage = "El tipo de factura es obligatorio.")]
        public int IdTipoFactura { get; set; }

        [Required(ErrorMessage = "La materia prima es obligatoria.")]
        public int? IdMateriaPrima { get; set; }

        public string? NombreMateriaPrima { get; set; }

        public string? NombrePago { get; set; }

        public string? NombreFactura { get; set; }

        [NotMapped]
        public string? IdProveedor { get; set; }

        [NotMapped]
        public DateTime FechaVencimiento { get; set; }

        [NotMapped]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El peso de la materia prima solo puede contener números.")]
        public decimal Peso { get; set; }

        [NotMapped]
        public int Cantidad { get; set; }

        [NotMapped]
        public IFormFile? FacturaDoc { get; set; }

        [NotMapped]
        public bool Activo { get; set; }

        [NotMapped]
        public string? TipoReporte { get; set; }

        [NotMapped]
        public DateTime FechaReporte { get; set; }
    }
}
