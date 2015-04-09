using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models.EntityModels
{
    public class GameAnnouncement
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime time_stamp { get; set; }
        public int game_id { get; set; }
    }
}