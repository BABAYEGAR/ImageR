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
    public class PhotographerCategoryController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public PhotographerCategoryController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        public ActionResult Index()
        {
            return View(_databaseConnection.PhotographerCategories.ToList());
        }

        // GET: ImageCategory/Details/5
        public ActionResult Details(int id)
        {
            return View(_databaseConnection.PhotographerCategories.Find(id));
        }

        // GET: ImageCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhotographerCategory photographerCategory,  IFormCollection collection)
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                photographerCategory.DateCreated = DateTime.Now;
                photographerCategory.DateLastModified = DateTime.Now;
                photographerCategory.CreatedBy = signedInUserId;
                photographerCategory.LastModifiedBy = signedInUserId;

                _databaseConnection.PhotographerCategories.Add(photographerCategory);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Photographer Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ImageCategory/Edit/5
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.PhotographerCategories.Find(id));
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhotographerCategory photographerCategory,IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                photographerCategory.DateLastModified = DateTime.Now;
                photographerCategory.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(photographerCategory).State = EntityState.Modified;;
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
        public ActionResult Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["CategoryId"]);
            var photographerCategory = _databaseConnection.PhotographerCategories.Find(id);

            _databaseConnection.PhotographerCategories.Remove(photographerCategory);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Image Category!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}