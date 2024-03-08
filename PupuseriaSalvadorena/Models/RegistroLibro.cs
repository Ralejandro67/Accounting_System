using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class RegistroLibro
    {
        [Key]
        public string? IdRegistroLibros { get; set; }

        [Required(ErrorMessage = "La fecha de registro es obligatoria")]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El monto solo puede contener números")]
        public decimal MontoTotal { get; set; }

        [Required(ErrorMessage = "El nombre del libro contable es obligatorio.")]
        public string? Descripcion { get; set; }

        public bool Conciliado { get; set; }
    }
}
