﻿using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Pronostico
    {
        [Key]
        public int IdPronostico { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un platillo para el pronostico.")]
        public int? IdPlatillo { get; set; }

        public string? NombrePlatillo { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public int CantTotalProd { get; set; }

        public decimal TotalVentas { get; set; }
    }
}
