using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class ImageCategoryController : Controller
    {
 
        private readonly ImageDataContext _databaseConnection;
        Role _userRole;
        List<Models.Entities.Image> _images = new List<Models.Entities.Image>();
        private readonly IHostingEnvironment _hostingEnv;

        public ImageCategoryController(IHostingEnvironment env, ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _hostingEnv = env;
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
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            if (HttpContext.Session.GetString("Role") != null)
            {
                var roleString = HttpContext.Session.GetString("Role");
                _userRole = JsonConvert.DeserializeObject<Role>(roleString);
            }
            if (_userRole.ManageImages)
            {
                _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                    .Where(n => n.AppUserId == signedInUserId && n.ImageCategoryId == id).ToList();


            }
            if (_userRole.UploadImage)
            {

                _images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageCategoryId).Where(n => n.CameraId == id).ToList();


            }
            ViewBag.Role = _userRole;
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
                var signedInUserId = HttpContext.Session.GetInt32("userId");
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
                        var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\ImageCategory\{fileName}";

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
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                imageCategory.DateLastModified = DateTime.Now;
                imageCategory.LastModifiedBy = signedInUserId;
                if (image.Count > 0)
                    foreach (var file in image)
                    {
                        var fileInfo = new FileInfo(file.FileName);
                        var ext = fileInfo.Extension.ToLower();
                        var name = DateTime.Now.ToFileTime().ToString();
                        var fileName = name + ext;
                        var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\ImageCategory\{fileName}";

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