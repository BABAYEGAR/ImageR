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
    public class CameraController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        AppUser _appUser;
        List<Image> _images = new List<Image>();

        public CameraController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        [SessionExpireFilter]
        public ActionResult Index()
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            return View(_databaseConnection.Cameras.Where(n=>n.CreatedBy == signedInUserId).ToList());
        }
        // GET: Image
        [SessionExpireFilter]
        public ActionResult Images(long id)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            if (HttpContext.Session.GetString("StudioLoggedInUserId") != null)
            {
                var userString = HttpContext.Session.GetString("StudioLoggedInUserId");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            }
            if (_appUser.Role.ManageImages)
            {
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                        .Where(n => n.AppUserId == signedInUserId && n.CameraId == id).ToList();
                

            }
            if (_appUser.Role.UploadImage)
            {

                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(n =>n.CameraId == id).ToList();
                

            }
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
        public ActionResult Create(Camera camera,  IFormCollection collection)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                camera.DateCreated = DateTime.Now;
                camera.DateLastModified = DateTime.Now;
                camera.CreatedBy = signedInUserId;
                camera.LastModifiedBy = signedInUserId;

                _databaseConnection.Cameras.Add(camera);
                _databaseConnection.SaveChanges();

            
                if (collection != null && collection["Image"] != "" && collection["Image"] == "Redirect")
                {
                    ViewBag.CameraId = new SelectList(
                        _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                        "Name");
                    var cameras = new Camera();
                    return PartialView("Partials/_PartialImageCamera");
                }
                //display notification
                TempData["display"] = "You have successfully added a new Camera!";
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
            return View(_databaseConnection.Cameras.Find(id));
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(Camera camera,IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                camera.DateLastModified = DateTime.Now;
                camera.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(camera).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Camera!";
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
            var id = Convert.ToInt64(collection["CameraId"]);
            var camera = _databaseConnection.Cameras.Find(id);

            _databaseConnection.Cameras.Remove(camera);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Camera!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}