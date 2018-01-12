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
    public class OrderController : Controller
    {

        private readonly CamerackStudioDataContext _databaseConnection;
        AppUser _appUser;
        private List<PushNotification> pushNotifications;

        public OrderController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            pushNotifications = new AppUserFactory().GetAllPushNotifications(new AppConfig()
                .UsersPushNotifications).Result.Where(n => n.ClientId == new AppConfig().ClientId).ToList();
        }
        [SessionExpireFilter]
        public async Task<IActionResult> Index(long? notificationId)
        {
            try
            {
                var signedInUserId = Convert.ToInt64(new RedisDataAgent().GetStringValue("CamerackLoggedInUserId"));

                //update notification to read
                if (notificationId != null)
                {
                    var notification = pushNotifications.SingleOrDefault(n => n.PushNotificationId == notificationId);

                    if (notification != null)
                    {
                        notification.Read = true;
                        notification.DateLastModified = DateTime.Now;
                        notification.LastModifiedBy = signedInUserId;
                        await new AppUserFactory().UpdatePushNotification(new AppConfig().UpdatePushNotifications, notification);
                    }
                }


                if (new RedisDataAgent().GetStringValue("CamerackLoggedInUser") != null)
                {
                    var userString = new RedisDataAgent().GetStringValue("CamerackLoggedInUser");
                    _appUser = JsonConvert.DeserializeObject<AppUser>(userString);
                }
                List<Order> orders = new List<Order>();
                if (_appUser.Role.UploadImage)
                {
                    orders = new OrderFactory().GetAllOrdersAsync(new AppConfig().FetchOrdersUrl).Result
                        .Where(n => n.AppUserId == signedInUserId).ToList();
                }
                if (_appUser.Role.ManageImageCategory)
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