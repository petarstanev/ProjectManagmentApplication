using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ProjectManagmentApplication.Hubs
{
    public class TaskHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}