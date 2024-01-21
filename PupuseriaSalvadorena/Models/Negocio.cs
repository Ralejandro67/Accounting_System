﻿using System.ComponentModel.DataAnnotations;

namespace PupuseriaSalvadorena.Models
{
    public class Negocio
    {
        [Key]   
        public long CedulaJuridica { get; set; }

        [Required(ErrorMessage = "El nombre del negocio es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del negocio solo puede contener letras")]
        public string? NombreEmpresa { get; set; }

        [Required(ErrorMessage = "La dirección del negocio es obligatoria")]
        public int IdDireccion { get; set; }

        [Required(ErrorMessage = "El teléfono del negocio es obligatorio")]
        public int IdTelefono { get; set; }

        public string? Detalles { get; set; }

        public int Telefono { get; set; }
    }
}