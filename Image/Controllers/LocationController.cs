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
    public class LocationController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public LocationController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        public ActionResult Index()
        {
            return View(_databaseConnection.Locations.ToList());
        }

        // GET: ImageCategory/Details/5
        public ActionResult Details(int id)
        {
            return View(_databaseConnection.Locations.Find(id));
        }

        // GET: ImageCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location,  IFormCollection collection)
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                location.DateCreated = DateTime.Now;
                location.DateLastModified = DateTime.Now;
                location.CreatedBy = signedInUserId;
                location.LastModifiedBy = signedInUserId;

                _databaseConnection.Locations.Add(location);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Location!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ImageCategory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location,IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                location.DateLastModified = DateTime.Now;
                location.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(location).State = EntityState.Modified;;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Location!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageCategory/Delete/5
        public ActionResult Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["LocationId"]);
            var location = _databaseConnection.Locations.Find(id);

            _databaseConnection.Locations.Remove(location);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Location!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}