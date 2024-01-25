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

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado de conciliacion es obligatorio")]
        public bool Conciliado { get; set; }
    }
}
