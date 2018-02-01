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
    public class TermAndConditionController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;

        public TermAndConditionController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View(_databaseConnection.TermsAndConditions.ToList());
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
        public ActionResult Create(TermAndCondition condition,  IFormCollection collection)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                condition.DateCreated = DateTime.Now;
                condition.DateLastModified = DateTime.Now;
                condition.CreatedBy = signedInUserId;
                condition.LastModifiedBy = signedInUserId;

                _databaseConnection.TermsAndConditions.Add(condition);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new T&C!";
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
        public ActionResult Edit(TermAndCondition condition,IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                condition.DateLastModified = DateTime.Now;
                condition.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(condition).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the T&C!";
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
            var id = Convert.ToInt64(collection["TermAndConditionId"]);
            var condition = _databaseConnection.TermsAndConditions.Find(id);

            _databaseConnection.TermsAndConditions.Remove(condition);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the T&C!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}