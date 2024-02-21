using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class DetallesPronosticos
    {
        public Pronostico Pronostico { get; set; }
        public List<DetallesPronostico> DetallesP { get; set; }
    }
}
