using System;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Boardgames_webproject.Hubs
{
    public class ChatHub : Hub
    {

        public void Join(string group_id)
        {
            Groups.Add(Context.ConnectionId, group_id);
        }


        public void Send(string group_id, string name, string message)
        {
            Clients.OthersInGroup(group_id).addNewMessageToPage(name, message);
        }
    }
}