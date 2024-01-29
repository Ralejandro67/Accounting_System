using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class HistorialCompra
    {
        public string? IdCompra { get; set; }

        [Required(ErrorMessage = "El id del producto es obligatorio")]
        public int IdMateriaPrima { get; set; }

        [Required(ErrorMessage = "La cantidad de materia prima es obligatoria")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cantidad de materia prima solo puede contener números")]
        public int CantCompra { get; set; }

        [Required(ErrorMessage = "El precio de la materia prima es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El precio de la materia prima solo puede contener números")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El peso de la materia prima es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El peso de la materia prima solo puede contener números")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "La fecha de compra es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage = "El id de la factura asociada es obligatoria")]
        public string? IdFacturaCompra { get; set; }
    }
}
