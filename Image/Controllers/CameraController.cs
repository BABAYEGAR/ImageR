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
    public class CameraController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public CameraController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        public ActionResult Index()
        {
            return View(_databaseConnection.Cameras.ToList());
        }

        // GET: ImageCategory/Details/5
        public ActionResult Details(int id)
        {
            return View(_databaseConnection.Cameras.Find(id));
        }

        // GET: ImageCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Camera camera,  IFormCollection collection)
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                camera.DateCreated = DateTime.Now;
                camera.DateLastModified = DateTime.Now;
                camera.CreatedBy = signedInUserId;
                camera.LastModifiedBy = signedInUserId;

                _databaseConnection.Cameras.Add(camera);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Camera!";
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
        public ActionResult Edit(Camera camera,IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                camera.DateLastModified = DateTime.Now;
                camera.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(camera).State = EntityState.Modified;;
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
        public ActionResult Delete(int id)
        {
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