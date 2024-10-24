namespace BanqueTardi.Models
{
    public class Interet
    {
        public string ClientID { get; set; }
        public int CompteType { get; set; }
        public double Montant { get; set; }
        public DateTime DateDebutCalcul { get; set; }
        public DateTime DateFin {  get; set; }
        public int TauxInteret { get; set; }
    }
}
