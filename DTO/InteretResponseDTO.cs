using BanqueTardi.Models;

namespace BanqueTardi.DTO
{
    public class InteretResponseDTO
    {
        public string ClientID { get; set; }
        public Comptetype CompteType { get; set; }
        public double Interet { get; set; }
    }
}
