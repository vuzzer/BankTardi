using Dapr.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanqueTardi.Controllers
{
    public class TransfertController : Controller
    {

        private readonly HttpClient _httpClient;
        public TransfertController() { 
            _httpClient = DaprClient.CreateInvokeHttpClient("transfert");
        }

        public async Task<ActionResult<object>> SendMoney(){  
            var response = await _httpClient.GetFromJsonAsync<object>("TransfertMoney");
            return response;
        }
    }
}
