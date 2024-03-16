using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class TipoVenta
    {
        [Key]
        public int IdTipoVenta { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de venta es obligatorio")]
        public string? NombreVenta { get; set; }

        public bool Estado { get; set; }
    }
}
