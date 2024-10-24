using BanqueTardi.Models;

namespace BanqueTardi.DTO
{
    public class OperationBodyRequestDTO
    {
        public CreditDebit? TypeOperation {  get; set; }
        public string CompteID { get; set; }
    }
}
