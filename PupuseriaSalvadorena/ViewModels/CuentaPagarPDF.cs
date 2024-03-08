using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class CuentaPagarPDF
    {
        public decimal Pagado { get; set; }
        public decimal PorPagar { get; set; }
        public Negocio Negocio { get; set; }
        public CuentaPagar CuentaPagar { get; set; }
        public List<DetalleCuenta> DetallesC { get; set; }
    }
}