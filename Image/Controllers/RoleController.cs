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
    public class RoleController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public RoleController(ImageDataContext databaseConnection)
        {

            _databaseConnection = databaseConnection;
        }
        // GET: Role
        public ActionResult Index()
        {
            return View(_databaseConnection.Roles.ToList());
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role,IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                role.DateCreated = DateTime.Now;
                role.DateLastModified = DateTime.Now;
                role.CreatedBy = signedInUserId;
                role.LastModifiedBy = signedInUserId;

                _databaseConnection.Roles.Add(role);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new role!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Edit/5
        public ActionResult Edit(long id)
        {
            var role = _databaseConnection.Roles.Find(id);
            return View(role);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                role.DateLastModified = DateTime.Now;
                role.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(role).State = EntityState.Modified; ;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Role!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                var id = Convert.ToInt64(collection["RoleId"]);
                var role = _databaseConnection.Roles.Find(id);

                _databaseConnection.Roles.Remove(role);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully deleted the Role!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}