using Assurance.ApplicationCore.Entites;
using BanqueTardi.DTO;

namespace BanqueTardi.Interfaces
{
    public interface IAssuranceClientServices
    {
        Task<List<AssuranceTardiDTO>> ObtenirTout();
        Task<AssuranceTardi> Obtenir(string id);
        Task Ajouter(AssuranceTardi Assurance);
        Task Supprimer(string id);
        Task Modifier(AssuranceTardi Assurance);
        Task Confirmer(string id);
    }
}
