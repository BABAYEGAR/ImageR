using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CamerackStudio.Models.SignaR
{
    public class NotificationHub  : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}
