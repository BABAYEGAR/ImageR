using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Image.Data;
using Image.Models;
using Image.Models.APIFactory;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageDataContext _databaseConnection;
        private List<Models.Entities.Image> _images = new List<Models.Entities.Image>();
        private Role _userRole;
        private List<AppUser> users;

        public ImageController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            users = new AppUserFactory().GetAllUsersAsync(new AppConfig().FetchUsersUrl).Result;
        }

        // GET: Image
        [SessionExpireFilter]
        public ActionResult Index(string status)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            if (HttpContext.Session.GetString("Role") != null)
            {
                var roleString = HttpContext.Session.GetString("Role");
                _userRole = JsonConvert.DeserializeObject<Role>(roleString);
            }
            if (_userRole.UploadImage)
                if (status != null)
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                        .Where(n => n.AppUserId == signedInUserId).Where(n => n.Status == status).ToList();
                else
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                        .Where(n => n.AppUserId == signedInUserId).ToList();
            if (_userRole.ManageImages)
                if (status != null)
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(n => n.Status == status)
                        .ToList();
                else
                    _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                        .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).ToList();
            ViewBag.status = status;
            ViewBag.Role = _userRole;
            return View(_images);
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
        public async Task<PartialViewResult> RateImage(IFormCollection collection, ImageAction action)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var result = _images;
            action.Action = "Rate";
            action.ActionDate = DateTime.Now;
            action.AppUserId = signedInUserId;
            action.OwnerId = signedInUserId;
            action.ImageId = Convert.ToInt64(collection["ImageId"]);
            action.Rating = Convert.ToInt64(collection["rating"]);
            action.TenancyId = 1;
            _databaseConnection.Add(action);
            _databaseConnection.SaveChanges();
            var image = _images.SingleOrDefault(n => n.ImageId == action.ImageId);
            return PartialView("Partials/_PartialRating", image);
        }

        // GET: Image/Details/5
        public async Task<ActionResult> ApproveOrRejectImage(long id, string status)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var image = _databaseConnection.Images.Find(id);
            if (status == ImageStatus.Accepted.ToString())
                image.Status = ImageStatus.Accepted.ToString();
            if (status == ImageStatus.Rejected.ToString())
            {
                image.Status = ImageStatus.Rejected.ToString();
                string imageFile = "" + image.FileName;
                //upload image via Cloudinary API Call
                var account = new Account(
                    new Config().CloudinaryAccoutnName,
                    new Config().CloudinaryApiKey,
                    new Config().CloudinaryApiSecret);
                var imageList = new List<string> {imageFile};
                var cloudinary = new Cloudinary(account);
                var delParams = new DelResParams
                {
                    PublicIds = imageList,
                    Invalidate = true
                };
                await cloudinary.DeleteResourcesAsync(delParams);
            }
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
        public ActionResult Create()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(), "ImageCategoryId",
                "Name");
            ViewBag.CameraId = new SelectList(
                _databaseConnection.Cameras.Where(n => n.CreatedBy == signedInUserId).ToList(), "CameraId",
                "Name");
            ViewBag.LocationId = new SelectList(
                _databaseConnection.Locations.Where(n => n.CreatedBy == signedInUserId).ToList(), "LocationId",
                "Name");
            return View();
        }

        // POST: Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public async Task<ActionResult> Create(Models.Entities.Image image, IFormCollection collection, IFormFile file)
        {
            try
            {
                if (file != null && file.Length < 15728640)
                {
                    // TODO: Add insert logic here
                    var tags = new List<string>();
                    var signedInUserId = HttpContext.Session.GetInt32("userId");
                    image.AppUserId = signedInUserId;
                    image.DateCreated = DateTime.Now;
                    image.DateLastModified = DateTime.Now;
                    image.CreatedBy = signedInUserId;
                    image.LastModifiedBy = signedInUserId;
                    image.FileName = DateTime.Now.ToFileTime().ToString();
                    image.Status = ImageStatus.Pending.ToString();


                    //upload image via Cloudinary API Call
                    var account = new Account(
                        new Config().CloudinaryAccoutnName,
                        new Config().CloudinaryApiKey,
                        new Config().CloudinaryApiSecret);

                    var cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, file.OpenReadStream()),
                        Invalidate = true
                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Format != null)
                    {
                        image.FilePath = uploadResult.Uri.AbsolutePath;
                        _databaseConnection.Images.Add(image);
                        _databaseConnection.SaveChanges();
                    }
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


                    //display notification
                    TempData["display"] = "You have successfully uploaded a new image!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");
                }
                    //display notification
                    TempData["display"] =
                        "No Image was Selected or selected image violate the Upload Size Rule of 15MB!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(),
                        "ImageCategoryId",
                        "Name", image.ImageCategoryId);
                    ViewBag.CameraId = new SelectList(_databaseConnection.Cameras.ToList(), "CameraId",
                        "Name", image.CameraId);
                    ViewBag.LocationId = new SelectList(_databaseConnection.Locations.ToList(), "LocationId",
                        "Name", image.LocationId);
                    return View(image);
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = "There was an issue uploading the image, Try Again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(),
                    "ImageCategoryId",
                    "Name", image.ImageCategoryId);
                ViewBag.CameraId = new SelectList(_databaseConnection.Cameras.ToList(), "CameraId",
                    "Name", image.CameraId);
                ViewBag.LocationId = new SelectList(_databaseConnection.Locations.ToList(), "LocationId",
                    "Name", image.LocationId);
                return View(image);
            }
        }


        // GET: Image/Edit/5
        [SessionExpireFilter]
        public ActionResult Edit(long id)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
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
        public ActionResult Edit(Models.Entities.Image image, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var tags = new List<string>();
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                image.DateLastModified = DateTime.Now;
                image.LastModifiedBy = signedInUserId;
                _databaseConnection.Entry(image).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //get tags
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
                        var allTags = _databaseConnection.ImageTags.Where(n => n.ImageId == image.ImageId);
                        if (allTags.Any(n => n.Name == item))
                        {
                        }
                        else
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


                //display notification
                TempData["display"] = "You have successfully modified the image information!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                //display notification
                TempData["display"] = "There was an issue modifying the image information, Try Again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(image);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult PostImageComment(ImageComment comment, IFormCollection collection)
        {
            long? imageId = Convert.ToInt64(collection["ImageId"]);
            var comments = _databaseConnection.ImageComments.Where(n => n.ImageId == imageId).ToList();
            string[] words = {"fuck", "Fuck", "4kin", "idiot", "pussy", "dick", "blowjob", "bastard", "stupid"};
            var userComment = collection["Comment"].ToString().ToLower();
            foreach (var item in words)
                if (userComment.Contains(item))
                {
                    TempData["display"] =
                        "This platform does not support the use of vulgar language,Please check your comment again!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return PartialView("Partials/_ImageComment", comments);
                }
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            comment.DateCreated = DateTime.Now;
            comment.AppUserId = signedInUserId;
            comment.ImageId = imageId;
            comment.Comment = collection["Comment"].ToString();
            comment.CreatedBy = signedInUserId;
            comment.LastModifiedBy = signedInUserId;
            comment.DateLastModified = DateTime.Now;
            _databaseConnection.ImageComments.Add(comment);
            _databaseConnection.SaveChanges();
            var newComments = _databaseConnection.ImageComments.Where(n => n.ImageId == imageId).ToList();
            ViewBag.Users = users;
            return PartialView("Partials/_ImageComment", newComments);
        }
      
        [HttpGet]
        public PartialViewResult ReloadImageComments(long? id)
        {
            ViewBag.Users = users;
            var imageComments = _databaseConnection.ImageComments.Where(n => n.ImageId == id).ToList();
            return PartialView("Partials/_ImageComment",imageComments);
        }
        // POST: Image/Delete/5
        [SessionExpireFilter]
        public async Task<ActionResult> Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["ImageId"]);
            var image = _databaseConnection.Images.Find(id);
            var imageFile = "" + image.FileName;

            _databaseConnection.Images.Remove(image);
            _databaseConnection.SaveChanges();

            //upload image via Cloudinary API Call
            var account = new Account(
                new Config().CloudinaryAccoutnName,
                new Config().CloudinaryApiKey,
                new Config().CloudinaryApiSecret);

            var cloudinary = new Cloudinary(account);
            var delParams = new DelResParams
            {
                PublicIds = new List<string> {imageFile},
                Invalidate = true
            };
           await cloudinary.DeleteResourcesAsync(delParams);

            //display notification
            TempData["display"] = "You have successfully deleted the Image from your Library!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }
    }
}