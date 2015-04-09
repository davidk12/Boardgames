using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models.ViewModels
{
    public class ActiveUserListVM
    {

        public List<User> user_list = null;
        public bool is_game_full { get; set; }
    }
}