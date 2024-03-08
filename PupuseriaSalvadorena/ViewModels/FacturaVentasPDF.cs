using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class FacturaVentasPDF
    {
        public string Mes { get; set; }
        public string Year { get; set; }
        public int TotalFacturas { get; set; }
        public decimal SubtotalVentas { get; set; }
        public decimal TotalVentas { get; set; }
        public Negocio Negocio { get; set; }
        public List<FacturaVenta> Facturas { get; set; }
    }
}
