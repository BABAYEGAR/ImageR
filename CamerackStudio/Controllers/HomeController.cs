using System;
using System.Collections.Generic;
using System.Linq;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class HomeController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private AppUser _appUser;
        private List<PushNotification> pushNotifications;

        public HomeController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _databaseConnection.Database.EnsureCreated();
            pushNotifications = new AppUserFactory().GetAllPushNotifications(new AppConfig().UsersPushNotifications).Result;
        }
        [SessionExpireFilter]
        public IActionResult Index(string id)
        {


            if (HttpContext.Session.GetString("CamerackLoggedInUser") != null)
            {
                RedirectToAction("Dashboard");
            }
            else
            {
                HttpContext.Response.Redirect("http://camerack.com/" + "error");
            }
            return View();
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
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("CamerackLoggedInUserId"));
            var notifications = new AppUserFactory().GetAllPushNotifications(new AppConfig().UsersPushNotifications)
                .Result.Where(n=>n.AppUserId == signedInUserId).ToList();
            return PartialView("Partials/_NotificationPartial", notifications);
        }
        public int RealoadNavigationAndCount()
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("CamerackLoggedInUserId"));
            var notifications = new AppUserFactory().GetAllPushNotifications(new AppConfig().UsersPushNotifications)
                .Result.Where(n => n.AppUserId == signedInUserId).Take(5).ToList();
            return notifications.Count;
        }
        //[SessionExpireFilter]
        public IActionResult Dashboard(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var userSessionString = new Md5Ecryption().Decrypt(id);
                var userId = Convert.ToInt64(userSessionString);

                var user = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result
                    .SingleOrDefault(n => n.AppUserId == userId);
                if (user != null)
                {
                    var userSession = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("CamerackLoggedInUserId", userId.ToString());
                    HttpContext.Session.SetString("CamerackLoggedInUser", userSession);
                }
                else
                {
                    return Redirect("http://camerack.com/Home/Index/" + "error");
                }
            }
            else
            {
                return Redirect("http://camerack.com/Home/Index/" + "error");
            }

            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("CamerackLoggedInUserId"));
            AppTransport appTransport = null;
            if (HttpContext.Session.GetString("CamerackLoggedInUser") != null)
            {
                var userString = HttpContext.Session.GetString("CamerackLoggedInUser");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            }

            //validate bank details
            if (_appUser != null)
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
                    if (string.IsNullOrEmpty(userBank.AccountName) || userBank.BankId <= 0
                        || string.IsNullOrEmpty(userBank.AccountName) && HttpContext.Session.GetString("UserBank") == null)
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
                appTransport.AppUsers = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result.ToList();
            return View(appTransport);
        }
    }
}