using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class HomeController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private AppUser _appUser;
        private readonly List<PushNotification> _pushNotifications;

        public HomeController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _databaseConnection.Database.EnsureCreated();
            _pushNotifications = new AppUserFactory().GetAllPushNotifications(new AppConfig().UsersPushNotifications).Result;
        }
        public IActionResult Index(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var appUserKeys = new AppUserFactory().GetUsersAccessKey(new AppConfig().FetchUsersAccessKeys);
                var userKey = appUserKeys.Result.SingleOrDefault(n => n.AccountActivationAccessCode == id);
               
                if (userKey != null)
                {
                    var user = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result
                        .SingleOrDefault(n => n.AppUserId == userKey.AppUserId);
                    if (user != null)
                    {
                        var userSession = JsonConvert.SerializeObject(user);
                        var imageCount = _databaseConnection.Images.Where(n => n.AppUserId == user.AppUserId).ToList().Count;
                        HttpContext.Session.SetString("StudioLoggedInUserId", userKey.AppUserId.ToString());
                        HttpContext.Session.SetString("StudioLoggedInUser", userSession);
                        HttpContext.Session.SetInt32("StudioLoggedInUserImageCount", imageCount);
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            if (HttpContext.Session.GetString("StudioLoggedInUserId") != null &&
                HttpContext.Session.GetString("StudioLoggedInUser") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return Redirect("https://camerack.com/Account/Login?returnUrl=sessionExpired");
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
  
        public ActionResult RealoadNavigation()
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            var notifications =_pushNotifications.Where(n=>n.AppUserId == signedInUserId).ToList();
            return PartialView("Partials/_NotificationPartial", notifications);
        }
        public int RealoadNavigationAndCount()
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            var notifications = _pushNotifications.Where(n => n.AppUserId == signedInUserId).Take(5).ToList();
            return notifications.Count;
        }
        public async Task<IActionResult> Dashboard()
        {
            
            AppTransport appTransport = null;
            if (HttpContext.Session.GetString("StudioLoggedInUser") != null &&
                HttpContext.Session.GetString("StudioLoggedInUserId") != null)
            {
                var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
                var userString = HttpContext.Session.GetString("StudioLoggedInUser");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);

                //update keys
                var updatedKeys = await new AppUserFactory().UpdateAccountActivationAccessKey(
                    new AppConfig().UpdateAccountActivationAccessKey, _appUser.AppUserId);
                if (updatedKeys.AppUserAccessKeyId > 0)
                {
                    //display notification
                    TempData["display"] = "Welcome back " + _appUser.Name + " our awesome photographer!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                }
                //validate bank details
                if (_appUser != null && _appUser.Role.UploadImage)
                    if (_databaseConnection.UserBanks.Where(n => n.CreatedBy == _appUser.AppUserId).ToList().Count <= 0)
                    {
                        //populate and save bank transaction
                        var newUserBank = new UserBank
                        {
                            CreatedBy = _appUser.AppUserId,
                            LastModifiedBy = _appUser.AppUserId,
                            DateCreated = DateTime.Now,
                            DateLastModified = DateTime.Now
                        };
                        _databaseConnection.UserBanks.Add(newUserBank);
                        _databaseConnection.SaveChanges();
                    }
                    else
                    {
                        var userBank =
                            _databaseConnection.UserBanks.SingleOrDefault(n => n.CreatedBy == _appUser.AppUserId);
                        if (userBank != null && (string.IsNullOrEmpty(userBank.AccountName) || userBank.BankId <= 0
                                                 || string.IsNullOrEmpty(userBank.AccountName) &&
                                                 HttpContext.Session.GetString("UserBank") == null))
                        {
                            var bankString = JsonConvert.SerializeObject(userBank);
                            HttpContext.Session.SetString("UserBank", bankString);
                        }
                        else
                        {
                            HttpContext.Session.Remove("UserBank");
                        }
                    }
                //store data temporarily, based on the user role
                if (_appUser != null && _appUser.Role.ManageImages)
                {
                    appTransport = new AppTransport
                    {
                        Images = _databaseConnection.Images.ToList(),
                        Cameras = _databaseConnection.Cameras.ToList(),
                        Locations = _databaseConnection.Locations.ToList(),
                        Orders = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result.ToList(),
                        Payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result
                            .ToList()
                    };
                }
                if (_appUser != null && _appUser.Role.UploadImage)
                {
                    appTransport = new AppTransport
                    {
                        Images = _databaseConnection.Images.Where(n => n.AppUserId == signedInUserId).ToList(),
                        Cameras = _databaseConnection.Cameras
                            .Where(n => n.CreatedBy == signedInUserId).ToList(),
                        Locations = _databaseConnection.Locations
                            .Where(n => n.CreatedBy == signedInUserId).ToList(),
                        Orders = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl)
                            .Result.Where(n => n.CreatedBy == signedInUserId).ToList(),
                        Payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result
                            .Where(n => n.AppUserId == signedInUserId)
                            .ToList()
                    };
                }
                //validate mapping
                if (appTransport != null)
                    appTransport.AppUsers = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result
                        .ToList();
                return View(appTransport);
            }
            return Redirect("https://camerack.com/Account/Login?returnUrl=sessionExpired");
        }

    }
}