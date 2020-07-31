using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace EIMS
{
    [HubName("CommentHub")]
    public class CommentHub : Hub
    {

        public void Send(string userid)
        {
       
            Clients.All.notify(userid);
        }
    }
}