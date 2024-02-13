using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Platillo
    {
        [Key]
        public int IdPlatillo { get; set; }

        [Required(ErrorMessage = "El nombre del platillo es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del platillo solo puede contener letras")]
        public string? NombrePlatillo { get; set; }

        [Required(ErrorMessage = "El costo de produccion del platillo es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "El costo de produccion del platillo solo puede contener números")]
        public decimal CostoProduccion { get; set; }

        [Required(ErrorMessage = "El precio del platillo es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "El precio del platillo solo puede contener números")]
        public decimal PrecioVenta { get; set; }
    }
}
