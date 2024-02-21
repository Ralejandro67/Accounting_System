using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class DeclaracionImpuesto
    {
        [Key]
        public string? IdDeclaracionImpuesto { get; set; }

        public long CedulaJuridica { get; set; }

        [Required(ErrorMessage = "La fecha de registro de la declaración es obligatorio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El trimestre de la declaración es obligatorio")]
        public string? Trimestre { get; set; }

        public decimal MontoRenta { get; set; }

        public decimal MontoIVA { get; set; }

        public decimal MontoTotalImpuestos { get; set; }

        public decimal MontoTotal { get; set; }

        public string? Observaciones { get; set; }
    }
}
