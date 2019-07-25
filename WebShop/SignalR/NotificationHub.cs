using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace WebShop.SignalR
{
    public class NotificationHub : Hub
    {


        private readonly IHubContext<NotificationHub> chatHub;

        public NotificationHub(IHubContext<NotificationHub> hubContext)
        {
            chatHub = hubContext;
        }



        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        public void JoinGroup(string groupName, string userFullName)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            // Send data back to everyone including the caller   
            
        }

    }
}
