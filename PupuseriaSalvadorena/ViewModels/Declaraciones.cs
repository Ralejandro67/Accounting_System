using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class Declaraciones
    {
        public Negocio Negocio { get; set; }
        public DeclaracionImpuesto DeclaracionImpuesto { get; set; }
        public List<DetalleTransaccion> DetallesTransacciones { get; set; }
    }
}
