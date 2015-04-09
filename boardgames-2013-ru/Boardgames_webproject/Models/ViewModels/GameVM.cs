using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models
{
    public class GameVM
    {
        public Game specific_game {get; set;}
        public Game highest_rated_game { get; set; }
        public List<Game> game_list = null;
        public List<Achievement> game_achievements = null;
        public List<UserRanking> user_rating_list = null;
        public List<GameInstance> active_games_list = null;

        public int current_user_id = -1;
    }
}