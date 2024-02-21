using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class ConciliacionBancaria
    {
        [Key]
        public string? IdConciliacion { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaConciliacion { get; set; }

        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El saldo bancario solo puede contener números")]
        public decimal SaldoBancario { get; set; }

        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El saldo en libros solo puede contener números")]
        public decimal SaldoLibro { get; set; }

        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "La diferencia entre saldos solo puede contener números")]
        public decimal Diferencia { get; set; }

        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "El registro bancario es obligatorio")]
        public string? IdRegistro { get; set; }

        public string? NumeroCuenta { get; set; }

        [Required(ErrorMessage = "El registro de libro es obligatorio")]
        public string? IdRegistroLibros { get; set; }

        public string? Descripcion { get; set; }
    }
}
