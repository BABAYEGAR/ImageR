using System;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Image.Data;
using Image.Models;
using Image.Models.APIFactory;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Image.Models.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ImageDataContext _databaseConnection;
        private readonly IHostingEnvironment _hostingEnv;
        public Role UserRole;
        private readonly List<AppUser> _users;

        public CompetitionController(IHostingEnvironment env, ImageDataContext databaseConnection)
        {
            _hostingEnv = env;
            _databaseConnection = databaseConnection;
            _users = new AppUserFactory().GetAllUsersAsync(new AppConfig().FetchUsersUrl).Result;
        }


        // GET: Package
        [SessionExpireFilter]
        public ActionResult Index()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            List<Competition> competitions = new List<Competition>();
            if (HttpContext.Session.GetString("Role") != null)
            {
                var roleString = HttpContext.Session.GetString("Role");
                UserRole = JsonConvert.DeserializeObject<Role>(roleString);
            }
            if (UserRole.ParticipateCompetition)
            {
                            competitions = (from a in _databaseConnection.Competition
                join b in _databaseConnection.CompetitionCategories
                on a.CompetitionId equals b.CompetitionId
                join c in _databaseConnection.PhotographerCategoryMappings on b.PhotographerCategoryId
                equals c.PhotographerCategoryId
                where c.AppUserId == signedInUserId
                select a).ToList();
            }
            if (UserRole.ManageCompetition)
            {
                competitions = _databaseConnection.Competition.ToList();
            }
            ViewBag.Role = UserRole;
            return View(competitions);
        }
        [SessionExpireFilter]
        public ActionResult CompetitionUpload(long? id)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            List<CompetitionUpload> competitionUploads = new List<CompetitionUpload>();
            if (HttpContext.Session.GetString("Role") != null)
            {
                var roleString = HttpContext.Session.GetString("Role");
                UserRole = JsonConvert.DeserializeObject<Role>(roleString);
            }
        
            if (UserRole.ManageCompetition)
            {
                competitionUploads = id != null ? _databaseConnection.CompetitionUploads.Include(n=>n.Competition).Include(n=>n.Location).Include(n=>n.Camera)
                    .Where(n=>n.CompetitionId == id).ToList() : _databaseConnection.CompetitionUploads
                    .Include(n => n.Competition).Include(n => n.Location).Include(n => n.Camera).ToList();
            }
            if (UserRole.ParticipateCompetition)
            {
                competitionUploads = id != null ? _databaseConnection.CompetitionUploads.Include(n => n.Competition).Include(n => n.Location).Include(n => n.Camera)
                    .Where(n => n.AppUserId == signedInUserId && n.CompetitionId == id).ToList() : 
                    _databaseConnection.CompetitionUploads.Include(n => n.Competition).Include(n => n.Location).Include(n => n.Camera)
                    .Where(n => n.AppUserId == signedInUserId)
                    .ToList();
            }
            ViewBag.Role = UserRole;
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

        public ActionResult SelectCategories(long id)
        {
            ViewBag.CompettitonId = id;
            ViewBag.Mapping = _databaseConnection.CompetitionCategories.ToList();
            return View(_databaseConnection.PhotographerCategories.OrderByDescending(n => n.Name).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapPhotographyCategory(int[] table_records, IFormCollection collection)
        {
            var allMappings = _databaseConnection.CompetitionCategories.ToList();
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var competitionId = Convert.ToInt64(collection["CompetitionId"]);
            var competition = _databaseConnection.Competition.Find(competitionId);
            if (table_records != null)
            {
                var length = table_records.Length;
                for (var i = 0; i < length; i++)
                {
                    var id = table_records[i];
                    var singleMapping = allMappings.SingleOrDefault(
                        n =>
                            n.PhotographerCategoryId == id && n.CompetitionId == competitionId);

                    if (singleMapping != null)
                    {
                    }
                    else
                    {
                        var competitionMapping = new CompetitionCategory
                        {
                            PhotographerCategoryId = id,
                            CompetitionId = competitionId,
                            DateCreated = DateTime.Now,
                            DateLastModified = DateTime.Now,
                            LastModifiedBy = signedInUserId,
                            CreatedBy = signedInUserId
                        };
                        _databaseConnection.CompetitionCategories.Add(competitionMapping);
                        _databaseConnection.SaveChanges();

                        var users = (from a in _users
                            join b in _databaseConnection.PhotographerCategoryMappings
                            on a.AppUserId equals b.AppUserId
                            join c in _databaseConnection.CompetitionCategories on competitionMapping.CompetitionId
                            equals competitionId
                            where c.PhotographerCategoryId == b.PhotographerCategoryId
                            select a).ToList();

                        foreach (var item in users)
                        {
                            var notification = new SystemNotification
                            {
                                AppUserId = item.AppUserId,
                                ControllerId = competitionId,
                                Read = false,
                                Message = "Check out the new Phtotogragh Category Competiton",
                                DateCreated = DateTime.Now,
                                DateLastModified = DateTime.Now,
                                LastModifiedBy = signedInUserId,
                                CreatedBy = signedInUserId
                            };
                            _databaseConnection.SystemNotifications.AddRange(notification);
                            var link = _hostingEnv.WebRootPath;
                            var mail = new Mailer();
                            mail.SendCompetitionEmail(new AppConfig().CompetitionHtml, item, competition);
                        }
                        _databaseConnection.SaveChanges();

                        TempData["display"] = "you have succesfully added the category(s) to the competition!";
                        TempData["notificationtype"] = NotificationType.Success.ToString();
                    }
                }
            }
            else
            {
                TempData["display"] = "no category has been selected!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return RedirectToAction("Index", "Competition");
            }
            return RedirectToAction("SelectCategories", "Competition",new{id=competitionId});
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
                var signedInUserId = HttpContext.Session.GetInt32("userId");
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
            var signedInUserId = HttpContext.Session.GetInt32("userId");

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
                    // TODO: Add insert logic here
                    var signedInUserId = HttpContext.Session.GetInt32("userId");
                    competitionUpload.AppUserId = signedInUserId;
                    competitionUpload.CompetitionId = Convert.ToInt64(collection["CompetitionId"]);
                    competitionUpload.DateCreated = DateTime.Now;
                    competitionUpload.DateLastModified = DateTime.Now;
                    competitionUpload.CreatedBy = signedInUserId;
                    competitionUpload.LastModifiedBy = signedInUserId;
                    competitionUpload.FileName = DateTime.Now.ToFileTime().ToString();

                    //upload image via Cloudinary API Call
                    var account = new Account(
                        new Config().CloudinaryAccoutnName,
                        new Config().CloudinaryApiKey,
                        new Config().CloudinaryApiSecret);

                    var cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(competitionUpload.FileName, file.OpenReadStream())
                    };
                    var uploadResult = cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Result.Format != null)
                    {
                        competitionUpload.FilePath = uploadResult.Result.Uri.AbsolutePath;
                        _databaseConnection.CompetitionUploads.Add(competitionUpload);
                        _databaseConnection.SaveChanges();
                    }
                    if (competitionUpload.CompetitionUploadId > 0)
                    {
                        var competition = _databaseConnection.Competition.Find(competitionUpload.CompetitionId);
                        var rating =
                            new ImageCompetitionRating
                            {
                                CompetitionUploadId = competitionUpload.CompetitionUploadId,
                                DateCreated = DateTime.Now,
                                DateLastModified = DateTime.Now,
                                CreatedBy = signedInUserId,
                                LastModifiedBy = signedInUserId,
                                TimeDeliveryRating = new CompetitionCalculator().CalculateTimeRating(competition.EndDate, competitionUpload.DateCreated)
                    };
                        _databaseConnection.ImageCompetitionRatings.Add(rating);
                        _databaseConnection.SaveChanges();
                    }

                    //display notification
                    TempData["display"] = "You have successfully uploaded an image for the competition!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
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
        public ActionResult CompetitionImageRating(long? id)
        {
            var competitionRating = _databaseConnection.ImageCompetitionRatings.SingleOrDefault(n=>n.CompetitionUploadId == id);
            return View(competitionRating);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult CompetitionImageRating(ImageCompetitionRating competitionRating)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            competitionRating.LastModifiedBy = signedInUserId;
            competitionRating.DateLastModified = DateTime.Now;
            _databaseConnection.Entry(competitionRating).State = EntityState.Modified;
            _databaseConnection.SaveChanges();

            long competitionId = _databaseConnection.CompetitionUploads.Find(competitionRating.CompetitionUploadId).CompetitionId;

            //display notification
            TempData["display"] = "You have successfully updated the rating of the contestant!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("CompetitionUpload", new { id = competitionId });
        }
        [SessionExpireFilter]
        public ActionResult AutoSelectWinner(long? competitionId, long userId)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var competitionUploads = _databaseConnection.CompetitionUploads.Where(n=>n.CompetitionId == competitionId);
            var competition = _databaseConnection.Competition.Find(competitionId);
            foreach (var item in competitionUploads)
            {
                //get rating
                var rating =
                    _databaseConnection.ImageCompetitionRatings.Include(n=>n.CompetitionUpload).SingleOrDefault(
                        n => n.CompetitionUpload.AppUserId == item.AppUserId);
                rating.AcceptanceRating = new CompetitionCalculator().CalculateUserAcceptanceRating(_users.Count,item.Vote);
                rating.TotalRating = rating.AcceptanceRating + rating.ClearityRating + rating.ConceptRating +
                                     rating.DescriptionRating + rating.QualityRating + rating.TimeDeliveryRating;

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
        public ActionResult RemovePhographerCategoryMapping(IFormCollection collection)
        {
            try
            {
                var competitionId = Convert.ToInt64(collection["CompetitionId"]);
                var categoryId = Convert.ToInt64(collection["PhtographerCategoryId"]);
                var competition =
                    _databaseConnection.CompetitionCategories.SingleOrDefault(
                        n => n.PhotographerCategoryId == categoryId && n.CompetitionId == competitionId);

                _databaseConnection.CompetitionCategories.Remove(competition);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully removed the Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("SelectCategories", new {id = competitionId});
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