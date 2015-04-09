using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models
{
    public class UserRanking
    {
        public string username { get; set; }
        public int total_games { get; set; }
        public int total_wins { get; set; }
        public int total_losses { get; set; }
        public int total_ties { get; set; }
    }
}