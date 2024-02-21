using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class DetallesConciliacion
    {
        public ConciliacionBancaria ConciliacionBancaria { get; set; }
        public RegistroLibro RegistroLibro { get; set; }
        public List<DetalleTransaccion> DetallesTransacciones { get; set; }
    }
}
