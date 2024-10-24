using BanqueTardi.Contract;

namespace BanqueTardi.DTO
{
    public class CompteEpargneDTO : ICompte
    {
        public int Identifiant { get; set; } = 11;
        public string Libelle { get; set; } = "Épargne";
        public int TauxInteret { get; set; } = 2;
        public int TauxInteretDecouvert { get; set; } = 0;
    }
}
