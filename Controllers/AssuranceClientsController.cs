using Assurance.ApplicationCore.Entites;
using BanqueTardi.ContexteDb;
using BanqueTardi.DTO;
using BanqueTardi.Interfaces;
using BanqueTardi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BanqueTardi.Controllers
{
    public class AssuranceClientsController : Controller
    {
        private readonly IAssuranceClientServices _assuranceClientServices;
        private readonly BanqueTardiContexte _context;
        public AssuranceClientsController(IAssuranceClientServices assuranceClientServices, BanqueTardiContexte contexte)
        {
            _assuranceClientServices = assuranceClientServices;
            _context = contexte;
        }
        // GET: AssuranceClientsController
        public async Task<ActionResult> Index()
        {
            return View(await _assuranceClientServices.ObtenirTout());
        }

        // GET: AssuranceClientsController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            return View(await _assuranceClientServices.Obtenir(id));
        }

        // GET: AssuranceClientsController/Create
        public ActionResult Create()
        {
            ListeDesClients();
            return View();
        }

        // POST: AssuranceClientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssuranceTardiBodyDTO collection)
        {
            
            AssuranceTardi assurance = new AssuranceTardi()
            {
                ClientID = collection.ClientID,
                NomClient = _context.Clients.First(cl=> cl.ClientID == collection.ClientID).NomClient,
                PrenomClient = _context.Clients.First(cl => cl.ClientID == collection.ClientID).PrenomClient,
                DateDeNaissance = _context.Clients.Where(c => c.ClientID == collection.ClientID).Single().DateNaissance,
                CodePartenaire = "TARDI1010",
                CodeRabais= CodeRabais.PRI,
                Solde = collection.Solde,
                Sexe= collection.Sexe,
                EstDiabetique = collection.EstDiabetique,
                EstFumeur = collection.EstFumeur,
                EstHypertendu = collection.EstHypertendu,
                PratiqueActivitePhysique = collection.PratiqueActivitePhysique,
            };

         
                if(ModelState.IsValid)
                {
                    await _assuranceClientServices.Ajouter(assurance);
                    return RedirectToAction(nameof(Index));
                }
                
           
                return View(collection);
            
        }
        [HttpGet]
        // GET: AssuranceClientsController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ListeDesClients();
            ViewBag.Id = id;
            var assuranceTardi = await _assuranceClientServices.Obtenir(id);
            AssuranceTardiBodyDTO assuranceTardiBodyDTO = new AssuranceTardiBodyDTO()
            { 
               ClientID =  assuranceTardi.ClientID,
               Solde = assuranceTardi.Solde,
               Sexe = assuranceTardi.Sexe,
               EstDiabetique = assuranceTardi.EstDiabetique,
               EstFumeur = assuranceTardi.EstFumeur,
               EstHypertendu = assuranceTardi.EstHypertendu
            };
            return View(assuranceTardiBodyDTO);
        }

        // POST: AssuranceClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AssuranceTardiBodyDTO collection)
        {
            
            AssuranceTardi assurance = new AssuranceTardi()
            {
                ID = id,
                ClientID = collection.ClientID,
                NomClient = _context.Clients.First(cl => cl.ClientID == collection.ClientID).NomClient,
                PrenomClient = _context.Clients.First(cl => cl.ClientID == collection.ClientID).PrenomClient,
                DateDeNaissance = _context.Clients.Where(c => c.ClientID == collection.ClientID).Single().DateNaissance,
                CodePartenaire = "TARDI1010",
                CodeRabais = CodeRabais.PRI,
                Solde = collection.Solde,
                Sexe = collection.Sexe,
                EstDiabetique = collection.EstDiabetique,
                EstFumeur = collection.EstFumeur,
                EstHypertendu = collection.EstHypertendu,
                PratiqueActivitePhysique = collection.PratiqueActivitePhysique,
               
            };
            if (ModelState.IsValid)
            {
                await _assuranceClientServices.Modifier(assurance);
                return RedirectToAction(nameof(Index));
            }
           
                return View(collection);
           
        }

        [HttpGet]
        // GET: AssuranceClientsController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
              await _assuranceClientServices.Supprimer(id);
            return RedirectToAction(nameof(Index));
        }

        
        
        [HttpGet]
        public async Task<ActionResult> Confirmer(string id)
        {
            await _assuranceClientServices.Confirmer(id);
            return RedirectToAction(nameof(Index));
        }

        private void ListeDesClients(object? selectedValue = null)
        {
            ViewData["Client"] = new SelectList((_context.Clients), "ClientID", "FullName", selectedValue);
        }


    }
}
