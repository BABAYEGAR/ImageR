using System;
using System.Collections.Generic;
using System.Linq;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class ImageSubCategoryController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        AppUser _appUser;
        List<Image> _images = new List<Image>();
        public ImageSubCategoryController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageSubCategory
        [SessionExpireFilter]
        public ActionResult Index(long id)
        {
            ViewBag.CategoryId = id;
            return View(_databaseConnection.ImageSubCategories.Where(n=>n.ImageCategoryId == id).ToList());
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
                    .Where(n => n.AppUserId == signedInUserId && n.ImageSubCategoryId == id).ToList();


            }
            if (_appUser.Role.UploadImage)
            {

                _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(n => n.ImageSubCategoryId == id).ToList();


            }
            ViewBag.Role = _appUser;
            return View(_images);
        }

        // GET: ImageSubCategory/Create
        [SessionExpireFilter]
        public ActionResult Create(long id)
        {
            ViewBag.CategoryId = id;
            return View();
        }

        // POST: ImageSubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Create(ImageSubCategory imageSubCategory,IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                imageSubCategory.ImageCategoryId = Convert.ToInt64(collection["CategoryId"]);
                imageSubCategory.DateCreated = DateTime.Now;
                imageSubCategory.DateLastModified = DateTime.Now;
                imageSubCategory.CreatedBy = signedInUserId;
                imageSubCategory.LastModifiedBy = signedInUserId;

                _databaseConnection.ImageSubCategories.Add(imageSubCategory);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Image Sub-Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new{id = imageSubCategory.ImageCategoryId});
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageSubCategory/Edit/5
        [SessionExpireFilter]
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.ImageSubCategories.Find(id));
        }

        // POST: ImageSubCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(ImageSubCategory imageSubCategory)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                imageSubCategory.DateLastModified = DateTime.Now;
                imageSubCategory.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(imageSubCategory).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Image Sub-Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { id = imageSubCategory.ImageCategoryId });
            }
            catch
            {
                return View();
            }
        }
        [SessionExpireFilter]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                var id = Convert.ToInt64(collection["CategoryId"]);
                var imageSubCategory = _databaseConnection.ImageSubCategories.Find(id);
                long? categoryId = imageSubCategory.ImageCategoryId;
                _databaseConnection.ImageSubCategories.Remove(imageSubCategory);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully deleted the Image Sub-Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { id = categoryId });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}