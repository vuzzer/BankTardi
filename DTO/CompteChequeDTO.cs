using BanqueTardi.Contract;

namespace BanqueTardi.DTO
{
    public class CompteChequeDTO : ICompte
    {
        public int Identifiant { get; set; } = 10;
        public string Libelle { get; set; } = "Chèque";
        public int TauxInteret { get; set; } = 0;
        public int TauxInteretDecouvert { get; set; } = 7;
    }
}
