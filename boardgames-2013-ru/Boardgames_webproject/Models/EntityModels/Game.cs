using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models
{
    public class Game
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int slots { get; set; }
        public int rating { get; set; }
    }
}