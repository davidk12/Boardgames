using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boardgames_webproject.Models;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Boardgames_webproject.Controllers
{
    public class TicTacToeHub : Hub
    {
        public void Join(string group_id)
        {
            Groups.Add(Context.ConnectionId, group_id);
        }

        public void ClickCell(string group_id, string cell_id)
        {
            Clients.OthersInGroup(group_id).cellClicked(cell_id);
        }
    }
}
