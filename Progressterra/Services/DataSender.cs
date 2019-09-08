using Microsoft.AspNetCore.SignalR;
using Progressterra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Services
{
    public class DataSender : Hub
    {
        public DataSender()
        {
            
        }

        //public void Send(string message)
        //{
        //    if (Clients == null) return;
        //    Clients.All.SendAsync("broadcastMessage", message);
        //}
    }
}
