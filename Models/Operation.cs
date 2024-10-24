using BanqueTardi.CustomValidators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanqueTardi.Models
{
    public class Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OperationID { get; set; }
        public string CompteID { get; set; }
        public string Libelle { get; set; }
        [CheckAmountValidation(ErrorMessage = "Le montant doit être positive")]
        public double Montant { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTransaction { get; set; } = DateTime.Now;
        public CreditDebit TypeOperation { get; set; }
        public virtual Compte? Compte { get; set; } 
    }

    public enum CreditDebit { credit, debit }
}
