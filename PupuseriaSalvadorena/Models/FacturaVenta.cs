using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupuseriaSalvadorena.Models
{
    public class FacturaVenta
    {
        [Key]
        public int IdFacturaVenta { get; set; }

        public long CedulaJuridica { get; set; }

        public decimal Consecutivo { get; set; }

        public DateTime FechaFactura { get; set; }

        [Required(ErrorMessage = "El sub total de la factura es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El sub total de la factura solo puede contener números")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "El monto total de la factura es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El monto total de la factura solo puede contener números")]
        public decimal TotalVenta { get; set; }

        [Required(ErrorMessage = "El tipo de pago de la factura es obligatorio.")]
        public int? IdTipoPago { get; set; }

        [Required(ErrorMessage = "El tipo de factura es obligatorio")]
        public int? IdTipoFactura { get; set; }

        public string? NombrePago { get; set; }

        public string? NombreFactura { get; set; }

        public bool Estado { get; set; }

        [NotMapped]
        public string? TipoId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "La identificación del cliente es obligatoria.")]
        public long? Identificacion { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        public string? NombreCliente { get; set; }

        [NotMapped]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        public string? CorreoElectronico { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El teléfono es requerido.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El teléfono solo puede contener números.")]
        public int? Telefono { get; set; }

        [NotMapped]
        public int[]? IdPlatillo { get; set; }

        [NotMapped]
        public int[]? CantVenta { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El tipo de venta es obligatorio.")]
        public int? IdTipoVenta { get; set; }

        [NotMapped]
        public bool FacturaElectronica { get; set; }

        [NotMapped]
        public string? TipoReporte { get; set; }

        [NotMapped]
        public DateTime FechaReporte { get; set; }
    }
}
