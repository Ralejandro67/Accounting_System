using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class AlertaCuentaPagar
    {
        [Key]
        public int IdAlerta { get; set; }

        [Required(ErrorMessage = "El mensaje de la alerta es obligatorio")]
        public string? Mensaje { get; set; }

        [Required(ErrorMessage = "La fecha de la alerta es obligatoria")]
        public DateTime FechaMensaje { get; set; }

        [Required(ErrorMessage = "El id de la cuenta por pagar es obligatorio")]
        public string? IdCuentaPagar { get; set; }

        [Required(ErrorMessage = "El estado de la alerta es obligatorio")]
        public bool Leido { get; set; }
    }
}
