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
    public class LudoHub : Hub
    {
        public void Join(string group_id)
        {
            Groups.Add(Context.ConnectionId, group_id);
        }

        public void MovePawn(string group_id, string pawn_start, string pawn_end, string pawn_id)
        {
            Clients.OthersInGroup(group_id).pawnMoved(pawn_start, pawn_end, pawn_id);
        }

        public void ShowName(string group_id, string turn)
        {
            Clients.OthersInGroup(group_id).hideName(turn);
        }
    }
}