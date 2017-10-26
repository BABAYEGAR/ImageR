using System;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Entities;
using Image.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Image.Controllers
{
    public class PackageController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public PackageController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: Package
        public ActionResult Index()
        {
            return View(_databaseConnection.Packages.Include(n=>n.PackageItems).ToList());
        }

        // GET: Package/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Package/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Package/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Package package,IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                package.DateCreated = DateTime.Now;
                package.DateLastModified = DateTime.Now;
                package.CreatedBy = signedInUserId;
                package.LastModifiedBy = signedInUserId;

                _databaseConnection.Packages.Add(package);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Package/Edit/5
        public ActionResult Edit(long id)
        {
            var pacakage = _databaseConnection.Packages.Find(id);
            return View(pacakage);
        }

        // POST: Package/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Package package, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                package.DateLastModified = DateTime.Now;
                package.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(package).State = EntityState.Modified; ;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                var id = Convert.ToInt64(collection["PackageId"]);
                var package = _databaseConnection.Packages.Find(id);

                _databaseConnection.Packages.Remove(package);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully deleted the Package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}