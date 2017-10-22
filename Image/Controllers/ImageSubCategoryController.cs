using System;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Image.Controllers
{
    public class ImageSubCategoryController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public ImageSubCategoryController(ImageDataContext databaseConnection)
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

        // GET: ImageSubCategory/Details/5
        [SessionExpireFilter]
        public ActionResult Details(int id)
        {
            return View();
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
                var signedInUserId = HttpContext.Session.GetInt32("userId");
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
        public ActionResult Edit(ImageSubCategory imageSubCategory, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                imageSubCategory.DateLastModified = DateTime.Now;
                imageSubCategory.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(imageSubCategory).State = EntityState.Modified; ;
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
                return View();
            }
        }
    }
}