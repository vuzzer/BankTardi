using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanqueTardi.ContexteDb;
using BanqueTardi.Models;
using BanqueTardi.DTO;

namespace BanqueTardi.Controllers
{
    public class OperationsController : Controller
    {
        private readonly BanqueTardiContexte _context;

        public OperationsController(BanqueTardiContexte context)
        {
            _context = context;
        }

        // GET: Operations
        public async Task<IActionResult> Index(string id)
        {

            var compte = await _context.Compte
                                        .AsNoTracking()
                                        .Include(c => c.Client)
                                        .FirstOrDefaultAsync(c => c.CompteID == id);

            if (compte == null)
            {
                return NotFound();
            }

            var banqueTardiContexte = _context.Operations.AsNoTracking()
                                            .Include(o => o.Compte)
                                            .Where(o => o.CompteID == id)
                                            .OrderByDescending(o => o.DateTransaction);
            ViewBag.CompteId = compte.NumeroCompte;
            ViewBag.ClientId = compte.ClientID;
            PopulateDropDownOperationType();
            return View(await banqueTardiContexte.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind(["TypeOperation", "CompteID"])] OperationBodyRequestDTO operationBody)
        {
            List<Operation>? operation;
            if (operationBody.TypeOperation != null)
            {
                operation = await _context.Operations.AsNoTracking()
                          .Where(o => o.TypeOperation == operationBody.TypeOperation && o.CompteID == operationBody.CompteID)
                          .OrderByDescending(o => o.DateTransaction).ToListAsync();
                ;
            }
            else
            {
                operation = await _context.Operations.AsNoTracking()
                    .Where(o => o.CompteID == operationBody.CompteID)
                .OrderByDescending(o => o.DateTransaction).ToListAsync(); ;
            }


            var compte = await _context.Compte
                            .AsNoTracking()
                            .Include(c => c.Client)
                            .FirstOrDefaultAsync(c => c.CompteID == operationBody.CompteID);

            ViewBag.CompteId = operationBody.CompteID;
            ViewBag.ClientId = compte!.ClientID;
            PopulateDropDownOperationType(operationBody.TypeOperation);

            return View(operation);
        }

        // GET: Operations/Create
        public IActionResult Create(string id)
        {
            if (CompteExists(id))
            {
                var compte = _context.Compte
                .AsNoTracking()
                .Include(c => c.Client)
                .First(c => c.CompteID == id);

                ViewBag.ClientID = compte!.ClientID;
                ViewBag.CompteID = compte!.CompteID;
                ViewBag.NumeroCompte = compte!.NumeroCompte;
                PopulateDropDownOperationType();
                return View();
            }
            return NotFound();
        }

        // POST: Operations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompteID,Libelle,Montant,TypeOperation")] Operation operation)
        {
            // Récupérer le compte associé à l'opération
            var compteMAJ = await _context.Compte.Include(c => c.Client)
                .Where(op => op.CompteID == operation.CompteID).SingleOrDefaultAsync();

            if (compteMAJ == null)
            {
                return NotFound();
            }

            // Mettre à jour le solde du compte
            switch (operation.TypeOperation)
            {
                case CreditDebit.credit:
                    compteMAJ!.Solde += operation.Montant;
                    break;
                case CreditDebit.debit:
                    compteMAJ!.Solde -= operation.Montant;
                    break;
                default:
                    break;
            }

            if (compteMAJ.Solde < 0 && compteMAJ.TypeCompte == Comptetype.Cheque)
            {
                // Ajouter une opération de débit pour le découvert
                var decouvertOperation = new Operation
                {
                    CompteID = compteMAJ.CompteID,
                    Libelle = "Découvert",
                    Montant = 10, // Montant du découvert
                    TypeOperation = CreditDebit.debit
                };

                // Mettre à jour le solde du compte après l'ajout du découvert
                compteMAJ.Solde -= decouvertOperation.Montant;

                // Incrémenter le nombre de découverts pour le client
                compteMAJ.Client!.NbDecouverts++;

                // Ajouter l'opération de découvert au contexte
                _context.Add(decouvertOperation);
            }
            else if(compteMAJ.Solde < 0 && compteMAJ.TypeCompte != Comptetype.Cheque)
            {
                ModelState.AddModelError("Echec", "Solde insuffisant");
            }

            if (ModelState.IsValid)
            {

                _context.Add(operation);
                _context.Update(compteMAJ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = operation.CompteID} );
            }

            var compte = _context.Compte
                                .AsNoTracking()
                                .Include(c => c.Client)
                                .First(c => c.CompteID == operation.CompteID);

            ViewBag.ClientID = compte!.ClientID;
            ViewData["CompteID"] = compte.CompteID;
            PopulateDropDownOperationType(operation.TypeOperation);

            return View(operation);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }
            ViewData["CompteID"] = new SelectList(_context.Compte, "CompteID", "CompteID", operation.CompteID);
            return View(operation);
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OperationID,CompteID,Libelle,Montant,DateTransaction,TypeOperation")] Operation operation)
        {
            if (id != operation.OperationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationExists(operation.OperationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompteID"] = new SelectList(_context.Compte, "CompteID", "CompteID", operation.CompteID);
            return View(operation);
        }

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Compte)
                .FirstOrDefaultAsync(m => m.OperationID == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation != null)
            {
                _context.Operations.Remove(operation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationExists(int id)
        {
            return _context.Operations.Any(e => e.OperationID == id);
        }

        private bool CompteExists(string id)
        {
            return _context.Compte.Any(e => e.CompteID == id);
        }

        private void PopulateDropDownOperationType(object? selectedValue = null)
        {
            ViewData["TypeOperation"] = new SelectList(Enum.GetValues(typeof(CreditDebit))
                                    .Cast<CreditDebit>()
                                    .Select(tc => new SelectListItem
                                    { Value = tc.ToString(), Text = tc.ToString() }), "Value", "Text", selectedValue);
        }

    }
}
