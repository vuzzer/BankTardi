using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanqueTardi.ContexteDb;
using BanqueTardi.Models;
using BanqueTardi.Models.ViewModels;
using BanqueTardi.Contract;
using BanqueTardi.DTO;
using BanqueTardi.TardiCompte;


namespace BanqueTardi.Controllers
{
    public class ComptesController : Controller
    {
        private readonly BanqueTardiContexte _context;
        private readonly IBanque tardi;

        public ComptesController(BanqueTardiContexte context, IBanque banque)
        {
            _context = context;
            tardi = banque;
        }

        // GET: Comptes
        public IActionResult Index(string? id)
        {
            try
            {
                BanqueIndexData viewModel = new BanqueIndexData();
                viewModel.Client = _context.Clients
                    .AsNoTracking()
                    .Include(c => c.Comptes)
                    !.ThenInclude(cpt => cpt.Operations)
                    .Where(c => c.ClientID == id).Single();

                viewModel.Comptes = viewModel.Client.Comptes!.ToList();

                viewModel.Operations = viewModel.Comptes.GroupBy(cpt => cpt.TypeCompte)
                    .ToDictionary(
                        grp => grp.Key, // Type de compte
                        grp => grp
                            .SelectMany(cpt => cpt.Operations!)
                            .OrderByDescending(op => op.DateTransaction)
                            .Take(10)
                            .ToList()
                    );

                return View(viewModel);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET: Comptes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compte = await _context.Compte
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.CompteID == id);
            if (compte == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // GET: Comptes/Create
        public IActionResult Create(string id)
        {
            if(ClientExists(id))
            {
                PopulateDropDownAccountType();
                ViewData["ClientID"] = id;
                return View();
            }
            return NotFound();
        }

        // POST: Comptes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientID,Solde,TypeCompte")] Compte compte)
        {

            if(DoesClientHaveAccountOfType(compte.ClientID, compte.TypeCompte))
            {
                ViewBag.Message = $"Le client possède déjà un compte {compte.TypeCompte}";
                ViewBag.ClientID = compte.ClientID;
                PopulateDropDownAccountType(compte.TypeCompte);
                return View(compte);
            }

            if (ModelState.IsValid)
            {
                ICompte configCompte = tardi.InitializeCompte(compte.TypeCompte);
                compte.Libelle = configCompte.Libelle;
                compte.TauxInteret = configCompte.TauxInteret;
                compte.Identifiant = configCompte.Identifiant;
                compte.TauxInteretDecouvert = configCompte.TauxInteretDecouvert;
                compte.NumeroCompte = tardi.GenererNumeroCompte(compte.TypeCompte);
                _context.Add(compte);

                await _context.SaveChangesAsync();

                // Sauvegarde des opération d'ouvertures, quand solde positives
                var newCompte = _context.Compte
                    .Where(c => c.NumeroCompte == compte.NumeroCompte).Single();

                if (compte.Solde > 0)
                {
                    Operation operation = new Operation
                    {
                        CompteID = newCompte.CompteID,
                        Montant = newCompte.Solde,
                        TypeOperation = CreditDebit.credit,
                        Libelle = "Solde d'ouverture",
                    };
                    _context.Add(operation);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = compte.ClientID });
            }

            ViewBag.ClientID = compte.ClientID;
            PopulateDropDownAccountType(compte.TypeCompte);
            return View(compte);
        }

        // GET: Comptes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var compte = await _context.Compte.Where(c => c.CompteID == id).FirstOrDefaultAsync();
            if (compte == null)
            {
                return NotFound();
            }
            PopulateDropDownAccountType();
            return View(compte);
        }

        // POST: Comptes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CompteID,Solde,TypeCompte")] Compte compteMaj)
        {

            var compte = _context.Compte.AsNoTracking().Include(c => c.Client)
                .Where(c => c.CompteID == compteMaj.CompteID).First();


            if (DoesClientHaveAccountOfType(compte.ClientID, compteMaj.TypeCompte))
            {
                ViewBag.Message = $"Le client possède déjà un compte {compteMaj.TypeCompte}";
                ViewBag.ClientID = compte.ClientID;
                PopulateDropDownAccountType(compte.TypeCompte);
                return View(compte);
            }


            if (DoesAccountHaveOperations(compte.CompteID))
            {
                ViewBag.Message = $"Impossible de modifier le compte, car il a déjà des opérations";
                ViewBag.ClientID = compte.ClientID;
                PopulateDropDownAccountType(compte.TypeCompte);
                return View(compte);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ICompte configCompte = tardi.InitializeCompte(compteMaj.TypeCompte);
                    compte.Libelle = configCompte.Libelle;
                    compte.TauxInteret = configCompte.TauxInteret;
                    compte.Identifiant = configCompte.Identifiant;
                    compte.TauxInteretDecouvert = configCompte.TauxInteretDecouvert;
                    compte.NumeroCompte = tardi.GenererNumeroCompte(compteMaj.TypeCompte);

                    if (compte.Solde > 0)
                    {
                        Operation operation = new Operation
                        {
                            CompteID = compte.CompteID,
                            Montant = compte.Solde,
                            TypeOperation = CreditDebit.credit,
                            Libelle = "Solde d'ouverture",
                        };
                        _context.Add(operation);
                    }

                    _context.Update(compte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompteExists(compte.CompteID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = compte.ClientID});
            }
            ViewBag.ClientID = compte.ClientID;
            PopulateDropDownAccountType();
            return View(compte);
        }

        // GET: Comptes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compte = await _context.Compte
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.CompteID == id);
            if (compte == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // POST: Comptes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var compte = await _context.Compte.FindAsync(id);
            if (compte != null)
            {
                _context.Compte.Remove(compte);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompteExists(string id)
        {
            return _context.Compte.Any(e => e.CompteID == id);
        }

        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.ClientID == id);
        }

        private void PopulateDropDownAccountType(object? selectedValue = null)
        {
            ViewData["TypeCompte"] = new SelectList(Enum.GetValues(typeof(Comptetype))
                                    .Cast<Comptetype>()
                                    .Select(tc => new SelectListItem
                                    { Value = tc.ToString(), Text = tc.ToString() }), "Value", "Text", selectedValue );
        }

        private bool DoesClientHaveAccountOfType(string clientId, Comptetype typeCompte)
        {
            return _context.Compte.Any(c => c.ClientID == clientId && c.TypeCompte == typeCompte);
        }

        private bool DoesAccountHaveOperations(string compteID)
        {
            return _context.Operations.Any(op => op.CompteID == compteID);
        }
    }
}
