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

namespace Image.Controllers
{
    public class PhotographerCategoryController : Controller
    {
        private readonly ImageDataContext _databaseConnection;
        private readonly IHostingEnvironment _hostingEnv;

        public PhotographerCategoryController(IHostingEnvironment env, ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _hostingEnv = env;
        }

        // GET: ImageCategory
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View(_databaseConnection.PhotographerCategories.ToList());
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
        public ActionResult Create(PhotographerCategory photographerCategory, IList<IFormFile> image,
            IFormCollection collection)
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                photographerCategory.DateCreated = DateTime.Now;
                photographerCategory.DateLastModified = DateTime.Now;
                photographerCategory.CreatedBy = signedInUserId;
                photographerCategory.LastModifiedBy = signedInUserId;
                if (image.Count > 0)
                    foreach (var file in image)
                    {
                        var fileInfo = new FileInfo(file.FileName);
                        var ext = fileInfo.Extension.ToLower();
                        var name = DateTime.Now.ToFileTime().ToString();
                        var fileName = name + ext;
                        var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\PhotoCategory\{fileName}";

                        using (var fs = System.IO.File.Create(uploadedImage))
                        {
                            if (fs != null)
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                                photographerCategory.FileName = fileName;
                            }
                        }
                    }
                _databaseConnection.PhotographerCategories.Add(photographerCategory);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new Photographer Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public void MapAndUnmapPhotographyCategory(long id)
        {
            var allMappings = _databaseConnection.PhotographerCategoryMappings.ToList();
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("userId"));
            var userMapping =
                allMappings.SingleOrDefault(n => n.AppUserId == signedInUserId && n.PhotographerCategoryId == id);
            if (userMapping != null)
            {
                _databaseConnection.PhotographerCategoryMappings.Remove(userMapping);
                _databaseConnection.SaveChanges();
            }
            else
            {
                var categoryMapping = new PhotographerCategoryMapping
                {
                    PhotographerCategoryId = id,
                    AppUserId = signedInUserId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    LastModifiedBy = signedInUserId,
                    CreatedBy = signedInUserId
                };
                _databaseConnection.PhotographerCategoryMappings.Add(categoryMapping);
                _databaseConnection.SaveChanges();
            }
            //var categories = _databaseConnection.PhotographerCategories.ToList();
            //ViewBag.Mapping = _databaseConnection.PhotographerCategoryMappings.ToList();
            //return PartialView("Partials/_PartialPhotoCategory",categories);
        }

        public ActionResult SelectCategories()
        {
            ViewBag.Mapping = _databaseConnection.PhotographerCategoryMappings.ToList();
            return View(_databaseConnection.PhotographerCategories.OrderByDescending(n => n.Name).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapPhotographyCategory(int[] table_records, IFormCollection collection)
        {
            var allMappings = _databaseConnection.PhotographerCategoryMappings.ToList();
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("userId"));
            if (table_records != null)
            {
                var length = table_records.Length;
                for (var i = 0; i < length; i++)
                {
                    var id = table_records[i];
                    var singleMapping = allMappings.SingleOrDefault(
                        n =>
                            n.PhotographerCategoryId == id && n.AppUserId == signedInUserId);
                    if (singleMapping != null)
                    {
                    }
                    else
                    {
                        var categoryMapping = new PhotographerCategoryMapping
                        {
                            PhotographerCategoryId = id,
                            AppUserId = signedInUserId,
                            DateCreated = DateTime.Now,
                            DateLastModified = DateTime.Now,
                            LastModifiedBy = signedInUserId,
                            CreatedBy = signedInUserId
                        };
                        _databaseConnection.PhotographerCategoryMappings.Add(categoryMapping);
                        _databaseConnection.SaveChanges();
                    }
                }
                TempData["display"] = "you have succesfully added the category(s) to the profile!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
            }
            else
            {
                TempData["display"] = "no category has been selected!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return RedirectToAction("SelectCategories", "PhotographerCategory");
            }
            return RedirectToAction("SelectCategories", "PhotographerCategory");
        }

        // GET: ImageCategory/Edit/5
        [SessionExpireFilter]
        public ActionResult Edit(long id)
        {
            return View(_databaseConnection.PhotographerCategories.Find(id));
        }

        // POST: ImageCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult Edit(PhotographerCategory photographerCategory, IList<IFormFile> image,
            IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                photographerCategory.DateLastModified = DateTime.Now;
                photographerCategory.LastModifiedBy = signedInUserId;
                if (image.Count > 0)
                    foreach (var file in image)
                    {
                        var fileInfo = new FileInfo(file.FileName);
                        var ext = fileInfo.Extension.ToLower();
                        var name = DateTime.Now.ToFileTime().ToString();
                        var fileName = name + ext;
                        var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\PhotoCategory\{fileName}";

                        using (var fs = System.IO.File.Create(uploadedImage))
                        {
                            if (fs != null)
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                                photographerCategory.FileName = fileName;
                            }
                        }
                    }
                _databaseConnection.Entry(photographerCategory).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the Image Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [SessionExpireFilter]
        public ActionResult RemovePhographerCategoryMapping(IFormCollection collection)
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                var categoryId = Convert.ToInt64(collection["PhtographerCategoryId"]);
                var mapping =
                    _databaseConnection.PhotographerCategoryMappings.SingleOrDefault(
                        n => n.PhotographerCategoryId == categoryId && n.AppUserId == signedInUserId);

                _databaseConnection.PhotographerCategoryMappings.Remove(mapping);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully removed the Category!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("SelectCategories");
            }
            catch
            {
                return RedirectToAction("SelectCategories");
            }
        }

        // GET: ImageCategory/Delete/5
        [SessionExpireFilter]
        public ActionResult Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["CategoryId"]);
            var photographerCategory = _databaseConnection.PhotographerCategories.Find(id);

            _databaseConnection.PhotographerCategories.Remove(photographerCategory);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the Image Category!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }
    }
}