using System;
using System.Linq;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using CamerackStudio.Models.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class HomeController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private AppUser _appUser;

        public HomeController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _databaseConnection.Database.EnsureCreated();
        }

        public IActionResult Index()
        {
            if (new RedisDataAgent().GetStringValue("CamerackLoggedInUser") != null)
            {
                var userString = new RedisDataAgent().GetStringValue("CamerackLoggedInUser");
                _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
            }
            if (_appUser != null)
                if (_databaseConnection.UserBanks.Where(n => n.CreatedBy == _appUser.AppUserId).ToList().Count <= 0)
                {
                    //populate and save bank transaction
                    var userBank = new UserBank
                    {
                        CreatedBy = _appUser.AppUserId,
                        LastModifiedBy = _appUser.AppUserId,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                    };
                    _databaseConnection.UserBanks.Add(userBank);
                    _databaseConnection.SaveChanges();
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

        public PartialViewResult RealoadNavigation()
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));
            var notifications = _databaseConnection.SystemNotifications.Where(n => n.AppUserId == signedInUserId)
                .ToList();
            return PartialView("Partials/_NotificationPartial", notifications);
        }
        [SessionExpireFilter]
        public IActionResult Dashboard()
        {
            var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));

            if (new RedisDataAgent().GetStringValue("CamerackLoggedInUser") != null)
            {
                var userString = new RedisDataAgent().GetStringValue("CamerackLoggedInUser");
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
                        || string.IsNullOrEmpty(userBank.AccountName) && new RedisDataAgent().GetStringValue("UserBank") == null)
                    {
                        var bankString = JsonConvert.SerializeObject(userBank);
                        new RedisDataAgent().SetStringValue("UserBank", bankString);
                    }
                    else
                    {
                        new RedisDataAgent().DeleteStringValue("UserBank");
                    }
                }

            //var notifications = _databaseConnection.SystemNotifications.Where(n => n.AppUserId == _appUser.AppUserId)
            //    .ToList();
            ////convert object to json string and insert into session
            //var notificationString = JsonConvert.SerializeObject(notifications);
            //HttpContext.Session.SetString("Notifications", notificationString);

            if (_appUser != null && _appUser.Role.ManageImages)
            {
                ViewBag.Images = _databaseConnection.Images.ToList();
                ViewBag.Cameras = _databaseConnection.Cameras.ToList();
                ViewBag.Locations = _databaseConnection.Locations.ToList();
                ViewBag.Orders = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result.ToList();
                ViewBag.Payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result
                    .ToList();
            }
            if (_appUser != null && _appUser.Role.UploadImage)
            {
                ViewBag.Images = _databaseConnection.Images.Where(n => n.AppUserId == signedInUserId).ToList();
                ViewBag.Cameras = _databaseConnection.Cameras
                    .Where(n => n.CreatedBy == signedInUserId).ToList();
                ViewBag.Locations = _databaseConnection.Locations
                    .Where(n => n.CreatedBy == signedInUserId).ToList();
                var result = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result;
                if (result != null)
                    ViewBag.Orders = result
                        .Where(n => n.CreatedBy == signedInUserId).ToList();
                var payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result;
                if (payments != null)
                    ViewBag.Payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl)
                        .Result.Where(n => n.AppUserId == signedInUserId).ToList();
            }
            //validate mapping
            ViewBag.AppUsers = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result.ToList();

            var mapping = _databaseConnection.PhotographerCategoryMappings.Where(n => n.AppUserId == signedInUserId)
                .ToList();

            if (mapping.Count <= 0 && _appUser != null && _appUser.Role.UploadImage)
            {
                //display notification
                TempData["display"] = "You have succesfully logged in," +
                                      "It is always adviced that you select the photographer categories you are involved with to fully setup your account!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("SelectCategories", "PhotographerCategory");
            }
            return View();
        }
    }
}