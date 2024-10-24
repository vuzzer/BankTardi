using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanqueTardi.ContexteDb;
using BanqueTardi.Models;

namespace BanqueTardi.Controllers
{
    public class ClientsController : Controller
    {
        private readonly BanqueTardiContexte _context;

        public ClientsController(BanqueTardiContexte context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public async Task<IActionResult> Create()
        {
            List<Client> clients = await _context.Clients.ToListAsync();
            Client client = new Client();
            if (clients.Count > 0)
            {

                // Recupere le client ayant le plus grand code, en effectuant un tri desc par matricule
                string dernierMatricule = clients.OrderByDescending(cli => cli.ClientID).FirstOrDefault()!.ClientID;

                // Convertir le code à 8 chiffre du code du client ayant le plus grand matricule
                int dernierCode = int.Parse(dernierMatricule.Substring(3));

                // Generation de l'identifiant du client
                client.ClientID = Utils.Utils.GenererIdentifiantClient(dernierCode);
            }
            else
            {
                // Generation de l'identifiant du client
                client.ClientID = Utils.Utils.GenererIdentifiantClient(clients.Count);

            }

            return View(client);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientID,NomClient,PrenomClient,DateNaissance,Adresse, CodePostal, Telephone, NomParent, TelephoneParent")] Client client)
        {
            if(client.DateNaissance.Year>= 2010)
            {
                ViewBag.Message = "Impossible d'inscrire un client qui a moins de 15 ans";
                return View(client);
            }

            if (DateTime.Now.Year - client.DateNaissance.Year < 18 &&(client.NomParent == null || client.TelephoneParent == null))
            {
                    ViewBag.MineurMessage = "Précisez le nom d'un de vos parents ainsi que son numero de téléphone";
                    ModelState.AddModelError(nameof(client.NomParent), "Vous devez préciser le nom d'un de vos parents");
                    return View(client); 
            }
            else if (ModelState.IsValid)
            {
               
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

            }
            //return View(client);
            return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            if (DateTime.Now.Year - client.DateNaissance.Year < 18)
            {
                ViewBag.MineurMessage = "Mineur";
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ClientID,NomClient,PrenomClient,DateNaissance,Adresse,CodePostal,NbDecouverts,Telephone,NomParent,TelephoneParent")] Client client)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Comptes", new { id = client.ClientID});
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Client client)
        {
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Recherche(string champRecherche)
        {
            List<Client> clients = await _context.Clients.ToListAsync();
            List<Client> listefiltrer = clients.Where(x => (string.IsNullOrEmpty(champRecherche)) ||
            x.NomClient.ToLower().Contains(champRecherche.ToLower())).ToList();
            return View("Index", listefiltrer);
        }

        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.ClientID == id);
        }
    }
}
