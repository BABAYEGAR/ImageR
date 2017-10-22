using System;
using System.Collections.Generic;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Image.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class AccountController : Controller
    {
        private readonly ImageDataContext _databaseConnection;
        private readonly IHostingEnvironment _hostingEnv;

        public AccountController(IHostingEnvironment env, ImageDataContext databaseConnection)
        {
            _hostingEnv = env;
            _databaseConnection = databaseConnection;
        }

        [SessionExpireFilter]
        public ActionResult Profile()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            ViewBag.Images = _databaseConnection.Images.Include(n => n.AppUser).Include(n => n.ImageCategory)
                .Include(n => n.ImageComments).Include(n => n.ImageTags).Include(n => n.ImageRatings)
                .Include(n => n.ImageClicks).Include(n => n.Location).Include(n => n.ImageSubCategory)
                .Where(n => n.CreatedBy == signedInUserId).ToList();
            return View();
        }

        [SessionExpireFilter]
        public ActionResult ManageProfile()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var user = _databaseConnection.AppUsers.Find(signedInUserId);
            ViewBag.PhotographerCategoryId = new SelectList(_databaseConnection.PhotographerCategories.ToList(),
                "PhotographerCategoryId",
                "Name");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult ManageProfile(AppUser appUser)
        {
            try
            {
                //populate object and save transaction
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                appUser.LastModifiedBy = signedInUserId;
                appUser.DateLastModified = DateTime.Now;
                _databaseConnection.Entry(appUser).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have succesfully updated your profile, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = "You are unable to update your profile, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(appUser);
            }
        }

        [SessionExpireFilter]
        public ActionResult ChangePassword()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var user = _databaseConnection.AppUsers.Find(signedInUserId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult ChangePassword(AppUser appUser)
        {
            try
            {
                //populate object and save transaction
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                appUser.Password = new Hashing().HashPassword(appUser.Password);
                appUser.Password = appUser.Password;
                appUser.LastModifiedBy = signedInUserId;
                appUser.DateLastModified = DateTime.Now;
                _databaseConnection.Entry(appUser).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have succesfully changed your password!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = "You are unable to update your profile, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(appUser);
            }
        }

        [SessionExpireFilter]
        public ActionResult AccountActivationLink(string accessCode)
        {
            var accessKey =
                _databaseConnection.AccessKeys.SingleOrDefault(n => n.AccountActivationAccessCode == accessCode);
            var user = _databaseConnection.AppUsers.Find(accessKey.AppUserId);
            if (user.Status == UserStatus.Inactive.ToString())
            {
                //update user
                user.Status = UserStatus.Active.ToString();
                _databaseConnection.Entry(user).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                var role = _databaseConnection.Roles.Find(user.RoleId);
                //convert object to json string and insert into session
                var userString = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("ImageLoggedInUser", userString);

                //convert object to json string and insert into session
                var roleString = JsonConvert.SerializeObject(role);
                HttpContext.Session.SetString("Role", roleString);

                //set user id inti=o session string
                HttpContext.Session.SetInt32("userId", (int) user.AppUserId);


                //update accessKeys
                accessKey.AccountActivationAccessCode = new Md5Ecryption().RandomString(20);
                accessKey.DateLastModified = DateTime.Now;
                //save transaction
                _databaseConnection.Entry(accessKey).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                return RedirectToAction("Dashboard", "Home");
            }
            //display notification
            TempData["display"] = "You have already activated your account, use your username and password to login!";
            TempData["notificationtype"] = NotificationType.Info.ToString();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult ForgotPasswordLink()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordLink(AccountModel model)
        {
            var user = _databaseConnection.AppUsers.SingleOrDefault(n=>n.Email == model.Email);
            var access = _databaseConnection.AccessKeys.SingleOrDefault(n => n.AppUserId == user.AppUserId);
            var link = _hostingEnv.WebRootPath;
            var mail = new Mailer();
            mail.SendForgotPasswordResetLink(link + "\\EmailTemplates\\ForgotPassword.html", user, access);
            //display notification
            TempData["display"] = "You have successfully sent a password rest lnk to your email!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login");
        }

        public ActionResult ForgotPassword(string id)
        {
            AppUser user = null;
            var accessKey = _databaseConnection.AccessKeys.SingleOrDefault(n => n.PasswordAccessCode == id);
            if (accessKey != null)
            {
                if (DateTime.Now > accessKey.ExpiryDate)
                {
                    //display notification
                    TempData["display"] = "This link has already expired, Reset the password again!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Login", "Account");
                }
                user = _databaseConnection.AppUsers.Find(accessKey.AppUserId);
                //update accessKeys
                accessKey.PasswordAccessCode = new Md5Ecryption().RandomString(15);
                accessKey.DateLastModified = DateTime.Now;
                //save transaction
                _databaseConnection.Entry(accessKey).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return View(user);
            }
            //display notification
            TempData["display"] = "This link is not genuine!";
            TempData["notificationtype"] = NotificationType.Error.ToString();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(AppUser appUser)
        {
            try
            {
                //populate object and save transaction
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                appUser.Password = new Hashing().HashPassword(appUser.Password);
                appUser.Password = appUser.Password;
                appUser.LastModifiedBy = signedInUserId;
                appUser.DateLastModified = DateTime.Now;
                _databaseConnection.Entry(appUser).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //display notification
                TempData["display"] = "You have succesfully changed your password!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = "You are unable to update your profile, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(appUser);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountModel model)
        {
            var appUser = new AppUser
            {
                Name = model.Username,
                Mobile = model.Mobile,
                Email = model.Email,
                Password = new Hashing().HashPassword(model.ConfirmPassword),
                Username = model.Username,
                Status = UserStatus.Inactive.ToString()
            };
            appUser.ConfirmPassword = appUser.Password;
            appUser.DateCreated = DateTime.Now;
            appUser.DateLastModified = DateTime.Now;
            appUser.RoleId = 2;

            if (_databaseConnection.AppUsers.Any(n => n.Email == appUser.Email || n.Username == appUser.Username))
            {
                //display notification
                TempData["display"] = "A user with the same Username/Email already exist, try another credential again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }
            _databaseConnection.AppUsers.Add(appUser);
            _databaseConnection.SaveChanges();

            //define acceskeys
            var accessKey = new AppUserAccessKey
            {
                AppUserId = appUser.AppUserId,
                PasswordAccessCode = new Md5Ecryption().RandomString(15),
                AccountActivationAccessCode = new Md5Ecryption().RandomString(20)
            };

            _databaseConnection.AccessKeys.Add(accessKey);
            _databaseConnection.SaveChanges();

            var userSubscription = new UserSubscription
            {
                AppUserId = appUser.AppUserId,
                DateCreated = DateTime.Now,
                DateLastModified = DateTime.Now,
                CreatedBy = appUser.AppUserId,
                LastModifiedBy = appUser.AppUserId,
                PackageId = 1,
                Status = UserStatus.Active.ToString()
            };

            _databaseConnection.UserSubscriptions.Add(userSubscription);
            _databaseConnection.SaveChanges();
            var role = _databaseConnection.Roles.Find(appUser.RoleId);

            var link = _hostingEnv.WebRootPath;
            var mail = new Mailer();
            mail.SendNewUserEmail(link + "\\email-templates\\NewUser.html", appUser, role, accessKey);
            //display notification
            TempData["display"] = "You have successfully registered to SOS Photo Studio, Check your email to confirm your account!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            _databaseConnection.Dispose();
            HttpContext.Session.Clear();
            if (returnUrl != null)
            {
                //display notification
                TempData["display"] = "Your session has expired, Login to continue!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountModel model)
        {
            AppUser userExist = null;
            List<PhotographerCategoryMapping> mapping = null;
                userExist =
                _databaseConnection.AppUsers.SingleOrDefault(
                    n => n.Email == model.Email || n.Username == model.Username);
       
            if (userExist != null && userExist.Status == UserStatus.Inactive.ToString())
            {
                //display notification
                TempData["display"] =
                    "You are yet to activate your account from the the link sent to your email when you created the account!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }
            if (userExist == null)
            {
                //display notification
                TempData["display"] = "The Account does not exist, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }

            var passwordCorrect = new Hashing().ValidatePassword(model.Password, userExist.ConfirmPassword);
            if (passwordCorrect == false)
            {
                //display notification
                TempData["display"] = "Dear " + userExist.Name + " your Password is Incorrect, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return View(model);
            }

            var role = _databaseConnection.Roles.Find(userExist.RoleId);
            //convert object to json string and insert into session
            var userString = JsonConvert.SerializeObject(userExist);
            HttpContext.Session.SetString("ImageLoggedInUser", userString);

            //convert object to json string and insert into session
            var roleString = JsonConvert.SerializeObject(role);
            HttpContext.Session.SetString("Role", roleString);

            //set user id inti=o session string
            HttpContext.Session.SetInt32("userId", (int) userExist.AppUserId);
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            mapping = _databaseConnection.PhotographerCategoryMappings.Where(n => n.AppUserId == signedInUserId).ToList();
            if (mapping.Count <= 0 && role.UploadImage)
            {
                //display notification
                TempData["display"] = "You have succesfully logged in," +
                                      "It is always adviced that you select the photographer categories you are involved with to fully setup your account!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("SelectCategories", "PhotographerCategory");
            }
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            _databaseConnection.Dispose();
            return RedirectToAction("Login", "Account");
        }
    }
}