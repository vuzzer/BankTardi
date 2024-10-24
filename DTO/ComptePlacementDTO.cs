using BanqueTardi.Contract;

namespace BanqueTardi.DTO
{
    public class ComptePlacementDTO : ICompte
    {
        public int Identifiant { get; set; } = 16;
        public string Libelle { get; set; } = "Compte de placement garanti";
        public int TauxInteret { get; set; } = 4;
        public int TauxInteretDecouvert { get; set; } = 0;
    }
}
