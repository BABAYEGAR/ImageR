using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Image.Controllers
{
    public class PackageItemController : Controller
    {
        // GET: PackageItem
        public ActionResult Index()
        {
            return View();
        }

        // GET: PackageItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PackageItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PackageItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PackageItem/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PackageItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PackageItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PackageItem/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}