using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using CamerackStudio.Models.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CamerackStudio.Controllers
{
    public class AppUserController : Controller
    {

        private readonly CamerackStudioDataContext _databaseConnection;
        AppUser _appUser;
        private List<PushNotification> pushNotifications;

        public AppUserController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            pushNotifications = new AppUserFactory().GetAllPushNotifications(new AppConfig()
                .UsersPushNotifications).Result.Where(n => n.ClientId == new AppConfig().ClientId).ToList();
        }
        [SessionExpireFilter]
        public IActionResult AllUsers()
        {
            try
            {
                var users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result.ToList();
                return View(users);
            }
            catch (Exception)
            {
                //display notification
                TempData["display"] = "An error ocurred while fetching your orders check your internet connectivity and try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View();
            }
        }
        [SessionExpireFilter]
        public IActionResult Customers()
        {
            try
            {
                var users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl)
                    .Result.Where(n=>n.RoleId == 3).ToList();
                return View(users);
            }
            catch (Exception)
            {
                //display notification
                TempData["display"] = "An error ocurred while fetching your orders check your internet connectivity and try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View();
            }
        }
        [SessionExpireFilter]
        public IActionResult Photographers()
        {
            try
            {
                var users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl)
                    .Result.Where(n=>n.RoleId == 3);
                List<AppUser> results = new List<AppUser>();
                var images = _databaseConnection.Images;
                foreach (var item in users)
                {
                    if (images.Where(n => n.AppUserId == item.AppUserId).ToList().Count > 0)
                    {
                        results.Add(item);
                    }   
                }
                return View(results);
            }
            catch (Exception)
            {
                //display notification
                TempData["display"] = "An error ocurred while fetching your orders check your internet connectivity and try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View();
            }
        }
    }
}