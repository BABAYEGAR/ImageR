using System;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Image.Data;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
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
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View(_databaseConnection.Competition.ToList());
        }

        // GET: Package/Details/5
        [SessionExpireFilter]
        public ActionResult Details(int id)
        {
            return View();
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

                        var users = (from a in _databaseConnection.AppUsers
                            join b in _databaseConnection.PhotographerCategoryMappings
                            on a.AppUserId equals b.AppUserId
                            join c in _databaseConnection.CompetitionCategories on competitionMapping.CompetitionId
                            equals competitionId
                            where c.PhotographerCategoryId == b.PhotographerCategoryId
                            select a).ToList();

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
            return RedirectToAction("Index", "Competition");
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
                ;
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
        public ActionResult UploadImage()
        {
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
                if (file != null && file.Length < 15728640)
                {
                    // TODO: Add insert logic here
                    var signedInUserId = HttpContext.Session.GetInt32("userId");
                    competitionUpload.AppUserId = signedInUserId;
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
                    var uploadResult = cloudinary.Upload(uploadParams);

                    if (uploadResult.Format != null)
                    {
                        competitionUpload.FilePath = uploadResult.Uri.AbsolutePath;
                        _databaseConnection.CompetitionUploads.Add(competitionUpload);
                        _databaseConnection.SaveChanges();
                    }

                    //display notification
                    TempData["display"] = "You have successfully uploaded a an image for the competition!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");
                }
                {
                    //display notification
                    TempData["display"] =
                        "No Image was Selected or selected image violate the Upload Size Rule of 15MB!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(competitionUpload);
                }
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = "There was an issue uploading the image, Try Again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(competitionUpload);
            }
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