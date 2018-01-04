using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using CamerackStudio.Models.RabbitMq;
using CamerackStudio.Models.Redis;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class ImageController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private readonly List<AppUser> _users;
        private List<Image> _images = new List<Image>();
        private AppUser _appUser;

        public ImageController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result;
        }

        // GET: Image
        [SessionExpireFilter]
        public ActionResult Index(string status)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            if (new RedisDataAgent().GetStringValue("CamerackLoggedInUser") != null)
            {
                var userString = new RedisDataAgent().GetStringValue("CamerackLoggedInUser");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            }
            if (_appUser.Role.UploadImage)
                if (status != null)
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                        .Where(n => n.AppUserId == signedInUserId).Where(n => n.Status == status).ToList();
                else
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                        .Where(n => n.AppUserId == signedInUserId).ToList();
            if (_appUser.Role.ManageImages)
                if (status != null)
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(n => n.Status == status)
                        .ToList();
                else
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).ToList();
            ViewBag.status = status;
            ViewBag.Role = _appUser.Role;
            return View(_images);
        }

        // GET: Image
        [SessionExpireFilter]
        public ActionResult Report()
        {
            return View(_databaseConnection.ImageReports.ToList());
        }

        /// <summary>
        ///     Sends Json responds object to view with sub categories of the categories requested via an Ajax call
        /// </summary>
        /// <param name="id"> state id</param>
        /// <returns>lgas</returns>
        /// Microsoft.CodeDom.Providers.DotNetCompilerPlatform
        public JsonResult GetSubForCategories(long id)
        {
            return Json(_databaseConnection.ImageSubCategories.Where(n => n.ImageCategoryId == id).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult RateImage(IFormCollection collection, ImageAction action)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            action.Action = "Rating";
            action.ActionDate = DateTime.Now;
            action.AppUserId = signedInUserId;
            action.OwnerId = signedInUserId;
            action.ImageId = Convert.ToInt64(collection["ImageId"]);
            action.Rating = Convert.ToInt64(collection["rating"]);
            action.ClientId = new AppConfig().ClientId;
            if (_databaseConnection.ImageActions
                    .Where(n => n.ImageId == action.ImageId && n.AppUserId == action.AppUserId).ToList().Count <= 0)
            {
                _databaseConnection.Add(action);
                _databaseConnection.SaveChanges();
            }
            var image = _images.SingleOrDefault(n => n.ImageId == action.ImageId);
            var appTransport = new AppTransport
            {
                AppUsers = _users,
                Images = _databaseConnection.Images.Include(n => n.ImageCategory)
                    .Include(n => n.ImageComments).Include(n => n.ImageTags)
                    .Include(n => n.Location).Include(n => n.ImageSubCategory).ToList(),
                ImageComments = _databaseConnection.ImageComments.ToList(),
                ImageActions = _databaseConnection.ImageActions.ToList(),
                Image = image,
                AppUser = _users.SingleOrDefault(n => n.AppUserId == signedInUserId)
            };
            return PartialView("Partials/_PartialRating", appTransport);
        }

        // GET: Image/Details/5
        [SessionExpireFilter]
        public async Task<ActionResult> ApproveOrRejectImage(long id, string status)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var image = _databaseConnection.Images.Find(id);
            if (status == ImageStatus.Accepted.ToString())
                image.Status = ImageStatus.Accepted.ToString();
            if (status == ImageStatus.Rejected.ToString())
                image.Status = ImageStatus.Rejected.ToString();
            var imageFile = "https://res.cloudinary.com/" + image.FilePath;


            //upload image via Cloudinary API Call
            var account = new Account(
                new AppConfig().CloudinaryAccoutnName,
                new AppConfig().CloudinaryApiKey,
                new AppConfig().CloudinaryApiSecret);

            var cloudinary = new Cloudinary(account);
            var delParams = new DelResParams
            {
                PublicIds = new List<string> {imageFile},
                Invalidate = true
            };
            await cloudinary.DeleteResourcesAsync(delParams);

            var tags = _databaseConnection.ImageTags.Where(n => n.ImageId == image.ImageId);
            foreach (var item in tags)
                _databaseConnection.ImageTags.RemoveRange(item);
            var imageActions = _databaseConnection.ImageActions.Where(n => n.ImageId == image.ImageId);
            foreach (var item in imageActions)
                _databaseConnection.ImageActions.RemoveRange(item);
            var imageComments = _databaseConnection.ImageComments.Where(n => n.ImageId == image.ImageId);
            foreach (var item in imageComments)
                _databaseConnection.ImageComments.RemoveRange(item);
            var imageReports = _databaseConnection.ImageReports.Where(n => n.ImageId == image.ImageId);
            foreach (var item in imageReports)
                _databaseConnection.ImageReports.RemoveRange(item);

            _databaseConnection.Entry(image).State = EntityState.Modified;
            _databaseConnection.SaveChanges();

            image.DateLastModified = DateTime.Now;
            image.LastModifiedBy = signedInUserId;
            _databaseConnection.Entry(image).State = EntityState.Modified;
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully " + image.Status + " the image!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        // GET: Image/Create
        [SessionExpireFilter]
        public ActionResult Create(long? id)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(), "ImageCategoryId",
                "Name");
            ViewBag.CameraId = new SelectList(
                _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                "Name");
            ViewBag.LocationId = new SelectList(
                _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                "Name");
            ViewBag.CompetitionId = new SelectList(
                _databaseConnection.Competition.Where(n => n.Status == CompetitionStatus.Open.ToString()).ToList(), "CompetitionId",
                "Name");
            var image = new Image
            {
                SellingPrice = 0,
                Discount = 0
            };
            if (id != null)
            {
                image.CompetitionId = id;
            }
            return View(image);
        }

        // POST: Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableFormValueModelBinding]
        [SessionExpireFilter]
        public ActionResult Create(Image image, IFormCollection collection, IFormFile file)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            try
            {
                image.AppUserId = signedInUserId;
                image.DateCreated = DateTime.Now;
                image.DateLastModified = DateTime.Now;
                image.CreatedBy = signedInUserId;
                image.LastModifiedBy = signedInUserId;
                image.FileName = DateTime.Now.ToFileTime().ToString();
                image.Status = ImageStatus.Accepted.ToString();

                //collect data from form as model binding is disabled
                if (!String.IsNullOrEmpty(collection["Title"]))
                {
                    image.Title = collection["Title"];
                }
                if (!String.IsNullOrEmpty(collection["Theme"]))
                {
                    image.Theme = collection["Theme"];
                }
                if (!String.IsNullOrEmpty(collection["Inspiration"]))
                {
                    image.Inspiration = collection["Inspiration"];
                }
                if (!String.IsNullOrEmpty(collection["Tags"]))
                {
                    image.Tags = collection["Tags"];
                }
                if (!String.IsNullOrEmpty(collection["CameraId"]))
                {
                    image.CameraId = Convert.ToInt64(collection["CameraId"]);
                }
                if (!String.IsNullOrEmpty(collection["LocationId"]))
                {
                    image.LocationId = Convert.ToInt64(collection["LocationId"]);
                }
                if (!String.IsNullOrEmpty(collection["ImageCategoryId"]))
                {
                    image.ImageCategoryId = Convert.ToInt64(collection["ImageCategoryId"]);
                }
                if (!String.IsNullOrEmpty(collection["ImageSubCategoryId"]))
                {
                    image.ImageSubCategoryId = Convert.ToInt64(collection["ImageSubCategoryId"]);
                }
                if (!String.IsNullOrEmpty(collection["SellingPrice"]))
                {
                    image.SellingPrice = Convert.ToInt64(collection["SellingPrice"]);
                    image.Discount = Convert.ToInt64(collection["Discount"]);
                }
                else
                {
                    image.SellingPrice = 0;
                    image.Discount = 0;
                }
                if (!String.IsNullOrEmpty(collection["Description"]))
                {
                    image.Description = collection["Description"];
                }
                image.Featured = false;
                image.A1 = Convert.ToBoolean(collection["A1"]);
                image.A2 = Convert.ToBoolean(collection["A2"]);
                image.A3 = Convert.ToBoolean(collection["A3"]);
                image.A4 = Convert.ToBoolean(collection["A4"]);
                image.A5 = Convert.ToBoolean(collection["A5"]);
                image.A6 = Convert.ToBoolean(collection["A6"]);

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

                var upload = new ImageUpload
                {
                    Image = image,
                    File = stream
                };
                //send message to rabbit queue to queue process
                new SendImageMessage().SendImageCreationMessage(upload);

                //display notification
                TempData["display"] = "Your image is uploading in the background continue your work while it uploads!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                if (image.CompetitionId != null)
                {
                    return RedirectToAction("Uploads","Competition");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = ex.ToString();
                TempData["notificationtype"] = NotificationType.Error.ToString();
                ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(),
                    "ImageCategoryId",
                    "Name", image.ImageCategoryId);
                ViewBag.CameraId = new SelectList(
                    _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                    "Name", image.CameraId);
                ViewBag.LocationId = new SelectList(
                    _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                    "Name",image.LocationId);
                return View(image);
            }
        }
        // GET: Image/Edit/5
        [SessionExpireFilter]
        public ActionResult SetAsFeatured(long id)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));

            var image = _databaseConnection.Images.Find(id);
            image.Featured = true;
            image.DateLastModified = DateTime.Now;
            image.LastModifiedBy = signedInUserId;

            _databaseConnection.Entry(image).State = EntityState.Modified;
            _databaseConnection.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Image/Edit/5
        [SessionExpireFilter]
        public ActionResult Edit(long id)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var image = _databaseConnection.Images.Find(id);
            ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(), "ImageCategoryId",
                "Name", image.ImageCategoryId);
            ViewBag.CameraId = new SelectList(
                _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                "Name", image.CameraId);
            ViewBag.LocationId = new SelectList(
                _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                "Name", image.LocationId);
            return View(image);
        }

        // POST: Image/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(Image image, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                image.DateLastModified = DateTime.Now;
                image.LastModifiedBy = signedInUserId;

                //collect data from form as model binding is disabled
                if (!String.IsNullOrEmpty(collection["Title"]))
                {
                    image.Title = collection["Title"];
                }
                if (!String.IsNullOrEmpty(collection["Theme"]))
                {
                    image.Theme = collection["Theme"];
                }
                if (!String.IsNullOrEmpty(collection["Inspiration"]))
                {
                    image.Inspiration = collection["Inspiration"];
                }
                if (!String.IsNullOrEmpty(collection["CameraId"]))
                {
                    image.CameraId = Convert.ToInt64(collection["CameraId"]);
                }
                if (!String.IsNullOrEmpty(collection["LocationId"]))
                {
                    image.LocationId = Convert.ToInt64(collection["LocationId"]);
                }
                if (!String.IsNullOrEmpty(collection["ImageCategoryId"]))
                {
                    image.ImageCategoryId = Convert.ToInt64(collection["ImageCategoryId"]);
                }
                if (!String.IsNullOrEmpty(collection["ImageSubCategoryId"]))
                {
                    image.ImageSubCategoryId = Convert.ToInt64(collection["ImageSubCategoryId"]);
                }
                if (!String.IsNullOrEmpty(collection["SellingPrice"]))
                {
                    image.SellingPrice = Convert.ToInt64(collection["SellingPrice"]);
                    image.Discount = Convert.ToInt64(collection["Discount"]);
                }
                else
                {
                    image.SellingPrice = 0;
                    image.Discount = 0;
                }
                if (!String.IsNullOrEmpty(collection["Description"]))
                {
                    image.Description = collection["Description"];
                }


                _databaseConnection.Entry(image).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the image information!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                //display notification
                TempData["display"] = ex.ToString();
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(image);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> PostImageComment(ImageComment comment, IFormCollection collection)
        {
            long? imageId = Convert.ToInt64(collection["ImageId"]);
            var image = _databaseConnection.Images.Find(imageId);
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            comment.DateCreated = DateTime.Now;
            comment.AppUserId = signedInUserId;
            comment.ImageId = imageId;
            comment.Comment = collection["Comment"].ToString();
            comment.CreatedBy = signedInUserId;
            comment.LastModifiedBy = signedInUserId;
            comment.DateLastModified = DateTime.Now;
            _databaseConnection.ImageComments.Add(comment);
            _databaseConnection.SaveChanges();

            //send notification for owner of photo
            if (image.AppUserId != comment.CreatedBy)
            {
                var notification = new PushNotification
                {
                    AppUserId = image.AppUserId,
                    CreatedBy = signedInUserId,
                    LastModifiedBy = signedInUserId,
                    DateLastModified = DateTime.Now,
                    DateCreated = DateTime.Now,
                    Category = SystemNotificationCategory.Comment.ToString(),
                    Read = false,
                    ControllerId = imageId
                };
                var singleOrDefault = _users.SingleOrDefault(n => n.AppUserId == comment.CreatedBy);
                if (singleOrDefault != null)
                    notification.Message = singleOrDefault.Name +
                                           " Commented on your Image";
                await new AppUserFactory().SavePushNotification(new AppConfig().SavePushNotifications, notification);
            }
            var newComments = _databaseConnection.ImageComments.Where(n => n.ImageId == imageId).ToList();
            var appTransport = new AppTransport
            {
                ImageComments = newComments,
                AppUsers = _users,
                Images = _databaseConnection.Images.Include(n => n.ImageCategory)
                    .Include(n => n.ImageComments).Include(n => n.ImageTags)
                    .Include(n => n.Location).Include(n => n.ImageSubCategory).ToList(),
                ImageActions = _databaseConnection.ImageActions.ToList(),
                Image = image,
                AppUser = _users.SingleOrDefault(n => n.AppUserId == signedInUserId)
            };
            return PartialView("Partials/_ImageComment", appTransport);
        }

        public PartialViewResult DeleteComment(IFormCollection collection)
        {
            long? commentId = Convert.ToInt64(collection["CommentId"]);
            long? imageId = Convert.ToInt64(collection["ImageId"]);
            var image = _databaseConnection.Images.Find(imageId);
            var comment = _databaseConnection.ImageComments.Find(commentId);
            _databaseConnection.ImageComments.Remove(comment);
            _databaseConnection.SaveChanges();
            var newComments = _databaseConnection.ImageComments.Where(n => n.ImageId == imageId).ToList();
            var appTransport = new AppTransport
            {
                ImageComments = newComments,
                AppUsers = _users,
                Images = _databaseConnection.Images.Include(n => n.ImageCategory)
                    .Include(n => n.ImageComments).Include(n => n.ImageTags)
                    .Include(n => n.Location).Include(n => n.ImageSubCategory).ToList(),
                ImageActions = _databaseConnection.ImageActions.ToList(),
                Image = image
            };
            return PartialView("Partials/_ImageComment", appTransport);
        }

        [HttpGet]
        public PartialViewResult ReloadImageComments(long? id)
        {
            var imageComments = _databaseConnection.ImageComments.Where(n => n.ImageId == id).ToList();

            var appTransport = new AppTransport
            {
                ImageComments = imageComments,
                AppUsers = _users
            };
            return PartialView("Partials/_ImageComment", appTransport);
        }

        // POST: Image/Delete/5
        [SessionExpireFilter]
        public async Task<ActionResult> Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["ImageId"]);
            var image = _databaseConnection.Images.Find(id);
            var imageFile = "https://res.cloudinary.com/" + image.FilePath;


            //upload image via Cloudinary API Call
            var account = new Account(
                new AppConfig().CloudinaryAccoutnName,
                new AppConfig().CloudinaryApiKey,
                new AppConfig().CloudinaryApiSecret);

            var cloudinary = new Cloudinary(account);
            var delParams = new DelResParams
            {
                PublicIds = new List<string> {imageFile},
                Invalidate = true
            };
            await cloudinary.DeleteResourcesAsync(delParams);

            var tags = _databaseConnection.ImageTags.Where(n => n.ImageId == image.ImageId);
            foreach (var item in tags)
                _databaseConnection.ImageTags.RemoveRange(item);
            var imageActions = _databaseConnection.ImageActions.Where(n => n.ImageId == image.ImageId);
            foreach (var item in imageActions)
                _databaseConnection.ImageActions.RemoveRange(item);
            var imageComments = _databaseConnection.ImageComments.Where(n => n.ImageId == image.ImageId);
            foreach (var item in imageComments)
                _databaseConnection.ImageComments.RemoveRange(item);
            var imageReports = _databaseConnection.ImageReports.Where(n => n.ImageId == image.ImageId);
            foreach (var item in imageReports)
                _databaseConnection.ImageReports.RemoveRange(item);

            _databaseConnection.Images.Remove(image);
            _databaseConnection.SaveChanges();


            //display notification
            TempData["display"] = "You have successfully deleted the Image from your Library!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }
    }
}