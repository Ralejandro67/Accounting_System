using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class PronosticoPDF
    {
        public int TotalPronosticos { get; set; }
        public Negocio Negocio { get; set; }
        public Pronostico Pronostico { get; set; }
        public List<DetallesPronostico> DetallesP { get; set; }

    }
}
