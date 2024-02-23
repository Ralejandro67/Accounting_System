using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class PresupuestoPDF
    {
        public decimal SaldoUsado { get; set; }
        public decimal SaldoRestante { get; set; }
        public int TotalTransacciones { get; set; }
        public Negocio Negocio { get; set; }
        public Presupuesto Presupuesto { get; set; }
        public List<DetallePresupuesto> DetallesP { get; set; }
    }
}
