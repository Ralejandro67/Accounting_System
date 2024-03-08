using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class LibroContablePDF
    {
        public int totalTransacciones { get; set; }
        public int totalIngresos { get; set; }
        public int totalEgresos { get; set; }
        public decimal valorIngresos { get; set; }
        public decimal valorEgresos { get; set; }
        public Negocio Negocio { get; set; }
        public RegistroLibro RegistroLibro { get; set; }
        public List<DetalleTransaccion> DetallesTransacciones { get; set; }
    }
}
