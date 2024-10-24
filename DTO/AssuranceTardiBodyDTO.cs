using Assurance.ApplicationCore.Entites;

namespace BanqueTardi.DTO
{
    public class AssuranceTardiBodyDTO
    {
        public string ClientID { get; set; }
        public string? NomClient { get; set; }
        public string? PrenomClient { get; set; }
        public double Solde { get; set; }
        public Sexe Sexe { get; set; }
        public bool EstFumeur { get; set; }
        public bool EstDiabetique { get; set; }
        public bool EstHypertendu { get; set; }
        public bool PratiqueActivitePhysique { get; set; }
        public bool Statut { get; set; }
    }
    
}
