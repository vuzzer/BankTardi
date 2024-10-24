using BanqueTardi.ContexteDb;
using BanqueTardi.DTO;
using BanqueTardi.Models;
using BanqueTardi.TardiCompte;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BanqueTardi.Data
{
    public class DbInitializer
    {
        public static async void Initialize(BanqueTardiContexte context, IBanque tardi)
        {

            if (context.Clients.Any())
            {
                return;   // DB has been seeded
            }

            var client = new Client[]
            {
                new Client {ClientID="CL00000001",  NomClient = "Carson",   PrenomClient = "Alexander",
                    DateNaissance = DateTime.Parse("2009-09-01"), Adresse= "2033 rue DesMonts",
                    CodePostal="K2L 8G8", Telephone = "438 989-9676", NomParent="Carson Patrice",
                    TelephoneParent = "393 673-6456" },
                new Client {ClientID="CL00000002", NomClient = "Meredith", PrenomClient = "Alonso",
                    DateNaissance = DateTime.Parse("2008-09-01"), Adresse= "1234 Avenue Rollex",
                    CodePostal="G1J 4N4", Telephone = "567 389-1278", NomParent="Meredith Alain",
                    TelephoneParent = "453 892-1076" },
                new Client {ClientID="CL00000003", NomClient = "Arturo",   PrenomClient = "Anand",
                    DateNaissance = DateTime.Parse("1999-09-01"), Adresse= "3312 Rue DesCocotiers",
                    CodePostal="H1H 9F9", Telephone = "546 896-7890"},
                new Client {ClientID="CL00000004", NomClient = "Gytis",    PrenomClient = "Barzdukas",
                    DateNaissance = DateTime.Parse("1990-09-01"), Adresse= "2034 Avenue DuRoi",
                    CodePostal="T2G 6N6", Telephone = "438 932-6292"},
            };

            foreach (Client c in client)
            {
                context.Clients.Add(c);
            }
            context.SaveChanges();

            CompteChequeDTO compteChequeDTO = new CompteChequeDTO();
            CompteEpargneDTO compteEpargneDTO = new CompteEpargneDTO();
            ComptePlacementDTO comptePlacementDTO = new ComptePlacementDTO();
            var compte = new Compte[]
            {
                // 10-00001
                new Compte
                {       
                        TypeCompte = Comptetype.Cheque,
                        NumeroCompte= tardi.GenererNumeroCompte(Comptetype.Cheque),
                        Identifiant = compteChequeDTO.Identifiant,
                        Libelle = compteChequeDTO.Libelle,
                        TauxInteret = compteChequeDTO.TauxInteret,
                        TauxInteretDecouvert = compteChequeDTO.TauxInteretDecouvert,
                        Solde = 0,
                        ClientID  = client.Single( c => c.NomClient == "Carson").ClientID
                },
                // 11-00001
                 new Compte
                 {
                        TypeCompte = Comptetype.Epargne,
                        NumeroCompte= tardi.GenererNumeroCompte(Comptetype.Epargne),
                        Identifiant = compteEpargneDTO.Identifiant,
                        Libelle = compteEpargneDTO.Libelle,
                        TauxInteret = compteEpargneDTO.TauxInteret,
                        TauxInteretDecouvert = compteEpargneDTO.TauxInteretDecouvert,
                        Solde = 10000,
                        ClientID  = client.Single( c => c.NomClient == "Arturo").ClientID
                 },
                  // 16-00001
                  new Compte
                  {
                        TypeCompte = Comptetype.Placement,
                        NumeroCompte= tardi.GenererNumeroCompte(Comptetype.Placement),
                        Identifiant = comptePlacementDTO.Identifiant,
                        Libelle = comptePlacementDTO.Libelle,
                        TauxInteret = comptePlacementDTO.TauxInteret,
                        TauxInteretDecouvert = comptePlacementDTO.TauxInteretDecouvert,
                        Solde = 350000,
                        ClientID  = client.Single( c => c.NomClient == "Meredith").ClientID
                  },
            };

            foreach (Compte co in compte)
            {
                context.Compte.Add(co);
            }
            context.SaveChanges();


            var operations = new Operation[]
            {
                new Operation {
                    CompteID = compte.Single(c=> c.NumeroCompte == "1100001").CompteID,
                    TypeOperation= CreditDebit.credit,
                    Montant = compte.Single(c=> c.NumeroCompte == "1100001").Solde,
                    Libelle="Solde d'ouverture",
                    },
                new Operation {
                    TypeOperation= CreditDebit.credit,
                    CompteID= compte.Single(c=> c.NumeroCompte == "1000001").CompteID,
                    Montant = 500,
                    Libelle="Dépot 500$",
                    DateTransaction= DateTime.Parse("2024-08-18"),
                    },
                 new Operation {
                    CompteID = compte.Single(c=> c.NumeroCompte == "1600001").CompteID,
                    TypeOperation= CreditDebit.credit,
                    Montant = compte.Single(c=> c.NumeroCompte == "1600001").Solde,
                    Libelle="Solde d'ouverture",
                    DateTransaction= DateTime.Parse("2024-02-11"),
                    },
                new Operation {
                    TypeOperation= CreditDebit.debit,
                    CompteID= compte.Single(c=> c.NumeroCompte == "1600001").CompteID,
                    Montant = 500,
                    Libelle="Achat 500$",
                    DateTransaction= DateTime.Parse("2024-08-18"),
                    },

            };

            foreach (Operation o in operations)
            {
                context.Operations.Add(o);
            }
            context.SaveChanges();
                

            // Mise à jour des comptes
            var compteCarson = context.Compte.Include(c => c.Client).Where(c => c.NumeroCompte == "1000001").Single();
            compteCarson.Solde += 500;
            
            var compteMeredith = context.Compte.Include(c => c.Client).Where(c => c.NumeroCompte == "1600001").Single();
            compteMeredith.Solde -= 500;

            context.Update(compteCarson);
            context.Update(compteMeredith);
            context.SaveChanges();
        }
    }
}
