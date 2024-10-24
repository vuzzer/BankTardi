using BanqueTardi.Contract;
using BanqueTardi.Models;

namespace BanqueTardi.TardiCompte
{
    public interface IBanque
    {
        string NumeroTransit { get; }
        string NumeroInstitution { get; }
        string GenererNumeroCompte(Comptetype typeCompte);
        ICompte InitializeCompte(Comptetype compteType);
    }
}
