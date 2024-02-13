using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class DetallesPresupuestos
    {
        public Presupuesto Presupuesto { get; set; }
        public List<DetalleTransaccion> DetallesTransacciones { get; set; }
        public List<int> TransaccionesSeleccionadas { get; set; } = new List<int>();
    }
}
