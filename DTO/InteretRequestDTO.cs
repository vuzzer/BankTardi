using BanqueTardi.Models;

namespace BanqueTardi.DTO
{
    public class InteretRequestDTO
    {
        public string ClientID { get; set; }
        public Comptetype CompteType { get; set; }
        public double Montant { get; set; }
        public DateTime DateDebutCalcul { get; set; }
        public DateTime DateFin { get; set; }
        public int TauxInteret { get; set; }
    }
}
