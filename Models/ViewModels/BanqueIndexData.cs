namespace BanqueTardi.Models.ViewModels
{
    public class BanqueIndexData
    {
        public Client Client {  get; set; }
        public IEnumerable<Compte> Comptes { get; set; }
        public Dictionary<Comptetype, List<Operation>> Operations { get; set; }
    }
}
