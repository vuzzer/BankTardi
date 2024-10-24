namespace BanqueTardi.Contract
{
    public interface ICompte
    {
        public int Identifiant { get; set; }
        public string Libelle { get; set; }
        public int TauxInteret { get; set; }
        public int TauxInteretDecouvert { get; set; }
    }
}
