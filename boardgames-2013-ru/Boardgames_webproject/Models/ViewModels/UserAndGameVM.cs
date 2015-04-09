using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models
{
    public class UserAndGameVM
    {
        public Game specific_game { get; set; }
        public Game highest_rated_game { get; set; }
        public List<Game> game_list { get; set; }
        public List<Achievement> game_achievements { get; set; }
        public List<User> user_list { get; set; }
        public User specific_user { get; set; }
        public User highest_rated_user { get; set; }
        public string error_message = null;
        public List<GameInstance> active_games_list = null;
        public List<UserRanking> user_score_list = null;
        public UserRanking specific_user_scores = new UserRanking();

        public int current_user_id = -1;
    }
}