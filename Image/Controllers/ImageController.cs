using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Image.Models.DataBaseConnections;
using Image.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;

namespace Image.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public ImageController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: Image
        public ActionResult Index()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            return View(_databaseConnection.Images.Include(n=>n.Camera).Include(n=>n.Location).Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(n=>n.AppUserId ==signedInUserId ).ToList());
        }
        /// <summary>
        ///     Sends Json responds object to view with sub categories of the categories requested via an Ajax call
        /// </summary>
        /// <param name="id"> state id</param>
        /// <returns>lgas</returns>
        /// Microsoft.CodeDom.Providers.DotNetCompilerPlatform
        public JsonResult GetSubForCategories(long id)
        {
            var subs = _databaseConnection.ImageSubCategories.Where(n => n.ImageCategoryId == id).ToList();
            return Json(subs);
        }
        // GET: Image/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Image/Create
        public ActionResult Create()
        {
            ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(), "ImageCategoryId",
                "Name");
            ViewBag.CameraId = new SelectList(_databaseConnection.Cameras.ToList(), "CameraId",
                "Name");
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations.ToList(), "LocationId",
                "Name");
            return View();
        }

        // POST: Image/Create
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public ActionResult Create(Models.Entities.Image image, IFormCollection collection, IFormFile file)
        {
            try
            {
                //var fs = System.IO.File.Create(file.OpenReadStream().BeginRead().)

                Account account = new Account(
                    "cloudmab",
                    "988581656515289",
                    "Odh29Eet7Ajilw0O0kCflwtnj9E");

                Cloudinary cloudinary = new Cloudinary(account);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(DateTime.Now.ToFileTime().ToString(),file.OpenReadStream())
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                image.AppUserId = signedInUserId;
                image.DateCreated = DateTime.Now;
                image.DateLastModified = DateTime.Now;
                image.CreatedBy = signedInUserId;
                image.LastModifiedBy = signedInUserId;
                image.FileName = uploadResult.Uri.AbsolutePath;

                _databaseConnection.Images.Add(image);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully uploaded a new image!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Image/Edit/5
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.Images.Find(id));
        }

        // POST: Image/Edit/5
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
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

        // GET: Image/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Image/Delete/5
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
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