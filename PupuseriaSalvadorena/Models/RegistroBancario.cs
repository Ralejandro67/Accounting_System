using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupuseriaSalvadorena.Models
{
    public class RegistroBancario
    {
        [Key]
        public string? IdRegistro { get; set; }

        [Required(ErrorMessage = "La fecha de registro es obligatoria.")]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "El monto inicial es obligatorio")]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "El monto inicial solo puede contener números.")]
        public decimal SaldoInicial { get; set; }

        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        public string? NumeroCuenta { get; set; }

        [Required(ErrorMessage = "Es necesaria una desccripcion para la cuenta bancaria.")]
        public string? Observaciones { get; set; }

        public long CedulaJuridica { get; set; }

        public string? NombreEmpresa { get; set; }
    }
}
