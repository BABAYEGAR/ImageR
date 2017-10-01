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
    public class PackageItemController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public PackageItemController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: PackageItem
        public ActionResult Index(long packageId)
        {
            ViewBag.PackageId = packageId;
            return View(_databaseConnection.PackageItem.Where(n=>n.PackageId == packageId).ToList());
        }

        // GET: PackageItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PackageItem/Create
        public ActionResult Create(long id)
        {
            ViewBag.packageId = id;
            return View();
        }

        // POST: PackageItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackageItem packageItem,IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                packageItem.DateCreated = DateTime.Now;
                packageItem.DateLastModified = DateTime.Now;
                packageItem.CreatedBy = signedInUserId;
                packageItem.LastModifiedBy = signedInUserId;

                _databaseConnection.PackageItem.Add(packageItem);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Package Item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
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
        public ActionResult Edit(PackageItem packageItem, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                packageItem.DateLastModified = DateTime.Now;
                packageItem.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(packageItem).State = EntityState.Modified; ;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Package Item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PackageItem/Delete/5
        public ActionResult Delete(long id, IFormCollection collection)
        {
            try
            {
                var packageItem = _databaseConnection.PackageItem.Find(id);

                _databaseConnection.PackageItem.Remove(packageItem);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully deleted the Package Item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new{packageId = id});
            }
            catch
            {
                return View();
            }
        }
    }
}