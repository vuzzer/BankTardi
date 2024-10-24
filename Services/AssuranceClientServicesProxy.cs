using Assurance.ApplicationCore.Entites;
using BanqueTardi.DTO;
using BanqueTardi.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace BanqueTardi.Services
{
    public class AssuranceClientServicesProxy : IAssuranceClientServices
    {
        private readonly HttpClient _httpClient;
        private const string _assuranceClientApiUrl = "api/Assurances/";
        public AssuranceClientServicesProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task Ajouter(AssuranceTardi Assurance)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(Assurance), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_assuranceClientApiUrl + "Creation", content);
        }

        public async Task Modifier(AssuranceTardi Assurance)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(Assurance), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_assuranceClientApiUrl + "Modifier", content);
        }

        public async Task<AssuranceTardi> Obtenir(string id)
        {
            return await _httpClient.GetFromJsonAsync<AssuranceTardi>(_assuranceClientApiUrl + id);
        }

        public async Task<List<AssuranceTardiDTO>> ObtenirTout()
        {
            return await _httpClient.GetFromJsonAsync<List<AssuranceTardiDTO>>(_assuranceClientApiUrl);
        }

        public async Task Supprimer(string id)
        {
            await _httpClient.DeleteAsync(_assuranceClientApiUrl + id); 
        }
        public async Task Confirmer(string id)
        {
            var payload = true;
            StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_assuranceClientApiUrl+ "Confirmer/" + id, content );
            Console.WriteLine(response);
        }
}
    }
