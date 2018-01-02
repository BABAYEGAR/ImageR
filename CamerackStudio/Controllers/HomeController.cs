using System;
using System.Linq;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using CamerackStudio.Models.RabbitMq;
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

        public ActionResult RealoadNavigation()
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
            AppTransport appTransport = null;
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

            //check if user is new and send competition details to user
            var competitionNotification = _databaseConnection.SystemNotifications
                .Where(n => n.AppUserId == signedInUserId &&
                            n.Category == SystemNotificationCategory.Competition.ToString()).ToList();
            var competitions = _databaseConnection.Competition.ToList();
            foreach (var item in competitions.Where(n=>n.EndDate > DateTime.Now))
            {
                if (competitionNotification.Any(n => n.ControllerId == item.CompetitionId) == false)
                {
                    var notification = new SystemNotification
                    {
                        AppUserId = signedInUserId,
                        ControllerId = item.CompetitionId,
                        Read = false,
                        Message = item.Name + " Competition has already started, Dont mIss Out!",
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        LastModifiedBy = signedInUserId,
                        CreatedBy = signedInUserId,
                        Category = SystemNotificationCategory.Competition.ToString()
                    };
                    _databaseConnection.SystemNotifications.Add(notification);
                    _databaseConnection.SaveChanges();
                    new SendEmailMessage().SendCompetitionEmailMessage(item);
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
            return View(appTransport);
        }
    }
}