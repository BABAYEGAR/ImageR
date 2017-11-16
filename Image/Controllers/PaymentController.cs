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
    public class PaymentController : Controller
    {

        private readonly ImageDataContext _databaseConnection;
        Role _userRole;

        public PaymentController(ImageDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public IActionResult Index()
        {
            try { 
            var signedInUserId = HttpContext.Session.GetInt32("userId");
            if (HttpContext.Session.GetString("Role") != null)
            {
                var roleString = HttpContext.Session.GetString("Role");
                _userRole = JsonConvert.DeserializeObject<Role>(roleString);
            }
            List<Payment> payments = new List<Payment>();
            if (_userRole.UploadImage)
            {
                payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result
                    .Where(n => n.AppUserId == signedInUserId).ToList();
            }
            if (_userRole.ManageApplicationUser)
            {
                payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result.ToList();
            }
            return View(payments);
        }
        catch (Exception)
        {
            //display notification
            TempData["display"] = "An error ocurred while fetching your payments check your internet connectivity and try again!";
            TempData["notificationtype"] = NotificationType.Error.ToString();
            return View();
        }
}

    }

}