using System;
using System.Collections.Generic;
using System.Linq;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class LocationController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        AppUser _appUser;
        List<Image> _images = new List<Image>();

        public LocationController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        [SessionExpireFilter]
        public ActionResult Index()
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            return View(_databaseConnection.Locations.Where(n=>n.CreatedBy == signedInUserId).ToList());
        }
        // GET: Image
        [SessionExpireFilter]
        public ActionResult Images(long id)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            if (HttpContext.Session.GetString("StudioLoggedInUser") != null)
            {
                var userString = HttpContext.Session.GetString("StudioLoggedInUser");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            }
            if (_appUser.Role.ManageImages)
            {
                _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                    .Where(n => n.AppUserId == signedInUserId && n.LocationId == id).ToList();


            }
            if (_appUser.Role.UploadImage)
            {

                _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(n => n.LocationId == id).ToList();


            }
            ViewBag.Role = _appUser.Role;
            return View(_images);
        }
        // GET: ImageCategory/Create
        [SessionExpireFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Create(Location location,  IFormCollection collection)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                location.DateCreated = DateTime.Now;
                location.DateLastModified = DateTime.Now;
                location.CreatedBy = signedInUserId;
                location.LastModifiedBy = signedInUserId;

                _databaseConnection.Locations.Add(location);
                _databaseConnection.SaveChanges();

          
                if (collection != null && collection["Image"] != "" && collection["Image"] == "Redirect")
                {
                    ViewBag.LocationId = new SelectList(
                        _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                        "Name");
                    return PartialView("Partials/_PartialImageLocation");
                }
                //display notification
                TempData["display"] = "You have successfully added a new Location!";
                TempData["notificationtype"] = NotificationType.Success.ToString();

                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                return View();
            }
        }

        // GET: ImageCategory/Edit/5
        [SessionExpireFilter]
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.Locations.Find(id));
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(Location location,IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                location.DateLastModified = DateTime.Now;
                location.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(location).State = EntityState.Modified;
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
        [SessionExpireFilter]
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