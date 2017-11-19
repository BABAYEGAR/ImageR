using System;
using System.Linq;
using Image.Models;
using Image.Models.APIFactory;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Image.Models.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var users = new AppUserFactory().GetAllUsersAsync(new AppConfig().FetchUsersUrl);
            ViewBag.Role = _databaseConnection.Roles.ToList();
            return View(users.Result);
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
        public ActionResult Create(AppUser model, IFormCollection collection)
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
                    BackgroundPicture = "photo1",
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    RoleId = model.RoleId,
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

                        _databaseConnection.AccessKeys.Add(accessKey);
                        _databaseConnection.SaveChanges();

                    var role = _databaseConnection.Roles.Find(appUser.RoleId);

                    var link = _hostingEnv.WebRootPath;
                    var mail = new Mailer();
                    mail.SendNewUserEmail(link + "\\EmailTemplates\\NewUser.html", appUser, role, accessKey);
                    //display notification
                        TempData["display"] =
                            response.AccessLog.Message;
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index","AppUser");
                
            }
            catch (Exception)
            {
                //display notification
                TempData["display"] =
                    "There was an issue trying to register,Check your intenet connection and Try Again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View(model);
            }
        }
        // GET: AppUser/Edit/5
        [SessionExpireFilter]
        public ActionResult ActivateUser(long id)
        {
            var response = new AppUserFactory().ActivateUser(new AppConfig().ActivateAccountUrl, id).Result;
            //display notification
            TempData["display"] = response.AccessLog.Message;
            TempData["notificationtype"] = NotificationType.Error.ToString();
            return RedirectToAction("Index");
        }
        // GET: AppUser/Edit/5
        [SessionExpireFilter]
        public ActionResult DeactivateUser(IFormCollection collection)
        {
            long id = Convert.ToInt64(collection["UserId"]);
            var response = new AppUserFactory().DeactivateUser(new AppConfig().DeActivateAccount, id).Result;
            //display notification
            TempData["display"] = response.AccessLog.Message;
            TempData["notificationtype"] = NotificationType.Error.ToString();
            return RedirectToAction("Index");
        }

    }
}