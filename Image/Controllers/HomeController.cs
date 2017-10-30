using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Image.Models;
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
        List<Models.Entities.Image> _images = new List<Models.Entities.Image>();
        List<Models.Entities.Location> _locations = new List<Models.Entities.Location>();
        List<Models.Entities.Camera> _cameras = new List<Models.Entities.Camera>();

        public HomeController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public IActionResult Index()
        {
            ViewBag.Packages = _databaseConnection.Packages.Include(n=>n.PackageItems).ToList();
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
            }
            //ViewBag.AppUsers = _databaseConnection.AppUsers.ToList();
            ViewBag.Competition = _databaseConnection.Competition.ToList();
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
