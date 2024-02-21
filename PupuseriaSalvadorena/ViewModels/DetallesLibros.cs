using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class DetallesLibros
    {
        public RegistroLibro RegistroLibro { get; set; }
        public List<DetalleTransaccion> DetallesTransacciones { get; set; }
    }
}
