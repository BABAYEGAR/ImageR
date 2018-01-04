using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private readonly List<AppUser> _users;
        private AppUser _appUser;

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
            var appTransport = new AppTransport();
            appTransport.Images = _databaseConnection.Images.ToList();
            appTransport.Competitions = competitions;
            return View(appTransport);
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
        [SessionExpireFilter]
        public ActionResult Uploads(long? id)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            List<Image> images = null;
            if (new RedisDataAgent().GetStringValue("CamerackLoggedInUser") != null)
            {
                var userString = new RedisDataAgent().GetStringValue("CamerackLoggedInUser");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            }
            if (_appUser.Role.UploadImage)
                if (id != null)
                    images = _databaseConnection.Images.Include(n => n.Competition)
                        .Where(n => n.AppUserId == signedInUserId && n.CompetitionId > 0).Where(n => n.CompetitionId == id).ToList();
                else
                    images = _databaseConnection.Images.Include(n => n.Competition).Where(n => n.AppUserId == signedInUserId && n.CompetitionId > 0).ToList();
            if (_appUser.Role.ManageImages)
                if (id != null)
                    images = _databaseConnection.Images.Include(n => n.Competition).Where(n => n.CompetitionId == id && n.CompetitionId > 0)
                        .ToList();
                else
                    images = _databaseConnection.Images.Include(n => n.Competition).Where(n=> n.CompetitionId > 0).ToList();


            return View(images);
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
        [SessionExpireFilter]
        public void AutoSelectWinner()
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));

            //get list of competitions
            var competitions = _databaseConnection.Competition.ToList();

            //get all competitions that end date is reached
            foreach (var item in competitions.Where(n=>n.EndDate == DateTime.Now))
            {
                //get image upload for the competitions
                var competitionUploads = _databaseConnection.Images.Where(n => n.CompetitionId == item.CompetitionId);
                
                foreach (var items in competitionUploads)
                {
                    //get rating
                    var rating =
                        _databaseConnection.ImageCompetitionRatings.SingleOrDefault(n => n.AppUserId == item.AppUserId &&
                        n.CompetitionId == item.CompetitionId);

                    //get image action
                    var imageAction = _databaseConnection.ImageActions.Where(n => n.ImageId == items.ImageId).ToList();
                    rating.AcceptanceRating = new CompetitionCalculator().CalculateUserAcceptanceRating(_users.Count, imageAction.Count);
                    rating.TotalRating = rating.AcceptanceRating +
                                         rating.DescriptionRating + rating.TimeDeliveryRating + rating.TagsRating;


                    //update rating and save transaction
                    _databaseConnection.Entry(rating).State = EntityState.Modified;
                    _databaseConnection.SaveChanges();

                    //get winner
                    var winner =
                        _databaseConnection.ImageCompetitionRatings.Where(
                            n => n.CompetitionId == item.CompetitionId).OrderByDescending(n => n.TotalRating).FirstOrDefault();

                    //append and populate object
                    var uploadCompetition = item;
                    uploadCompetition.AppUserId = winner.AppUserId;
                    uploadCompetition.Status = CompetitionStatus.Closed.ToString();
                    uploadCompetition.DateLastModified = DateTime.Now;
                    uploadCompetition.LastModifiedBy = signedInUserId;

                    //save transaction to database
                    _databaseConnection.Entry(item).State = EntityState.Modified;
                    _databaseConnection.SaveChanges();
                }
         
            }
        
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