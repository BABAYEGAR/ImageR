using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataManager.Enum;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Image.Controllers
{
    public class AccountController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public AccountController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(AccountModel model)
        {
            AppUser appUser = new AppUser
            {
                Name = model.Username,
                Mobile = model.Mobile,
                Email = model.Email,
                Password = new Hashing().HashPassword(model.ConfirmPassword)
            };
            appUser.ConfirmPassword = appUser.Password;
            appUser.DateCreated = DateTime.Now;
            appUser.DateLastModified = DateTime.Now;
            appUser.RoleId = 2;


            _databaseConnection.AppUsers.Add(appUser);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully registered to Image R, Login and start uploading your Images!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
    }
}