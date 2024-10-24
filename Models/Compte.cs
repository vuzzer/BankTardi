using BanqueTardi.CustomValidators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BanqueTardi.Models
{

    public class Compte
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), ValidateNever]
        public string CompteID { get; set; }
        [ValidateNever]
        [Display(Name = "Numéro compte")]
        public string NumeroCompte { get; set; }
        [ValidateNever]
        public string ClientID { get; set; }
        [CheckAmountValidation]
        public double Solde { get; set; }
        [ValidateNever]
        public int Identifiant { get; set; }
        [ValidateNever]
        public string Libelle { get; set; }
        [ValidateNever]
        public int TauxInteret { get; set; }
        [ValidateNever]
        public int TauxInteretDecouvert { get; set; }
        public Comptetype TypeCompte { get; set; }

        public virtual ICollection<Operation>? Operations { get; set; }
        public virtual Client? Client { get; set; }
    }
    public enum Comptetype { Cheque, Epargne, Placement }
}
