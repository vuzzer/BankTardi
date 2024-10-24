using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assurance.ApplicationCore.Entites
{
    public class AssuranceTardi
    {
        public string? ID { get; set; }
        public string ClientID { get; set; }
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string CodePartenaire { get; set; }
        public CodeRabais CodeRabais { get; set; }
        public double Solde { get; set; }
        public Sexe Sexe { get; set; }
        public bool EstFumeur { get; set; }
        public bool EstDiabetique { get; set; }
        public bool EstHypertendu { get; set; }
        public bool PratiqueActivitePhysique { get; set; }
        public bool Statut { get; set; } = false;

    }
    public enum CodeRabais
    {
        PRI = 10,
        SRI = 5,
        TRI = 15
    }
    public enum Sexe
    {
        masculin, feminin
    }
}
