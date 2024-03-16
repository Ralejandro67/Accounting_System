using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class DetalleFactura
    {
        public Negocio Negocio { get; set; }
        public FacturaVenta FacturaVenta { get; set; }
        public EnvioFactura EnvioFactura { get; set; }
        public List<HistorialVenta> DetallesF { get; set; }
    }
}
