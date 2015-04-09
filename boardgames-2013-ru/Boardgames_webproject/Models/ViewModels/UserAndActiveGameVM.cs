using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models
{
    public class UserAndActiveGameVM
    {
        public GameInstance specific_game_instance { get; set; }
        public List<GameInstance> instance = null;
        public User owner { get; set; }
        public User specific_user { get; set; }
        public List<User> user_list { get; set; }
        public List<Achievement> game_achievement_list { get; set; }
        public List<UserRanking> user_rating_list { get; set; }
        public int current_user_id { get; set; }
    }
}