using System;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Encryption;
using Image.Models.Entities;
using Image.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class AccountController : Controller
    {
        private readonly ImageDataContext _databaseConnection;

        public AccountController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public ActionResult Profile()
        {
            return View();
        }
        public ActionResult Register()
        {
            ViewBag.PhotographerCategoryId = new SelectList(_databaseConnection.PhotographerCategories.ToList(), "PhotographerCategoryId",
                "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountModel model)
        {
            var appUser = new AppUser
            {
                Name = model.Username,
                Mobile = model.Mobile,
                Email = model.Email,
                Password = new Hashing().HashPassword(model.ConfirmPassword),
                Username = model.Username
            };
            appUser.ConfirmPassword = appUser.Password;
            appUser.DateCreated = DateTime.Now;
            appUser.DateLastModified = DateTime.Now;
            appUser.RoleId = 2;
            appUser.PhotographerCategoryId = model.PhotographerCategoryId;


            _databaseConnection.AppUsers.Add(appUser);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully registered to Image R, Login and start uploading your Images!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login");
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
            var userExist =
                _databaseConnection.AppUsers.SingleOrDefault(
                    n => n.Email == model.Email || n.Username == model.Username);
            if (userExist == null)
            {
                //display notification
                TempData["display"] = "Your Email does not exist, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return View(model);
            }
            var passwordCorrect = new Hashing().ValidatePassword(model.Password, userExist.ConfirmPassword);
            if (passwordCorrect == false)
            {
                //display notification
                TempData["display"] = "Dear " + userExist.Name + " your Password is Incorrect, Check and Try again!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return View(model);
            }

            var role = _databaseConnection.Roles.Find(userExist.RoleId);
            //convert object to json string and insert into session
            var userString = JsonConvert.SerializeObject(userExist);
            HttpContext.Session.SetString("ImageLoggedInUser", userString);

            //convert object to json string and insert into session
            var roleString = JsonConvert.SerializeObject(role);
            HttpContext.Session.SetString("Role", roleString);

            return RedirectToAction("Dashboard", "Home");
        }
        [HttpGet]
        public IActionResult LogOut()
        {
            _databaseConnection.Dispose();
            return RedirectToAction("Login", "Account");
        }
    }
}