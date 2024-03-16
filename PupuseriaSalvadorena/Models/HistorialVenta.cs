using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class HistorialVenta
    {
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "La cantidad vendida es obligatoria")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cantidad vendida solo puede contener números")]
        public int CantVenta { get; set; }

        [Required(ErrorMessage = "La fecha de venta es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El platillo vendido es obligatorio")]
        public int IdPlatillo { get; set; }

        public string? NombrePlatillo { get; set; }

        [Required(ErrorMessage = "La factura de venta asociada es requerida")]
        public int IdFacturaVenta { get; set; }

        [Required(ErrorMessage = "El tipo de venta es obligatorio")]
        public int IdTipoVenta { get; set; }

        public decimal PrecioVenta { get; set; }

        public decimal PrecioSubtotal { get; set; }
    }
}
