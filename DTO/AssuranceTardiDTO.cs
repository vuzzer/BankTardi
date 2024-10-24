using System.ComponentModel;

namespace BanqueTardi.DTO
{
    public class AssuranceTardiDTO
    {
        [DisplayName("Numero contrat")]
        public string ID { get; set; }
        [DisplayName("Nom client")]
        public string NomClient { get; set; }
        [DisplayName("Prénom client")]
        public string PrenomClient { get; set; }
        public double Prime { get; set; }
        public double Montant { get; set; }
        public bool Statut { get; set; } 
    }
}
