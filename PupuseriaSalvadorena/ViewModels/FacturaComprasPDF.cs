using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class FacturaComprasPDF
    {
        public string Mes { get; set; }
        public string Year { get; set; }
        public int TotalFacturas { get; set; }
        public decimal TotalVentas { get; set; }
        public Negocio Negocio { get; set; }
        public List<FacturaCompra> Facturas { get; set; }
    }
}
