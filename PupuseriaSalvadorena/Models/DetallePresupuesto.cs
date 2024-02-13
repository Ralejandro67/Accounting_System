using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class DetallePresupuesto
    {
        public string? IdPresupuesto { get; set; }

        public string? IdRegistroLibros { get; set; }

        public int IdTransaccion { get; set; }

        [Required(ErrorMessage = "La fecha de la transacción es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        public string? Observaciones { get; set; }

        public decimal Monto { get; set; }
    }
}
