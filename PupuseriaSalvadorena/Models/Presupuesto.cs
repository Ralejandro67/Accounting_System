using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Presupuesto
    {
        [Key]
        public string? IdPresupuesto { get; set; }

        [Required(ErrorMessage = "El nombre del presupuesto es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del presupuesto solo puede contener letras")]
        public string? NombreP { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El saldo usado es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El saldo usado solo puede contener números")]
        public decimal SaldoUsado { get; set; }

        [Required(ErrorMessage = "El saldo del presupuesto es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El saldo del presupuesto solo puede contener números")]
        public decimal SaldoPresupuesto { get; set; }

        [Required(ErrorMessage = "El estado del presupuesto es obligatorio")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "El tipo de presupuesto es obligatorio")]
        public int IdCategoriaP { get; set; }
    }
}
