using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
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
        private AppUser _appUser;
        private List<Image> _images = new List<Image>();

        public ImageController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result;
        }

        // GET: Image
        [SessionExpireFilter]
        public ActionResult Index(string status, string notify)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));

            if (HttpContext.Session.GetString("StudioLoggedInUser") != null)
            {
                var userString = HttpContext.Session.GetString("StudioLoggedInUser");
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
            if (!string.IsNullOrEmpty(notify))
            {
                if (notify == "success")
                {
                    //display notification
                    TempData["display"] = "The image is being uploaded to the server, Continue our work!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                }
                if (notify == "fail")
                {
                    //display notification
                    TempData["display"] = "There was an issue uploading the image, Try again Later!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                }
            }
            return View(_images);
        }

        // GET: Image
        [SessionExpireFilter]
        public ActionResult Report()
        {
            return View(_databaseConnection.ImageReports.Include(n => n.Image).ToList());
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
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
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
            var image = _databaseConnection.Images.SingleOrDefault(n => n.ImageId == action.ImageId);
            var appTransport = new AppTransport
            {
                AppUsers = _users,
                Images = _databaseConnection.Images.ToList(),
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
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
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
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(), "ImageCategoryId",
                "Name");
            ViewBag.CameraId = new SelectList(
                _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                "Name");
            ViewBag.LocationId = new SelectList(
                _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                "Name");
            var image = new Image
            {
                SellingPrice = 0,
                Discount = 0
            };
            return View(image);
        }

        // POST: Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[DisableFormValueModelBinding]
        [SessionExpireFilter]
        public ActionResult Create(Image image, IFormCollection collection, IFormFile FileName)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            var tags = new List<string>();
            var newLocation = new Location();
            var newCamera = new Camera();
            try
            {
                if (!string.IsNullOrEmpty(FileName.FileName))
                {
                    image.AppUserId = signedInUserId;
                    image.DateCreated = DateTime.Now;
                    image.DateLastModified = DateTime.Now;
                    image.CreatedBy = signedInUserId;
                    image.LastModifiedBy = signedInUserId;
                    image.Status = ImageStatus.Accepted.ToString();
                    if (image.SellingPrice == null || image.SellingPrice <= 0)
                    {
                        image.SellingPrice = 0;
                    }
                    if (image.Discount == null || image.Discount <= 0)
                    {
                        image.Discount = 0;
                    }
                    if (!string.IsNullOrEmpty(collection["NewCameraText"]))
                    {
                        string camera = collection["NewCameraText"];
                        var checkCamera = collection["NewCameraText"].ToString().ToLower();
                        if (_databaseConnection.Cameras
                                .Where(n => n.Name.ToLower() == checkCamera && n.CreatedBy == signedInUserId)
                                .ToList()
                                .Count <= 0)
                            newCamera = new Camera
                            {
                                Name = camera,
                                CreatedBy = signedInUserId,
                                LastModifiedBy = signedInUserId,
                                DateCreated = DateTime.Now,
                                DateLastModified = DateTime.Now
                            };
                        else
                            newCamera = _databaseConnection.Cameras
                                .SingleOrDefault(n => n.Name.ToLower() == checkCamera &&
                                                      n.CreatedBy == signedInUserId);
                    }

                    if (!string.IsNullOrEmpty(collection["NewLocationText"]))
                    {
                        string location = collection["NewLocationText"];
                        var checkLocation = collection["NewLocationText"].ToString().ToLower();
                        if (_databaseConnection.Locations.Where(n => n.Name.ToLower() == checkLocation
                                                                     && n.CreatedBy == signedInUserId).ToList()
                                .Count <=
                            0)
                            newLocation = new Location
                            {
                                Name = location,
                                CreatedBy = signedInUserId,
                                LastModifiedBy = signedInUserId,
                                DateCreated = DateTime.Now,
                                DateLastModified = DateTime.Now
                            };
                        else
                            newLocation = _databaseConnection.Locations.SingleOrDefault(
                                n => n.Name.ToLower() == checkLocation
                                     && n.CreatedBy == signedInUserId);
                    }
                    image.Featured = false;
                    image.A1 = Convert.ToBoolean(collection["A1"]);
                    image.A2 = Convert.ToBoolean(collection["A2"]);
                    image.A3 = Convert.ToBoolean(collection["A3"]);
                    image.A4 = Convert.ToBoolean(collection["A4"]);
                    image.A5 = Convert.ToBoolean(collection["A5"]);
                    image.A6 = Convert.ToBoolean(collection["A6"]);

                    var account = new Account(
                        new AppConfig().CloudinaryAccoutnName,
                        new AppConfig().CloudinaryApiKey,
                        new AppConfig().CloudinaryApiSecret);
                    //connect to cloudinary account
                    var cloudinary = new Cloudinary(account);
                    var filename = DateTime.Now.ToFileTime().ToString();
                    //upload parameters
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(filename, FileName.OpenReadStream()),
                        Invalidate = true,
                        Format = "JPG"
                    };
                    //upload image
                    var uploadResult = cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Result.Format != null)
                    {
                        image.FilePath = uploadResult.Result.Uri.AbsolutePath;
                        image.Width = uploadResult.Result.Width;
                        image.Height = uploadResult.Result.Height;
                        image.FileName = filename;
                        if (string.IsNullOrEmpty(collection["CameraId"]) &&
                            !string.IsNullOrEmpty(collection["NewCameraText"]))
                        {
                            if (newCamera != null && newCamera.CameraId > 0)
                                image.CameraId = newCamera.CameraId;
                            if (newCamera != null && newCamera.CameraId <= 0)
                            {
                                _databaseConnection.Add(newCamera);
                                _databaseConnection.SaveChanges();
                                image.CameraId = newCamera.CameraId;
                            }
                        }
                        if (string.IsNullOrEmpty(collection["LocationId"]) &&
                            !string.IsNullOrEmpty(collection["NewLocationText"]))
                        {
                            if (newLocation != null && newLocation.LocationId > 0)
                                image.LocationId = newLocation.LocationId;
                            if (newLocation != null && newLocation.LocationId <= 0)
                            {
                                _databaseConnection.Add(newLocation);
                                _databaseConnection.SaveChanges();
                                image.LocationId = newLocation.LocationId;
                            }
                        }
                        if (signedInUserId > 0)
                        {
                            _databaseConnection.Add(image);
                            _databaseConnection.SaveChanges();

                            //get tags
                            if (!string.IsNullOrEmpty(image.Tags))
                            {
                                var values = image.Tags.Split(',');
                                for (var i = 0; i < values.Length; i++)
                                {
                                    values[i] = values[i].Trim();
                                    tags.Add(values[i]);
                                }
                                //save tags
                                foreach (var item in tags)
                                {
                                    var tag = new ImageTag
                                    {
                                        Name = item,
                                        ImageId = image.ImageId,
                                        DateCreated = DateTime.Now,
                                        DateLastModified = DateTime.Now,
                                        CreatedBy = signedInUserId,
                                        LastModifiedBy = signedInUserId
                                    };
                                    _databaseConnection.ImageTags.AddRange(tag);
                                }
                                _databaseConnection.SaveChanges();
                            }
                         
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Home");
                        }


                        //display notification
                        TempData["display"] =
                            "Your image has been succesfully uploaded!";
                        TempData["notificationtype"] = NotificationType.Success.ToString();
                        return RedirectToAction("Index");
                    }
                    //display notification
                    TempData["display"] =
                        uploadResult.Result.Error.Message;
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");
                }
                //display notification
                TempData["display"] = "Please fill all the compulsory fields and upload an Image!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(),
                    "ImageCategoryId",
                    "Name", image.ImageCategoryId);
                ViewBag.CameraId = new SelectList(
                    _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                    "Name", image.CameraId);
                ViewBag.LocationId = new SelectList(
                    _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                    "Name", image.LocationId);
                return View(image);
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] =
                    "There was an issue performing the request, check the image deatails,size and try again Later!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(),
                    "ImageCategoryId",
                    "Name", image.ImageCategoryId);
                ViewBag.CameraId = new SelectList(
                    _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                    "Name", image.CameraId);
                ViewBag.LocationId = new SelectList(
                    _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                    "Name", image.LocationId);
                return View(image);
            }
        }

        // GET: Image/Edit/5
        [SessionExpireFilter]
        public ActionResult SetAsFeatured(long id)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));

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
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            var image = _databaseConnection.Images.Find(id);
            ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(), "ImageCategoryId",
                "Name", image.ImageCategoryId);
            ViewBag.CameraId = new SelectList(
                _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                "Name", image.CameraId);
            ViewBag.LocationId = new SelectList(
                _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                "Name", image.LocationId);

            if (image.AppUserId != signedInUserId)
            {
                //display notification
                TempData["display"] = "This request is denied due to inavlid access of information!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return RedirectToAction("Index");
            }
            return View(image);
        }

        // POST: Image/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(Image image, IFormCollection collection)
        {
            var tags = new List<string>();
            var newLocation = new Location();
            var newCamera = new Camera();
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                image.DateLastModified = DateTime.Now;
                image.LastModifiedBy = signedInUserId;
                if (image.SellingPrice == null || image.SellingPrice <= 0)
                {
                    image.SellingPrice = 0;
                }
                if (image.Discount == null || image.Discount <= 0)
                {
                    image.Discount = 0;
                }
                //collect data from form as model binding is disabled
                if (!string.IsNullOrEmpty(collection["Description"]))
                    image.Description = collection["Description"];
                if (string.IsNullOrEmpty(collection["CameraId"]) &&
                    !string.IsNullOrEmpty(collection["NewCameraText"]))
                {
                    if (newCamera.CameraId > 0)
                        image.CameraId = newCamera.CameraId;
                    if (newCamera.CameraId <= 0)
                    {
                        _databaseConnection.Add(newCamera);
                        _databaseConnection.SaveChanges();
                        image.CameraId = newCamera.CameraId;
                    }
                }
                if (string.IsNullOrEmpty(collection["LocationId"]) &&
                    !string.IsNullOrEmpty(collection["NewLocationText"]))
                {
                    if (newLocation.LocationId > 0)
                        image.LocationId = newLocation.LocationId;
                    if (newLocation.LocationId <= 0)
                    {
                        _databaseConnection.Add(newLocation);
                        _databaseConnection.SaveChanges();
                        image.LocationId = newLocation.LocationId;
                    }
                }
                var allTags = _databaseConnection.ImageTags.Where(n => n.ImageId == image.ImageId);
                //get tags
                if (!string.IsNullOrEmpty(image.Tags))
                {
                    var values = image.Tags.Split(',');
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        tags.Add(values[i]);
                    }
                    //save tags
                    foreach (var item in tags)
                    {
                        if (allTags.Where(n => n.Name == item).ToList().Count <= 0)
                        {
                            var tag = new ImageTag
                            {
                                Name = item,
                                ImageId = image.ImageId,
                                DateCreated = DateTime.Now,
                                DateLastModified = DateTime.Now,
                                CreatedBy = signedInUserId,
                                LastModifiedBy = signedInUserId
                            };
                            _databaseConnection.ImageTags.AddRange(tag);
                        }
                    }
                    _databaseConnection.SaveChanges();
                }
                _databaseConnection.Entry(image).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the image details!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = "There was an issue performing the request, try again Later!";
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
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
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
            var imageFile = image.FilePath.Replace("/cloudmab/image/upload/v1517001994/", "");


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

        [SessionExpireFilter]
        public ActionResult RejectImage(long id)
        {
            var image = new Image();
            image.ImageId = id;
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectImage(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["ImageId"]);
            var image = _databaseConnection.Images.Find(id);
            //customize user object to sent email to user
            var appUser = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl)
                .Result.Single(n => n.AppUserId == image.AppUserId);
            appUser.Biography = collection["Reason"];
            appUser.DateCreated = image.DateCreated;
            appUser.Address = image.Title;

            //upload image via Cloudinary API Call
            var account = new Account(
                new AppConfig().CloudinaryAccoutnName,
                new AppConfig().CloudinaryApiKey,
                new AppConfig().CloudinaryApiSecret);

            var cloudinary = new Cloudinary(account);
            var delParams = new DelResParams
            {
                PublicIds = new List<string> {image.FileName},
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
            TempData["display"] = "You have successfully removed the image from the platform!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }
        public IActionResult Downloads()
        {
            var downloads = new ImageFactory().GetAllDownloads(new AppConfig().GetImageDownloadsUrl);
            ViewBag.Images = _databaseConnection.Images.ToList();
            return View(downloads.Result.ToList());
        }
    }
}