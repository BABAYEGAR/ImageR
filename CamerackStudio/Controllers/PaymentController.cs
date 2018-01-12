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
    public class PaymentController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private AppUser _appUser;
        private List<PushNotification> pushNotifications;

        public PaymentController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            pushNotifications = new AppUserFactory().GetAllPushNotifications(new AppConfig()
                .UsersPushNotifications).Result.Where(n=>n.ClientId == new AppConfig().ClientId).ToList();
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
                var payments = new List<Payment>();
                if (_appUser.Role.UploadImage)
                    payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result
                        .Where(n => n.AppUserId == signedInUserId).ToList();
                if (_appUser.Role.ManageApplicationUser)
                    payments = new OrderFactory().GetAllPaymentsAsync(new AppConfig().FetchPaymentsUrl).Result.ToList();
                return View(payments);
            }
            catch (Exception)
            {
                //display notification
                TempData["display"] =
                    "An error ocurred while fetching your payments check your internet connectivity and try again!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return View();
            }
        }

        public async Task<IActionResult> ApprovePayment(long id)
        {
            try
            {
                var response = await new OrderFactory().ApprovePaymentsAsync(new AppConfig().ApprovePaymentsUrl, id);
                if (response.PaymentId > 0)
                {
                    //display notification
                    TempData["display"] ="You have successfuly Paid the User the Fees Due";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //display notification
                TempData["display"] = "There was an issue trying to pay the fees";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return RedirectToAction("Index");
            }
        }
    }
}