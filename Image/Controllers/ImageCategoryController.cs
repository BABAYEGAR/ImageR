using System;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Image.Controllers
{
    public class ImageCategoryController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public ImageCategoryController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        public ActionResult Index()
        {
            return View(_databaseConnection.ImageCategories.ToList());
        }

        // GET: ImageCategory/Details/5
        public ActionResult Details(int id)
        {
            return View(_databaseConnection.ImageCategories.Find(id));
        }

        // GET: ImageCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImageCategory imageCategory,  IFormCollection collection)
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                imageCategory.DateCreated = DateTime.Now;
                imageCategory.DateLastModified = DateTime.Now;
                imageCategory.CreatedBy = signedInUserId;
                imageCategory.LastModifiedBy = signedInUserId;

                _databaseConnection.ImageCategories.Add(imageCategory);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Image Category!";
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
        public ActionResult Edit(ImageCategory imageCategory,IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                imageCategory.DateLastModified = DateTime.Now;
                imageCategory.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(imageCategory).State = EntityState.Modified;;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Image Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageCategory/Delete/5
        public ActionResult Delete(int id)
        {
            var imageCategory = _databaseConnection.ImageCategories.Find(id);

            _databaseConnection.ImageCategories.Remove(imageCategory);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Image Category!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}