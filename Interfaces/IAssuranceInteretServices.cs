using Assurance.ApplicationCore.Entites;
using BanqueTardi.DTO;
using BanqueTardi.Models;

namespace BanqueTardi.Interfaces
{
    public interface IAssuranceInteretServices
    {

        Task<IEnumerable<InteretResponseDTO>> Ajouter(IEnumerable<InteretRequestDTO> interet);
       
    }
}
