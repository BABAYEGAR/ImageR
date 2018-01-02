using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using CamerackStudio.Models.RabbitMq;
using CamerackStudio.Models.Redis;
using CamerackStudio.Models.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CamerackStudio.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private AppUser _appUser;
        private readonly List<AppUser> _users;

        public CompetitionController(IHostingEnvironment env, CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result;
        }


        // GET: Package
        [SessionExpireFilter]
        public ActionResult Index()
        {
         var competitions = _databaseConnection.Competition.Where(n=>n.Status == CompetitionStatus.Open.ToString()).ToList();
            return View(competitions);
        }
        [SessionExpireFilter]
        public ActionResult CompetitionUpload(long? id)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            List<CompetitionUpload> competitionUploads = new List<CompetitionUpload>();
            if (_appUser.Role.ManageCompetition)
            {
                competitionUploads = id != null ? _databaseConnection.CompetitionUploads.Include(n=>n.Competition).Include(n=>n.Location).Include(n=>n.Camera)
                    .Where(n=>n.CompetitionId == id).ToList() : _databaseConnection.CompetitionUploads
                    .Include(n => n.Competition).Include(n => n.Location).Include(n => n.Camera).ToList();
            }
            if (_appUser.Role.ParticipateCompetition)
            {
                competitionUploads = id != null ? _databaseConnection.CompetitionUploads.Include(n => n.Competition).Include(n => n.Location).Include(n => n.Camera)
                    .Where(n => n.AppUserId == signedInUserId && n.CompetitionId == id).ToList() : 
                    _databaseConnection.CompetitionUploads.Include(n => n.Competition).Include(n => n.Location).Include(n => n.Camera)
                    .Where(n => n.AppUserId == signedInUserId)
                    .ToList();
            }
            return View(competitionUploads);
        }
        // GET: Package/Create
        [SessionExpireFilter]
        public ActionResult Create()
        {
         
            return View();
        }

        // POST: Package/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competition competition, IFormCollection collection)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                competition.DateCreated = DateTime.Now;
                competition.DateLastModified = DateTime.Now;
                competition.CreatedBy = signedInUserId;
                competition.LastModifiedBy = signedInUserId;
                competition.Status = CompetitionStatus.Open.ToString();

                _databaseConnection.Competition.Add(competition);
                _databaseConnection.SaveChanges();

                foreach (var item in _users)
                {

                }
                new SendEmailMessage().SendCompetitionEmailMessage(competition);
                _databaseConnection.SaveChanges();
                //display notification
                TempData["display"] = "You have successfully added a new Competition, emails and notifications have been dispatched!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Package/Edit/5
        [SessionExpireFilter]
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.Competition.Find(id));
        }

        // POST: Package/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(Competition competition, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                competition.DateLastModified = DateTime.Now;
                competition.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(competition).State = EntityState.Modified;
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

        // GET: Image/Create
        [SessionExpireFilter]
        public ActionResult UploadImage(long id)
        {
            // TODO: Add insert logic here
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));

            ViewBag.Competition = _databaseConnection.Competition.Find(id);
            //view bags for lists
            ViewBag.CameraId = new SelectList(_databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                "Name");
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                "Name");
            return View();
        }

        // POST: Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult UploadImage(CompetitionUpload competitionUpload, IFormCollection collection, IFormFile file)
        {
            try
            {
                    var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                    competitionUpload.AppUserId = signedInUserId;
                    competitionUpload.CompetitionId = Convert.ToInt64(collection["CompetitionId"]);
                    competitionUpload.DateCreated = DateTime.Now;
                    competitionUpload.DateLastModified = DateTime.Now;
                    competitionUpload.CreatedBy = signedInUserId;
                    competitionUpload.LastModifiedBy = signedInUserId;
                    competitionUpload.FileName = DateTime.Now.ToFileTime().ToString();

                //Append new upload object and send task to rabbit MQ API Via CamerackImageUploader API APPLICATION
                string stream = null;
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        stream = Convert.ToBase64String(fileBytes);
                    }
                }
                competitionUpload.FilePath = stream;
                //send message to rabbit queue to queue process
                new SendImageMessage().SendCompetitionImageUploadMessage(competitionUpload);
             

                    //display notification
                    TempData["display"] = "You have successfully uploaded an image for the competition!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");

            }
            catch (Exception)
            {
                // TODO: Add insert logic here
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                ViewBag.CameraId = new SelectList(_databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                    "Name",competitionUpload.CameraId);
                ViewBag.LocationId = new SelectList(_databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                    "Name", competitionUpload.LocationId);
                ViewBag.Competition = _databaseConnection.Competition.Find(competitionUpload.CompetitionId);
                //display notification
                TempData["display"] = "There was an issue uploading the image, Try Again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(competitionUpload);
            }
        }
        [SessionExpireFilter]
        public ActionResult AutoSelectWinner(long? competitionId, long userId)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var competitionUploads = _databaseConnection.CompetitionUploads.Where(n=>n.CompetitionId == competitionId);
            var competition = _databaseConnection.Competition.Find(competitionId);
            foreach (var item in competitionUploads)
            {

                //get rating
                var rating =
                    _databaseConnection.ImageCompetitionRatings.Include(n=>n.CompetitionUpload).SingleOrDefault(
                        n => n.CompetitionUpload.AppUserId == item.AppUserId);
                rating.AcceptanceRating = new CompetitionCalculator().CalculateUserAcceptanceRating(_users.Count,item.Vote);
                rating.TotalRating = rating.AcceptanceRating + 
                                     rating.DescriptionRating + rating.TimeDeliveryRating;

                //update rating
                _databaseConnection.Entry(rating).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //get winner
                var winner =
                    _databaseConnection.ImageCompetitionRatings.Include(n=>n.CompetitionUpload).Where(
                        n => n.CompetitionUploadId == item.CompetitionUploadId).OrderByDescending(n => n.TotalRating).FirstOrDefault();
                competition.AppUserId = winner.CompetitionUpload.AppUserId;
            }
        
         
            competition.AppUserId = userId;
            competition.Status = CompetitionStatus.Closed.ToString();
            competition.DateLastModified = DateTime.Now;
            competition.LastModifiedBy = signedInUserId;
            _databaseConnection.Entry(competition).State = EntityState.Modified;
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully selected the winner!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("CompetitionUpload", new { id = competitionId });
        }
        [SessionExpireFilter]
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
                return RedirectToAction("Index");
            }
        }
    }
}