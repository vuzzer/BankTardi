using BanqueTardi.ContexteDb;
using BanqueTardi.Contract;
using BanqueTardi.DTO;
using BanqueTardi.Models;

namespace BanqueTardi.TardiCompte
{
    public class CompteCanadien : IBanque
    {
        private static readonly string transit = "00234";
        private static readonly string institution = "001";

        public string NumeroTransit => transit;
        public string NumeroInstitution => institution;
        public BanqueTardiContexte _banqueTardiContexte;

        public CompteCanadien(BanqueTardiContexte contexte) {
            _banqueTardiContexte = contexte;
        }

        public string GenererNumeroCompte(Comptetype typeCompte)
        {
            // Determine le nombre de compte pour le type spécifier
            int compteCounters = _banqueTardiContexte.Compte.Where(c => c.TypeCompte == typeCompte).Count();
            var compte = InitializeCompte(typeCompte);

            // Incrémenter le compteur pour ce type de compte
            compteCounters++;

            // Construire le numéro de compte
            return $"{compte.Identifiant}{compteCounters.ToString("D5")}";
        }

        public ICompte InitializeCompte(Comptetype compteType)
        {
            switch (compteType)
            {
                case Comptetype.Cheque:
                    return new CompteChequeDTO();
                case Comptetype.Epargne:
                    return new CompteEpargneDTO();
                default:
                    return new ComptePlacementDTO();
            }
        }
    }
}
