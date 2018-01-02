using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using CamerackStudio.Models.RabbitMq;
using CamerackStudio.Models.Redis;
using CamerackStudio.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class AccountController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private readonly List<AppUser> _users;

        public AccountController(IHostingEnvironment env, CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result;
        }

        [SessionExpireFilter]
        public ActionResult Profile()
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var appTransport = new AppTransport
            {
                AppUsers = _users,
                Images = _databaseConnection.Images.Include(n => n.ImageCategory)
                    .Include(n => n.ImageComments).Include(n => n.ImageTags)
                    .Include(n => n.Location).Include(n => n.ImageSubCategory).ToList(),
                ImageComments = _databaseConnection.ImageComments.ToList(),
                ImageActions = _databaseConnection.ImageActions.ToList(),
                AppUser = _users.SingleOrDefault(n=>n.AppUserId ==signedInUserId )
            };
            return View(appTransport);
        }
        [SessionExpireFilter]
        public ActionResult UserProfile(long id)
        {
            var appTransport = new AppTransport
            {
                AppUsers = _users,
                Images = _databaseConnection.Images.Include(n => n.ImageCategory)
                    .Include(n => n.ImageComments).Include(n => n.ImageTags)
                    .Include(n => n.Location).Include(n => n.ImageSubCategory).ToList(),
                ImageComments = _databaseConnection.ImageComments.ToList(),
                ImageActions = _databaseConnection.ImageActions.ToList(),
                Image = _databaseConnection.Images.Find(id),
                AppUser = _users.SingleOrDefault(n => n.AppUserId == id)
            };
            return View(appTransport);
        }
        [SessionExpireFilter]
        public ActionResult SingleImage(long id)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var appTransport = new AppTransport
            {
                AppUsers = _users,
                Images = _databaseConnection.Images.Include(n => n.ImageCategory)
                    .Include(n => n.ImageComments).Include(n => n.ImageTags)
                    .Include(n => n.Location).Include(n => n.ImageSubCategory).ToList(),
                ImageComments = _databaseConnection.ImageComments.ToList(),
                ImageActions = _databaseConnection.ImageActions.ToList(),
                Image = _databaseConnection.Images.Find(id)
            };
            return View(appTransport);
        }
        public ActionResult Notification()
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            return View(_databaseConnection.SystemNotifications.Where(n=>n.AppUserId == signedInUserId).ToList());
        }

        public ActionResult MarkNotificationAsRead(long id)
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
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
            ViewBag.Users = _users;
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var userString = HttpContext.Session.GetString("CamerackLoggedInUser");
            var appUser = JsonConvert.DeserializeObject<AppUser>(userString);

            ActionResponse response;
            if (profile.Count > 0)
                foreach (var file in profile)
                {
                    var fileInfo = new FileInfo(file.FileName);
                    var ext = fileInfo.Extension.ToLower();
                    var name = DateTime.Now.ToFileTime().ToString();
                    var fileName = name + ext;
                    var uploadedImage = new AppConfig().ProfilePicture + fileName;

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
                                new RedisDataAgent().SetStringValue("CamerackLoggedInUser",newUserString);
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
                    //var uploadedImage = _hostingEnv.WebRootPath + $@"\UploadedImage\ProfileBackground\{fileName}";
                    var uploadedImage = new AppConfig().ProfileBackgorundPicture + fileName;

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
                                new RedisDataAgent().SetStringValue("CamerackLoggedInUser", newerUserString);
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
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var userBank = _databaseConnection.UserBanks.SingleOrDefault(n => n.CreatedBy == signedInUserId);
            if (userBank.BankId != null)
                ViewBag.BankId = new SelectList(
                    _databaseConnection.Banks.ToList(), "BankId",
                    "Name", userBank.BankId);
            else
                ViewBag.BankId = new SelectList(
                    _databaseConnection.Banks.ToList(), "BankId",
                    "Name");
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
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                userBank.LastModifiedBy = signedInUserId;
                userBank.DateLastModified = DateTime.Now;

                _databaseConnection.Entry(userBank).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                if (string.IsNullOrEmpty(userBank.AccountName) || userBank.BankId <= 0
                    || string.IsNullOrEmpty(userBank.AccountName) &&
                    new RedisDataAgent().GetStringValue("UserBank") == null)
                {
                    var bankString = JsonConvert.SerializeObject(userBank);
                    new RedisDataAgent().SetStringValue("UserBank", bankString);
                }
                else
                {
                    new RedisDataAgent().DeleteStringValue("UserBank");
                }
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
            var userString = new RedisDataAgent().GetStringValue("CamerackLoggedInUser");
            var appUser = JsonConvert.DeserializeObject<AppUser>(userString);
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
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                appUser.LastModifiedBy = signedInUserId;
                appUser.DateLastModified = DateTime.Now;
                appUser.ClientId = new AppConfig().ClientId;
                var resonse = new AppUserFactory().EditProfile(new AppConfig().EditProfileUrl, appUser);


                if (resonse.Result.AppUser == null)
                {
                    //display notification
                    TempData["display"] = resonse.Result.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(appUser);
                }
                var userString = JsonConvert.SerializeObject(resonse.Result.AppUser);
                new RedisDataAgent().SetStringValue("CamerackLoggedInUser",userString);

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
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                var userString = new RedisDataAgent().GetStringValue("CamerackLoggedInUser");
                var appUser = JsonConvert.DeserializeObject<AppUser>(userString);
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
                new RedisDataAgent().SetStringValue("CamerackLoggedInUser",newUserString);

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

        public async Task<ActionResult> AccountActivationLink(string accessCode)
        {
            var accessKey = new AppUserFactory().GetUsersAccessKey(new AppConfig().FetchUsersAccessKeys)
                .Result.SingleOrDefault(n => n.PasswordAccessCode == accessCode);
            var appUser = _users.SingleOrDefault(n => accessKey != null && n.AppUserId == accessKey.AppUserId);
            if (appUser != null)
            {
                if (appUser.Status == UserStatus.Inactive.ToString())
                {
                    //update user
                    var response =
                        await new AppUserFactory().ActivateUser(new AppConfig().ActivateAccountUrl, appUser.AppUserId);
                    if (response.AppUser != null)
                    {
                        //display notification
                        TempData["display"] =
                            "You have successfully verified your account, Login and Enjoy the Experience!";
                        TempData["notificationtype"] = NotificationType.Success.ToString();
                        return RedirectToAction("Login", "Account");
                    }
                    //display notification
                    TempData["display"] =
                        "There was an issue Activating your Account Try again or Contact Camerack Support!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Login", "Account");
                }
                if (appUser.Status == UserStatus.Active.ToString())
                {
                    //display notification
                    TempData["display"] =
                        "You have already activated your account, use your username and password to login!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
            }
            //display notification
            TempData["display"] =
                "Your Reuqest is Invalid, Try again Later!";
            TempData["notificationtype"] = NotificationType.Error.ToString();
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
            model.ClientId = new AppConfig().ClientId;
            var response = new AppUserFactory().ForgetPasswordLink(new AppConfig().ForgotPasswordLinkUrl, model).Result;

            //display notification
            TempData["display"] = response.AccessLog.Message;
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> ForgotPassword(string accessCode)
        {
            var model = new AccountModel();
            var accessKey = new AppUserFactory().GetUsersAccessKey(new AppConfig().FetchUsersAccessKeys)
                .Result.SingleOrDefault(n => n.PasswordAccessCode == accessCode);
            if (accessKey != null)
            {
                if (DateTime.Now > accessKey.ExpiryDate)
                {
                    //display notification
                    TempData["display"] = "This link has already expired, Reset the password again!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Login", "Account");
                }
                var user = _users.SingleOrDefault(n => n.AppUserId == accessKey.AppUserId);
                if (user != null)
                {
                    model.Email = user.Email;
                    model.Username = user.Username;
                    model.ClientId = new AppConfig().ClientId;
                    model.LoginName = user.Email;
                }
                //update accessKeys
                if (user != null)
                    await new AppUserFactory().UpdatePasswordAccessKey(new AppConfig().UpdatePasswordAccessKey,
                        user.AppUserId);
                return View(model);
            }
            //display notification
            TempData["display"] = "This link is not genuine!";
            TempData["notificationtype"] = NotificationType.Error.ToString();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(AccountModel model)
        {
            try
            {
                //populate object and save transaction
                var response = new AppUserFactory().PasswordReset(new AppConfig().ResetPasswordUrl, model).Result;

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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountModel model, IFormCollection collection)
        {
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
                    RoleId = 3,
                    ClientId = new AppConfig().ClientId,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };
                //post user object and get response
                var response = new AppUserFactory()
                    .RegisterUser(new AppConfig().RegisterUsersUrl, appUser).Result;

                //validate response
                if (response.AccessLog.Status == "Denied")
                {
                    //display notification
                    TempData["display"] =
                        response.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(model);
                }
                if (response.AppUser != null)
                {
                    appUser = response.AppUser;

                    //populate and save bank transaction
                    var userBank = new UserBank
                    {
                        CreatedBy = appUser.AppUserId,
                        LastModifiedBy = appUser.AppUserId,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                    };
                    _databaseConnection.UserBanks.Add(userBank);
                    _databaseConnection.SaveChanges();

                    //display notification
                    TempData["display"] =
                        response.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Login");
                }
                //display notification
                TempData["display"] = "This Request is Unavailable, Try again Later";
                TempData["notificationtype"] = NotificationType.Success.ToString();
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            _databaseConnection.Dispose();
            HttpContext.Session.Clear();
            if (new RedisDataAgent().GetStringValue("CamerackLoggedInUser") != null)
            {
                return RedirectToAction("Dashboard", "Home");
            }
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
            try
            {
                model.ClientId = new AppConfig().ClientId;
                var response = new AuthenticateFactory().Login(new AppConfig().LoginUrl, model);

                if (response.Result.AppUser == null)
                {
                    //display notification
                    TempData["display"] = response.Result.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(model);
                }
                var userExist = response.Result.AppUser;
                //convert object to json string and insert into session
                var userString = JsonConvert.SerializeObject(userExist);
                new RedisDataAgent().SetStringValue("CamerackLoggedInUser",userString);

                var userBank = _databaseConnection.UserBanks.SingleOrDefault(n => n.CreatedBy == userExist.AppUserId);
                if(string.IsNullOrEmpty(userBank.AccountName) || userBank.BankId <= 0
                    || string.IsNullOrEmpty(userBank.AccountName) && new RedisDataAgent().GetStringValue("UserBank") == null)
                {
                    var bankString = JsonConvert.SerializeObject(userBank);
                    new RedisDataAgent().SetStringValue("UserBank", bankString);
                }
                else
                {
                    new RedisDataAgent().DeleteStringValue("UserBank");
                }
                //set user id into session string
                new RedisDataAgent().SetStringValue("CamerackLoggedInUserId",userExist.AppUserId.ToString());
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
                var mapping = _databaseConnection.PhotographerCategoryMappings.Where(n => n.AppUserId == signedInUserId)
                    .ToList();

                if (mapping.Count <= 0 && userExist.Role.UploadImage)
                {
                    //display notification
                    TempData["display"] = "You have succesfully logged in," +
                                          "It is always adviced that you select the photographer categories you are involved with to fully setup your account!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("SelectCategories", "PhotographerCategory");
                }
                return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                TempData["display"] = ex.ToString();
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            _databaseConnection.Dispose();
            new RedisDataAgent().DeleteStringValue("CamerackLoggedInUser");
            new RedisDataAgent().DeleteStringValue("CamerackLoggedInUserId");
            return RedirectToAction("Login", "Account");
        }
    }
}