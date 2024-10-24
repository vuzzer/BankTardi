using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanqueTardi.Controllers
{
    public class AssuranceClientController : Controller
    {
        // GET: AssuranceClientController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AssuranceClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssuranceClientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssuranceClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssuranceClientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssuranceClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssuranceClientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AssuranceClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
