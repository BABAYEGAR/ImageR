using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Image.Models.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View(_databaseConnection.AppUsers.Include(n=>n.Role).ToList());
        }

        // GET: AppUser/Details/5
        [SessionExpireFilter]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppUser/Create
        //[SessionExpireFilter]
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(_databaseConnection.Roles.ToList(), "RoleId",
                "Name");
            return View();
        }

        // POST: AppUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[SessionExpireFilter]
        public ActionResult Create(AppUser user, IList<IFormFile> images, IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("userId"));
                user.DateCreated = DateTime.Now;
                user.Password = new Hashing().HashPassword(user.ConfirmPassword);
                user.ConfirmPassword = user.Password;
                user.DateLastModified = DateTime.Now;
                user.CreatedBy = signedInUserId;
                user.LastModifiedBy = signedInUserId;
                user.Status = UserStatus.Inactive.ToString();

                if (_databaseConnection.AppUsers.Any(n => n.Email == user.Email || n.Username == user.Username))
                {
                    //display notification
                    TempData["display"] = "A user with the same Username/Email already exist, try another credential again!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(user);
                }
                _databaseConnection.AppUsers.Add(user);
                _databaseConnection.SaveChanges();

                //define acceskeys
                var accessKey = new AppUserAccessKey
                {
                    AppUserId = user.AppUserId,
                    PasswordAccessCode = new Md5Ecryption().RandomString(15),
                    AccountActivationAccessCode = new Md5Ecryption().RandomString(20),
                    CreatedBy = signedInUserId,
                    LastModifiedBy = signedInUserId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddDays(1)
                };

                _databaseConnection.AccessKeys.Add(accessKey);
                _databaseConnection.SaveChanges();

                //var userSubscription = new UserSubscription
                //{
                //    AppUserId = user.AppUserId,
                //    DateCreated = DateTime.Now,
                //    DateLastModified = DateTime.Now,
                //    CreatedBy = signedInUserId,
                //    LastModifiedBy = signedInUserId,
                //    PackageId = 1,
                //    Status = UserStatus.Active.ToString()
                //};

                //_databaseConnection.UserSubscriptions.Add(userSubscription);
                //_databaseConnection.SaveChanges();
                var role = _databaseConnection.Roles.Find(user.RoleId);

                var link = _hostingEnv.WebRootPath;
                var mail = new Mailer();
                mail.SendNewUserEmail(link + "\\EmailTemplates\\NewUser.html", user, role, accessKey);
                //display notification
                TempData["display"] = "You have successfully registered to SOS Photo Studio, The user should check email to confirm your account!";
                TempData["notificationtype"] = NotificationType.Success.ToString();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                //display notification
                TempData["display"] = "Registration of new user is incomplete,Try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                ViewBag.RoleId = new SelectList(_databaseConnection.Roles.ToList(), "RoleId",
                    "Name");
                return View(user);
            }
        }

        // GET: AppUser/Edit/5
        [SessionExpireFilter]
        public ActionResult Edit(long id)
        {
            return View();
        }

        // POST: AppUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
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
        // POST: AppUser/Delete/5
        [SessionExpireFilter]
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