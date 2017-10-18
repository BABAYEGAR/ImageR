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
    public class CompetitionController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public CompetitionController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: Package
        public ActionResult Index()
        {
            return View(_databaseConnection.Competition.ToList());
        }

        // GET: Package/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Package/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Package/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competition competition,IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                competition.DateCreated = DateTime.Now;
                competition.DateLastModified = DateTime.Now;
                competition.CreatedBy = signedInUserId;
                competition.LastModifiedBy = signedInUserId;
                competition.Status = CompetitionStatus.Open.ToString();

                _databaseConnection.Competition.Add(competition);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Competition!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Package/Edit/5
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.Competition.Find(id));
        }

        // POST: Package/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Competition competition, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                competition.DateLastModified = DateTime.Now;
                competition.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(competition).State = EntityState.Modified; ;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Competition!";
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
                var id = Convert.ToInt64(collection["CompetitionId"]);
                var competition = _databaseConnection.Competition.Find(id);

                _databaseConnection.Competition.Remove(competition);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully deleted the Competition!";
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