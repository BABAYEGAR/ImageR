using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CamerackStudio.Models;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class ImageCategoryController : Controller
    {
 
        private readonly CamerackStudioDataContext _databaseConnection;
        AppUser _appUser;
        List<Image> _images = new List<Image>();

        public ImageCategoryController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View(_databaseConnection.ImageCategories.ToList());
        }
        // GET: Image
        [SessionExpireFilter]
        public ActionResult Images(long id)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            if (HttpContext.Session.GetString("StudioLoggedInUser") != null)
            {
                var userString = HttpContext.Session.GetString("StudioLoggedInUser");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            }
            if (_appUser.Role.ManageImages)
            {
                _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                    .Where(n => n.AppUserId == signedInUserId && n.ImageCategoryId == id).ToList();


            }
            if (_appUser.Role.UploadImage)
            {

                _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageCategoryId).Where(n => n.CameraId == id).ToList();


            }
            return View(_images);
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
        public ActionResult Create(ImageCategory imageCategory, IList<IFormFile> image, IFormCollection collection)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                imageCategory.DateCreated = DateTime.Now;
                imageCategory.DateLastModified = DateTime.Now;
                imageCategory.CreatedBy = signedInUserId;
                imageCategory.LastModifiedBy = signedInUserId;
                if (image.Count > 0)
                    foreach (var file in image)
                    {
                        var fileInfo = new FileInfo(file.FileName);
                        var ext = fileInfo.Extension.ToLower();
                        var name = DateTime.Now.ToFileTime().ToString();
                        var fileName = name + ext;
                        //var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\ImageCategory\{fileName}";
                        var uploadedImage = new AppConfig().ImageCategoryPicture + fileName;
                        using (var fs = System.IO.File.Create(uploadedImage))
                        {
                            if (fs != null)
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                                imageCategory.FileName = fileName;

                            }
                        }
                    }
                _databaseConnection.ImageCategories.Add(imageCategory);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Image Category!";
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
            return View(_databaseConnection.ImageCategories.Find(id));
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(ImageCategory imageCategory, IList<IFormFile> image, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                imageCategory.DateLastModified = DateTime.Now;
                imageCategory.LastModifiedBy = signedInUserId;
                if (image.Count > 0)
                    foreach (var file in image)
                    {
                        var fileInfo = new FileInfo(file.FileName);
                        var ext = fileInfo.Extension.ToLower();
                        var name = DateTime.Now.ToFileTime().ToString();
                        var fileName = name + ext;
                        //var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\ImageCategory\{fileName}";
                        var uploadedImage = new AppConfig().ImageCategoryPicture + fileName;
                        using (var fs = System.IO.File.Create(uploadedImage))
                        {
                            if (fs != null)
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                                imageCategory.FileName = fileName;

                            }
                        }
                    }
                _databaseConnection.Entry(imageCategory).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Image Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                return View();
            }
        }

        // GET: ImageCategory/Delete/5
        [SessionExpireFilter]
        public ActionResult Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["CategoryId"]);
            var imageCategory = _databaseConnection.ImageCategories.Find(id);

            _databaseConnection.ImageCategories.Remove(imageCategory);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Image Category!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}