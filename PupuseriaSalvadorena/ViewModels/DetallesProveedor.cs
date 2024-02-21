using PupuseriaSalvadorena.Models;
using System.Collections.Generic;

namespace PupuseriaSalvadorena.ViewModels
{
    public class DetallesProveedor
    {
        public Proveedor Proveedor { get; set; }
        public List<MateriaPrima> MateriasPrimas { get; set; }
        public List<CuentaPagar> CuentasPagar { get; set; }
    }
}
