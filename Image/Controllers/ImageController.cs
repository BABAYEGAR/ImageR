using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Image.Data;
using Image.Models.DataBaseConnections;
using Image.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;

namespace Image.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public ImageController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: Image
        public ActionResult Index()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            return View(_databaseConnection.Images.Include(n=>n.Camera).Include(n=>n.Location).Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(n=>n.AppUserId ==signedInUserId ).ToList());
        }
        /// <summary>
        ///     Sends Json responds object to view with sub categories of the categories requested via an Ajax call
        /// </summary>
        /// <param name="id"> state id</param>
        /// <returns>lgas</returns>
        /// Microsoft.CodeDom.Providers.DotNetCompilerPlatform
        
        public JsonResult GetSubForCategories(long id)
        {
            var subs = _databaseConnection.ImageSubCategories.Where(n => n.ImageCategoryId == id).ToList();
            return Json(subs);
        }
        // GET: Image/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Image/Create
        public ActionResult Create()
        {
            ViewBag.ImageCategoryId = new SelectList(_databaseConnection.ImageCategories.ToList(), "ImageCategoryId",
                "Name");
            ViewBag.CameraId = new SelectList(_databaseConnection.Cameras.ToList(), "CameraId",
                "Name");
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations.ToList(), "LocationId",
                "Name");
            return View();
        }

        // POST: Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Entities.Image image, IFormCollection collection, IFormFile file)
        {
            try
            {
                if (file != null && file.Length < 15728640)
                {
                    // TODO: Add insert logic here
                    var signedInUserId = HttpContext.Session.GetInt32("userId");
                    image.AppUserId = signedInUserId;
                    image.DateCreated = DateTime.Now;
                    image.DateLastModified = DateTime.Now;
                    image.CreatedBy = signedInUserId;
                    image.LastModifiedBy = signedInUserId;
                    image.FileName = DateTime.Now.ToFileTime().ToString();


                        //upload image via Cloudinary API Call
                        Account account = new Account(
                            new Config().CloudinaryAccoutnName,
                            new Config().CloudinaryApiKey,
                            new Config().CloudinaryApiSecret);

                        Cloudinary cloudinary = new Cloudinary(account);
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(image.FileName, file.OpenReadStream())
                        };
                        var uploadResult = cloudinary.Upload(uploadParams);
                     
                    if (uploadResult.Format != null)
                    {
                        image.FilePath = uploadResult.Uri.AbsolutePath;
                        _databaseConnection.Images.Add(image);
                        _databaseConnection.SaveChanges();
                    }

                    //display notification
                    TempData["display"] = "You have successfully uploaded a new image!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");

                }
                {
                    //display notification
                    TempData["display"] = "No Image was Selected or selected image violate the Upload Size Rule of 15MB!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(image);

                }
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = "There was an issue uploading the image, Try Again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(image);
            }
        }


        // GET: Image/Edit/5
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.Images.Find(id));
        }

        // POST: Image/Edit/5
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public ActionResult Edit(Models.Entities.Image image, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                image.DateLastModified = DateTime.Now;
                image.LastModifiedBy = signedInUserId;
                _databaseConnection.Entry(image).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
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
        // POST: Image/Delete/5
        public ActionResult Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["ImageId"]);
            var image = _databaseConnection.Images.Find(id);
            var imageFile = "" + image.FileName;

            _databaseConnection.Images.Remove(image);
            _databaseConnection.SaveChanges();

            //upload image via Cloudinary API Call
            Account account = new Account(
                new Config().CloudinaryAccoutnName,
                new Config().CloudinaryApiKey,
                new Config().CloudinaryApiSecret);

            Cloudinary cloudinary = new Cloudinary(account);
            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { imageFile },
                Invalidate = true
            };
            cloudinary.DeleteResources(delParams);

            //display notification
            TempData["display"] = "You have successfully deleted the Image from your Library!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }
    }
}