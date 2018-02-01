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
    public class AdvertisementController : Controller
    {
 
        private readonly CamerackStudioDataContext _databaseConnection;
        public AdvertisementController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        // GET: ImageCategory
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View(_databaseConnection.Advertisements.ToList());
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
        public ActionResult Create(Advertisement advertisement, IList<IFormFile> image, IFormCollection collection)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                advertisement.DateCreated = DateTime.Now;
                advertisement.DateLastModified = DateTime.Now;
                advertisement.CreatedBy = signedInUserId;
                advertisement.LastModifiedBy = signedInUserId;
                if (_databaseConnection.Advertisements
                        .Where(n => n.EndDate > advertisement.StartDate && n.PageCategory == advertisement.PageCategory)
                        .ToList().Count <= 0)
                {
                    if (image.Count > 0)
                        foreach (var file in image)
                        {
                            var fileInfo = new FileInfo(file.FileName);
                            var ext = fileInfo.Extension.ToLower();
                            var name = DateTime.Now.ToFileTime().ToString();
                            var fileName = name + ext;
                            var uploadedImage = new AppConfig().AdvertPicture + fileName;
                            using (var fs = System.IO.File.Create(uploadedImage))
                            {
                                if (fs != null)
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                    advertisement.File = fileName;

                                }
                            }
                        }
                    _databaseConnection.Advertisements.Add(advertisement);
                    _databaseConnection.SaveChanges();

                    //display notification
                    TempData["display"] = "You have successfully added a new Advertisement to Camerack!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");
                }

                //display notification
                TempData["display"] = "The advert cannot be scheduled for the category at the set time" +
                                      ", check the category and time constraints!";
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
            return View(_databaseConnection.Advertisements.Find(id));
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(Advertisement advertisement, IList<IFormFile> image, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                advertisement.DateLastModified = DateTime.Now;
                advertisement.LastModifiedBy = signedInUserId;
                if (_databaseConnection.Advertisements
                        .Where(n => n.EndDate > advertisement.StartDate && n.PageCategory == advertisement.PageCategory 
                        && n.AdvertisementId != advertisement.AdvertisementId)
                        .ToList().Count <= 0)
                {
                    if (image.Count > 0)
                        foreach (var file in image)
                        {
                            var fileInfo = new FileInfo(file.FileName);
                            var ext = fileInfo.Extension.ToLower();
                            var name = DateTime.Now.ToFileTime().ToString();
                            var fileName = name + ext;
                            var uploadedImage = new AppConfig().AdvertPicture + fileName;
                            using (var fs = System.IO.File.Create(uploadedImage))
                            {
                                if (fs != null)
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                    advertisement.File = fileName;

                                }
                            }
                        }
                    _databaseConnection.Entry(advertisement).State = EntityState.Modified;
                    _databaseConnection.SaveChanges();

                    //display notification
                    TempData["display"] = "You have successfully modified the Advertisement!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");
                }

                //display notification
                TempData["display"] = "The advert cannot be scheduled for the category at the set time" +
                                      ", check the category and time constraints!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
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
            var id = Convert.ToInt64(collection["AdvertisementId"]);
            var advertisement = _databaseConnection.Advertisements.Find(id);

            _databaseConnection.Advertisements.Remove(advertisement);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Advertisement!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

    }
}