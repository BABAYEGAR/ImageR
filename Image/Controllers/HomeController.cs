using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Image.Models;
using Image.Models.APIFactory;
using Image.Models.DataBaseConnections;
using Image.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageDataContext _databaseConnection;
        Role _userRole;

        public HomeController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _databaseConnection.Database.EnsureCreated();


        }
        public IActionResult Index()
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
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            var notifications = _databaseConnection.SystemNotifications.Where(n => n.AppUserId == signedInUserId)
                .ToList();
            return PartialView("Partials/_NotificationPartial",notifications);
        }
        public IActionResult Dashboard()
        {
            var signedInUserId = HttpContext.Session.GetInt32("userId");

            if (HttpContext.Session.GetString("Role") != null)
            {
                var roleString = HttpContext.Session.GetString("Role");
                _userRole = JsonConvert.DeserializeObject<Role>(roleString);
            }
            if (_userRole.ManageImages || _userRole.ManageApplicationUser)
            {
                ViewBag.Images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).ToList();
                ViewBag.Cameras = _databaseConnection.Cameras.ToList();
                ViewBag.Locations = _databaseConnection.Locations.ToList();
                ViewBag.Orders = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result.ToList();
                ViewBag.Payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result.ToList();
            }
            if (_userRole.UploadImage)
            {

                ViewBag.Images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                    .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory)
                    .Where(n => n.AppUserId == signedInUserId).ToList();
                ViewBag.Cameras = _databaseConnection.Cameras
                    .Where(n => n.CreatedBy == signedInUserId).ToList();
                ViewBag.Locations = _databaseConnection.Locations
                    .Where(n => n.CreatedBy == signedInUserId).ToList();
                var result = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result;
                if (result != null)
                {
                    ViewBag.Orders = result
                        .Where(n => n.CreatedBy == signedInUserId).ToList();
             
                }
                var payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result;
                if (payments != null)
                    ViewBag.Payments = payments
                        .Where(n => n.CreatedBy == signedInUserId).ToList();
            }
            ViewBag.AppUsers = new AppUserFactory().GetAllUsersAsync(new AppConfig().FetchUsersUrl).Result.ToList();
            return View();
        }
    }
}
