using Dapr.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transfert.API;

namespace BanqueTardi.Controllers
{
    public class TransfertController : Controller
    {

        private readonly HttpClient _httpClient;
        public TransfertController() { 
            _httpClient = DaprClient.CreateInvokeHttpClient("transfert");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<object>> TransfertMoney(){  
            var response = await _httpClient.GetFromJsonAsync<object>("TransfertMoney");
            return response;
        }
    }
}
