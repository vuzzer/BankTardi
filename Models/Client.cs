using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BanqueTardi.Models
{
    public class Client
    {
        [DisplayName("Code")]
        public string ClientID { get; set; }

        [MaxLength(150)]
        [DisplayName("Nom")]
        public string NomClient { get; set; }

        [MaxLength(150)]
        [DisplayName("Prénom")]
        public string PrenomClient { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date de naissance")]
        public DateTime DateNaissance { get; set; }
        [MaxLength(250)]
        public string Adresse { get; set; }
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Le code postal est invalide.")]
        [DisplayName("Code postal")]
        public string CodePostal { get; set; }
        public int NbDecouverts { get; set; }
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Le Numéro de téléphone n'est pas valide.")]
        public string Telephone { get; set; }
        [DisplayName("Nom parent")]
        public string? NomParent { get; set; }
        [DisplayName("Telephone parent")]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage ="Le Numéro de téléphone n'est pas valide.")]
        public string? TelephoneParent { get; set; }
        public string FullName 
        {
            get
            {
                return NomClient + "," + PrenomClient;
            }  
        }


        public virtual ICollection<Compte>? Comptes { get; set; }

    }
}
