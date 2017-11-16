﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Image.Models;
using Image.Models.APIFactory;
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
        private readonly List<AppUser> users;

        public AccountController(IHostingEnvironment env, ImageDataContext databaseConnection)
        {
            _hostingEnv = env;
            _databaseConnection = databaseConnection;
            users = new AppUserFactory().GetAllUsersAsync(new AppConfig().FetchUsersUrl).Result;
        }

        [SessionExpireFilter]
        public ActionResult Profile()
        {
            ViewBag.Users = users;
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            ViewBag.Images = _databaseConnection.Images.Include(n => n.ImageCategory)
                .Include(n => n.ImageComments).Include(n => n.ImageTags)
                .Include(n => n.Location).Include(n => n.ImageSubCategory).ToList();
            ViewBag.ImageComments = _databaseConnection.ImageComments.Where(n => n.AppUserId == signedInUserId)
                .ToList();
            ViewBag.Rating = _databaseConnection.ImageActions.ToList();
            return View();
        }
        public ActionResult Notification()
        {
            return View(_databaseConnection.SystemNotifications.ToList());
        }
        public ActionResult MarkNotificationAsRead(long id)
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var notification = _databaseConnection.SystemNotifications.Find(id);
            notification.DateLastModified = DateTime.Now;
            notification.LastModifiedBy = signedInUserId;
            notification.Read = true;

            _databaseConnection.Entry(notification).State = EntityState.Modified;
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have succesfully marked the notification as read!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Notification");
        }
        public ActionResult ChangeProfileImage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfileImage(IList<IFormFile> profile, IList<IFormFile> background)
        {
            ViewBag.Users = users;
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            AppUser appUser = null;
            var userString = HttpContext.Session.GetString("ImageLoggedInUser");
            appUser = JsonConvert.DeserializeObject<AppUser>(userString);

            var response = new ActionResponse();
            if (profile.Count > 0)
                foreach (var file in profile)
                {
                    var fileInfo = new FileInfo(file.FileName);
                    var ext = fileInfo.Extension.ToLower();
                    var name = DateTime.Now.ToFileTime().ToString();
                    var fileName = name + ext;
                    var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\ProfilePicture\{fileName}";

                    using (var fs = System.IO.File.Create(uploadedImage))
                    {
                        if (fs != null)
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                            appUser.ProfilePicture = fileName;
                            appUser.LastModifiedBy = signedInUserId;
                            appUser.DateLastModified = DateTime.Now;
                            response = new AppUserFactory().EditProfile(new AppConfig().EditProfileUrl, appUser).Result;
                            if (response.AppUser != null)
                            {
                                var newUserString = JsonConvert.SerializeObject(appUser);
                                HttpContext.Session.SetString("ImageLoggedInUser", newUserString);
                            }
                        }
                    }
                }
            if (background.Count > 0)
                foreach (var file in background)
                {
                    var fileInfo = new FileInfo(file.FileName);
                    var ext = fileInfo.Extension.ToLower();
                    var name = DateTime.Now.ToFileTime().ToString();
                    var fileName = name + ext;
                    var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\ProfileBackground\{fileName}";

                    using (var fs = System.IO.File.Create(uploadedImage))
                    {
                        if (fs != null)
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                            appUser.BackgroundPicture = fileName;
                            appUser.LastModifiedBy = signedInUserId;
                            appUser.DateLastModified = DateTime.Now;
                            response = new AppUserFactory().EditProfile(new AppConfig().EditProfileUrl, appUser).Result;
                            if (response.AppUser != null)
                            {
                                var newerUserString = JsonConvert.SerializeObject(appUser);
                                HttpContext.Session.SetString("ImageLoggedInUser", newerUserString);
                            }
                        }
                    }
                }
            if (background.Count <= 0 && profile.Count <= 0)
            {
                //display notification
                TempData["display"] = "No Image has been selected!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View();
            }
            //display notification
            TempData["display"] = "You have succesfully uploaded a new profile/background!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Profile");
        }
        [SessionExpireFilter]
        public ActionResult UserBank()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var userBank = _databaseConnection.UserBanks.SingleOrDefault(n => n.CreatedBy == signedInUserId);
            if (userBank.BankId != null)
            {
                ViewBag.BankId = new SelectList(
                    _databaseConnection.Bank.ToList(), "BankId",
                    "Name", userBank.BankId);
            }
            else
            {
                ViewBag.BankId = new SelectList(
                    _databaseConnection.Bank.ToList(), "BankId",
                    "Name");
            }
            return View(userBank);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult UserBank(UserBank userBank)
        {
            try
            {
                //populate object and save transaction
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                userBank.LastModifiedBy = signedInUserId;
                userBank.DateLastModified = DateTime.Now;

                _databaseConnection.Entry(User).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                var bankString = JsonConvert.SerializeObject(userBank);
                HttpContext.Session.SetString("UserBank", bankString);

                //display notification
                TempData["display"] = "You have successfully updated your bank information";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = ex.ToString();
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(userBank);
            }
        }
        [SessionExpireFilter]
        public ActionResult EditProfile()
        {
            AppUser appUser = null;
            var userString = HttpContext.Session.GetString("ImageLoggedInUser");
            appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            return View(appUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult EditProfile(AppUser appUser)
        {
            try
            {
                //populate object and save transaction
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                appUser.LastModifiedBy = signedInUserId;
                appUser.DateLastModified = DateTime.Now;
                appUser.TenancyId = new AppConfig().TenancyId;
                var resonse = new AppUserFactory().EditProfile(new AppConfig().EditProfileUrl, appUser);


                if (resonse.Result.AppUser == null)
                {
                    //display notification
                    TempData["display"] = resonse.Result.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(appUser);
                }
                var userString = JsonConvert.SerializeObject(resonse.Result.AppUser);
                HttpContext.Session.SetString("ImageLoggedInUser", userString);

                //display notification
                TempData["display"] = resonse.Result.AccessLog.Message;
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile");
            }
            catch (Exception)
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult ChangePassword(AccountModel model)
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                AppUser appUser = null;
                var userString = HttpContext.Session.GetString("ImageLoggedInUser");
                appUser = JsonConvert.DeserializeObject<AppUser>(userString);
                if (appUser != null)
                {
                    appUser.Password = model.Password;
                    appUser.Password = appUser.Password;
                    appUser.LastModifiedBy = signedInUserId;
                    appUser.DateLastModified = DateTime.Now;
                }

                var resonse = new AppUserFactory().ChangePassword(new AppConfig().ChangePasswordrl, appUser);


                if (resonse.Result.AppUser == null)
                {
                    //display notification
                    TempData["display"] = resonse.Result.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(model);
                }
                var newUserString = JsonConvert.SerializeObject(resonse.Result.AppUser);
                HttpContext.Session.SetString("ImageLoggedInUser", newUserString);

                //display notification
                TempData["display"] = resonse.Result.AccessLog.Message;
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = ex.ToString();
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }
        }

        public ActionResult AccountActivationLink(string accessCode)
        {
            var accessKey =
                _databaseConnection.AccessKeys.SingleOrDefault(n => n.AccountActivationAccessCode == accessCode);
            var appUser = users.SingleOrDefault(n => n.AppUserId == accessKey.AppUserId);
            if (appUser.Status == UserStatus.Inactive.ToString())
            {
                //update user
                appUser.Status = UserStatus.Active.ToString();
                _databaseConnection.Entry(appUser).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                var role = _databaseConnection.Roles.Find(appUser.RoleId);
                //convert object to json string and insert into session
                var userString = JsonConvert.SerializeObject(appUser);
                HttpContext.Session.SetString("ImageLoggedInUser", userString);

                //convert object to json string and insert into session
                var roleString = JsonConvert.SerializeObject(role);
                HttpContext.Session.SetString("Role", roleString);

                //set user id inti=o session string
                HttpContext.Session.SetInt32("userId", (int) appUser.AppUserId);


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
            var response = new ActionResponse();
            model.TenancyId = new AppConfig().TenancyId;
            response = new AppUserFactory().ForgetPasswordLink(new AppConfig().ForgotPasswordLinkUrl, model).Result;
            var access = _databaseConnection.AccessKeys.SingleOrDefault(n => n.AppUserId == response.AppUser.AppUserId);
            var link = _hostingEnv.WebRootPath;
            var mail = new Mailer();
            mail.SendForgotPasswordResetLink(link + "\\EmailTemplates\\ForgotPassword.html", response.AppUser, access);
            //display notification
            TempData["display"] = response.AccessLog.Message;
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login");
        }

        public ActionResult ForgotPassword(string accessCode)
        {
            AppUser user = null;
            var model = new AccountModel();
            var accessKey = _databaseConnection.AccessKeys.SingleOrDefault(n => n.PasswordAccessCode == accessCode);
            if (accessKey != null)
            {
                if (DateTime.Now > accessKey.ExpiryDate)
                {
                    //display notification
                    TempData["display"] = "This link has already expired, Reset the password again!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Login", "Account");
                }
                user = users.SingleOrDefault(n => n.AppUserId == accessKey.AppUserId);

                if (user != null)
                {
                    model.Email = user.Email;
                    model.Username = user.Username;
                    model.TenancyId = new AppConfig().TenancyId;
                }
                //update accessKeys
                accessKey.PasswordAccessCode = new Md5Ecryption().RandomString(15);
                accessKey.DateLastModified = DateTime.Now;
                //save transaction
                _databaseConnection.Entry(accessKey).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return View(model);
            }
            //display notification
            TempData["display"] = "This link is not genuine!";
            TempData["notificationtype"] = NotificationType.Error.ToString();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(AccountModel model)
        {
            try
            {
                
                //populate object and save transaction
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                var response = new ActionResponse();
                response = new AppUserFactory().PasswordReset(new AppConfig().ResetPasswordUrl, model).Result;

                //display notification
                TempData["display"] = response.AccessLog.Message;
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = ex.ToString();
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }
        }

        public ActionResult Register(long? packageId)
        {
            if (packageId != null)
                ViewBag.PackageId = packageId;
            else
                ViewBag.PackageId = 5;
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountModel model, IFormCollection collection)
        {
            ActionResponse response = null;
            try
            {
                var appUser = new AppUser
                {
                    Name = model.Username,
                    Mobile = model.Mobile,
                    Email = model.Email,
                    Username = model.Username,
                    Status = UserStatus.Inactive.ToString(),
                    ProfilePicture = "Avatar.jpg",
                    BackgroundPicture = "photo1.jpg",
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    RoleId = 2,
                    TenancyId = new AppConfig().TenancyId,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                //define acceskeys
                var accessKey = new AppUserAccessKey
                {
                    PasswordAccessCode = new Md5Ecryption().RandomString(15),
                    AccountActivationAccessCode = new Md5Ecryption().RandomString(20),
                    CreatedBy = appUser.AppUserId,
                    LastModifiedBy = appUser.AppUserId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddDays(1)
                };

                long? packageId = null;
                Package package = null;
                if (collection["PackageId"] != "")
                {
                    packageId = Convert.ToInt64(collection["PackageId"]);
                    package = _databaseConnection.Packages.Find(packageId);
                }


                var userSubscription = new UserSubscription
                {
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    CreatedBy = appUser.AppUserId,
                    LastModifiedBy = appUser.AppUserId,
                    PackageId = packageId,
                    Status = UserStatus.Active.ToString(),
                    ExpiryDate = DateTime.Now.AddMonths(1),
                    MonthLength = 1
                };

                if (packageId == 5 || packageId == null)
                {
                    if (package != null)
                    {
                        response = new AppUserFactory()
                            .RegisterUser(new AppConfig().RegisterUsersUrl, appUser).Result;
                        if (response.AccessLog.Status == "Denied")
                        {
                            //display notification
                            TempData["display"] =
                                response.AccessLog.Message;
                            TempData["notificationtype"] = NotificationType.Error.ToString();
                            return View(model);
                        }
                    

                        appUser.AppUserId = response.AppUser.AppUserId;
                        accessKey.AppUserId = appUser.AppUserId;
                        userSubscription.AppUserId = appUser.AppUserId;

                        var userBank = new UserBank
                        {
                            CreatedBy = appUser.AppUserId,
                            LastModifiedBy = appUser.AppUserId,
                            DateCreated = DateTime.Now,
                            DateLastModified = DateTime.Now,
                        };
                        _databaseConnection.AccessKeys.Add(accessKey);
                        _databaseConnection.SaveChanges();

                        _databaseConnection.UserSubscriptions.Add(userSubscription);
                        _databaseConnection.SaveChanges();

                        _databaseConnection.UserBanks.Add(userBank);
                        _databaseConnection.SaveChanges();
                    }


                    var role = _databaseConnection.Roles.Find(appUser.RoleId);

                    var link = _hostingEnv.WebRootPath;
                    var mail = new Mailer();
                    mail.SendNewUserEmail(link + "\\EmailTemplates\\NewUser.html", appUser, role, accessKey);
                    //display notification
                    if (response != null)
                        TempData["display"] =
                            response.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Login");
                }

                //convert object to json string and insert into session
                var userString = JsonConvert.SerializeObject(appUser);
                HttpContext.Session.SetString("User", userString);

                //convert object to json string and insert into session
                var subscriptionString = JsonConvert.SerializeObject(userSubscription);
                HttpContext.Session.SetString("Subscription", subscriptionString);

                //convert object to json string and insert into session
                var accessString = JsonConvert.SerializeObject(accessKey);
                HttpContext.Session.SetString("Access", accessString);
            }
            catch (Exception ex)
            {
                //display notification
                TempData["display"] = ex.ToString();
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }

            TempData["display"] =
                response.AccessLog.Message;
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("SubscriptionInvoice");
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
            model.TenancyId = new AppConfig().TenancyId;
            AppUser userExist = null;
            List<PhotographerCategoryMapping> mapping = null;
            var response = new AuthenticateFactory().Login(new AppConfig().LoginUrl, model);

            if (response.Result.AppUser == null)
            {
                //display notification
                TempData["display"] = response.Result.AccessLog.Message;
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }
            userExist = response.Result.AppUser;
            var role = _databaseConnection.Roles.Find(userExist.RoleId);
            //convert object to json string and insert into session
            var userString = JsonConvert.SerializeObject(userExist);
            HttpContext.Session.SetString("ImageLoggedInUser", userString);

            //convert object to json string and insert into session
            var usersString = JsonConvert.SerializeObject(users);
            HttpContext.Session.SetString("Users", usersString);

            var notifications = _databaseConnection.SystemNotifications.ToList();
            //convert object to json string and insert into session
            var notificationString = JsonConvert.SerializeObject(notifications);
            HttpContext.Session.SetString("Notifications", notificationString);
            if (role.UploadImage)
            {
                var userSubscription =
                    _databaseConnection.UserSubscriptions.Include(n => n.Package).SingleOrDefault(
                        n => n.AppUserId == userExist.AppUserId && n.Status == UserStatus.Active.ToString());
                if (userSubscription != null)
                {
                    var package = _databaseConnection.Packages.Find(userSubscription.PackageId);
                    //convert object to json string and insert into session
                    var packageString = JsonConvert.SerializeObject(package);
                    HttpContext.Session.SetString("Package", packageString);
                }
            }
            //convert object to json string and insert into session
            var userBank = _databaseConnection.UserBanks.SingleOrDefault(n => n.CreatedBy == userExist.AppUserId);
            var bankString = JsonConvert.SerializeObject(userBank);
            HttpContext.Session.SetString("UserBank", bankString);

            //convert object to json string and insert into session
            var roleString = JsonConvert.SerializeObject(role);
            HttpContext.Session.SetString("Role", roleString);

            //set user id inti=o session string
            HttpContext.Session.SetInt32("userId", (int) userExist.AppUserId);
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            mapping = _databaseConnection.PhotographerCategoryMappings.Where(n => n.AppUserId == signedInUserId)
                .ToList();
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

        public ActionResult SubscriptionInvoice(IFormCollection collection)
        {
            var subscriptionString = HttpContext.Session.GetString("Subscription");
            var userSubscription = JsonConvert.DeserializeObject<UserSubscription>(subscriptionString);

            var userString = HttpContext.Session.GetString("User");
            var appUser = JsonConvert.DeserializeObject<AppUser>(userString);

            ViewBag.Subscription = userSubscription;
            ViewBag.User = appUser;
            return View(_databaseConnection.Packages.Find(userSubscription.PackageId));
        }

        [HttpPost]
        public ActionResult SubscriptionInvoice(AccountModel model, IFormCollection collection)
        {
            var userString = HttpContext.Session.GetString("User");
            var appUser = JsonConvert.DeserializeObject<AppUser>(userString);

            var accessString = HttpContext.Session.GetString("Access");
            var accessKey = JsonConvert.DeserializeObject<AppUserAccessKey>(accessString);

            var subscriptionString = HttpContext.Session.GetString("Subscription");
            var userSubscription = JsonConvert.DeserializeObject<UserSubscription>(subscriptionString);

            //_databaseConnection.AppUsers.Add(appUser);
            _databaseConnection.SaveChanges();

            accessKey.AppUserId = appUser.AppUserId;
            userSubscription.AppUserId = appUser.AppUserId;

            _databaseConnection.AccessKeys.Add(accessKey);
            _databaseConnection.SaveChanges();

            _databaseConnection.UserSubscriptions.Add(userSubscription);
            _databaseConnection.SaveChanges();

            var role = _databaseConnection.Roles.Find(appUser.RoleId);

            var link = _hostingEnv.WebRootPath;
            var mail = new Mailer();
            mail.SendNewUserEmail(link + "\\EmailTemplates\\NewUser.html", appUser, role, accessKey);
            //display notification
            TempData["display"] =
                "You have successfully registered to SOS Photo Studio, Check your email to confirm your account!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            _databaseConnection.Dispose();
            return RedirectToAction("Login", "Account");
        }
    }
}