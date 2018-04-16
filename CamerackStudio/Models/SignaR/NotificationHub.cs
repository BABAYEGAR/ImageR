using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Entities;
using Microsoft.AspNetCore.SignalR;

namespace CamerackStudio.Models.SignaR
{
    public class NotificationHub  : Hub
    {
        private readonly CamerackStudioDataContext _databaseConnection;
        private readonly List<PushNotification> _pushNotifications;
        public NotificationHub(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _pushNotifications = new AppUserFactory().GetAllPushNotifications(new AppConfig().UsersPushNotifications).Result;
        }
        public Task GetNotification()
        {
            var notifications = _pushNotifications.ToList();
            return Clients.All.InvokeAsync("GetNotification", notifications);
        }
    }
}
