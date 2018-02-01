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
    public class HeaderImageController : Controller
    {
 
        private readonly CamerackStudioDataContext _databaseConnection;
        public HeaderImageController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View(_databaseConnection.HeaderImages.ToList());
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
        public ActionResult Create(HeaderImage headerImage, IList<IFormFile> image, IFormCollection collection)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                headerImage.DateCreated = DateTime.Now;
                headerImage.DateLastModified = DateTime.Now;
                headerImage.CreatedBy = signedInUserId;
                headerImage.LastModifiedBy = signedInUserId;
                if (_databaseConnection.HeaderImages
                        .Where(n => n.PageCategory == headerImage.PageCategory)
                        .ToList().Count <= 0)
                {
                    if (image.Count > 0)
                        foreach (var file in image)
                        {
                            var fileInfo = new FileInfo(file.FileName);
                            var ext = fileInfo.Extension.ToLower();
                            var name = DateTime.Now.ToFileTime().ToString();
                            var fileName = name + ext;
                            var uploadedImage = new AppConfig().HeaderPicture + fileName;
                            using (var fs = System.IO.File.Create(uploadedImage))
                            {
                                if (fs != null)
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                    headerImage.File = fileName;

                                }
                            }
                        }
                    _databaseConnection.HeaderImages.Add(headerImage);
                    _databaseConnection.SaveChanges();

                    //display notification
                    TempData["display"] = "You have successfully added a new Header Image!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");
                }
                //display notification
                TempData["display"] = "The Header Image for the category Cannot be duplicated as it already exist!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
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
            return View(_databaseConnection.HeaderImages.Find(id));
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(HeaderImage headerImage, IList<IFormFile> image, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                headerImage.DateLastModified = DateTime.Now;
                headerImage.LastModifiedBy = signedInUserId;
                if (image.Count > 0)
                    foreach (var file in image)
                    {
                        var fileInfo = new FileInfo(file.FileName);
                        var ext = fileInfo.Extension.ToLower();
                        var name = DateTime.Now.ToFileTime().ToString();
                        var fileName = name + ext;
                        var uploadedImage = new AppConfig().HeaderPicture + fileName;
                        using (var fs = System.IO.File.Create(uploadedImage))
                        {
                            if (fs != null)
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                                headerImage.File = fileName;

                            }
                        }
                    }
                _databaseConnection.Entry(headerImage).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Header Image!";
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
            var id = Convert.ToInt64(collection["HeaderImageId"]);
            var headerImage = _databaseConnection.HeaderImages.Find(id);

            _databaseConnection.HeaderImages.Remove(headerImage);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Header Image!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}