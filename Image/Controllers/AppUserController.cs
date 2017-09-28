using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Image.Controllers
{
    public class AppUserController : Controller
    {
        private readonly ImageDataContext _databaseConnection;
        private readonly IHostingEnvironment _hostingEnv;

        public AppUserController(IHostingEnvironment env, ImageDataContext databaseConnection)
        {
            _hostingEnv = env;
            _databaseConnection = databaseConnection;
        }
        // GET: AppUser
        public ActionResult Index()
        {
            return View(_databaseConnection.AppUsers.ToList());
        }

        // GET: AppUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppUser user, IList<IFormFile> images, IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                user.DateCreated = DateTime.Now;
                user.DateLastModified = DateTime.Now;
                user.CreatedBy = signedInUserId;
                user.LastModifiedBy = signedInUserId;

                if (images.Count > 0)
                {
                    {
                        foreach (var file in images)
                        {
                            var fileInfo = new FileInfo(file.FileName);
                            var ext = fileInfo.Extension.ToLower();
                            var name = DateTime.Now.ToFileTime().ToString();
                            var fileName = name + ext;
                            var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedFiles\ProfilePicture\{fileName}";

                            using (var fs = System.IO.File.Create(uploadedImage))
                            {
                                if (fs != null)
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();

                                }
                            }
                        }
                    }
                }

                _databaseConnection.Add(user);
                _databaseConnection.SaveChanges();
           
                //display notification
                TempData["display"] = "You have successfully added a new user!";
                TempData["notificationtype"] = NotificationType.Success.ToString();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: AppUser/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: AppUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AppUser user, IList<IFormFile> images, IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                user.DateCreated = DateTime.Now;
                user.DateLastModified = DateTime.Now;
                user.CreatedBy = signedInUserId;
                user.LastModifiedBy = signedInUserId;

                if (images.Count > 0)
                {
                    {
                        foreach (var file in images)
                        {
                            var fileInfo = new FileInfo(file.FileName);
                            var ext = fileInfo.Extension.ToLower();
                            var name = DateTime.Now.ToFileTime().ToString();
                            var fileName = name + ext;
                            var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedFiles\ProfilePicture\{fileName}";

                            using (var fs = System.IO.File.Create(uploadedImage))
                            {
                                if (fs != null)
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();

                                }
                            }
                        }
                    }
                }

                _databaseConnection.Add(user);
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully added a new user!";
                TempData["notificationtype"] = NotificationType.Success.ToString();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: AppUser/Edit/5
        public ActionResult Edit(long id)
        {
            return View();
        }

        // POST: AppUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppUser user, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                user.DateLastModified = DateTime.Now;
                user.LastModifiedBy = signedInUserId;

                _databaseConnection.Entry(user).State = EntityState.Modified; ;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have successfully modified the user!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AppUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}