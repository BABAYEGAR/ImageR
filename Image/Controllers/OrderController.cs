using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Image.Models;
using Image.Models.APIFactory;
using Image.Models.DataBaseConnections;
using Image.Models.Entities;
using Image.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class OrderController : Controller
    {

        private readonly ImageDataContext _databaseConnection;
        Role _userRole;

        public OrderController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public IActionResult Index()
        {
            try
            {
                var signedInUserId = HttpContext.Session.GetInt32("userId");
                if (HttpContext.Session.GetString("Role") != null)
                {
                    var roleString = HttpContext.Session.GetString("Role");
                    _userRole = JsonConvert.DeserializeObject<Role>(roleString);
                }
                List<Order> orders = new List<Order>();
                if (_userRole.UploadImage)
                {
                    orders = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result
                        .Where(n => n.AppUserId == signedInUserId).ToList();
                }
                if (_userRole.ManageApplicationUser)
                {
                    orders = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result.ToList();
                }
                return View(orders);
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