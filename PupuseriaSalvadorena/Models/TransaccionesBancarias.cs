namespace PupuseriaSalvadorena.Models
{
    public class TransaccionesBancarias
    {
        public string Referencia { get; set; }

        public DateTime Fecha { get; set; }

        public string Concepto { get; set; }

        public decimal Debitos { get; set; }

        public decimal Creditos { get; set; }
    }
}
