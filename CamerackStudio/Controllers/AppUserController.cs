using System;
using System.Collections.Generic;
using System.Linq;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using CamerackStudio.Models.RabbitMq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CamerackStudio.Controllers
{
    public class AppUserController : Controller
    {

        private readonly CamerackStudioDataContext _databaseConnection;

        public AppUserController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
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
        public IActionResult Newsletter()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Newsletter(UserEmail email)
        {
            List<AppUser> users = new List<AppUser>();


            var subscriptions = new AppUserFactory().GetAllSubscriptions(new AppConfig().GetSubscriptionsUrl).Result.ToList();
            foreach (var item in subscriptions)
            {
                AppUser user = new AppUser
                {
                    Name = item.Name,
                    Email = item.Email
                };
                users.Add(user);
            }
            email.MessageCategory = "Newsletter";
            email.AppUsers = users;
            new SendUserMessage().SendNewsLetter(email);
            TempData["display"] = "You have successfully sent the Newsletter!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Dashboard", "Home");
        }
        public IActionResult GeneralNotice()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GeneralNotice(UserEmail email)
        {
            var users = new AppUserFactory().GetAllUsers(new AppConfig().FetchUsersUrl).Result.ToList();
            email.MessageCategory = "General";
            email.AppUsers = users;
            new SendUserMessage().SendGeneralNotice(email);
            //display notification
            TempData["display"] = "You have successfully sent the General Notice!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Dashboard", "Home");
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
                var results = (from a in users
                    join b in _databaseConnection.Images.ToList()
                    on a.AppUserId equals b.AppUserId
                    where b != null
                    select a).Distinct().ToList();
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