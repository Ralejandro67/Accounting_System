using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class ConciliacionPDF
    {
        public Negocio Negocio { get; set; }
        public RegistroLibro RegistroLibro { get; set; }
        public RegistroBancario RegistroBancario { get; set; }
        public ConciliacionBancaria ConciliacionBancaria { get; set; }
        public List<DetalleTransaccion> DetallesTransacciones { get; set; }
    }
}
