﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string bio { get; set; }
        public int rating { get; set; }
        public int user_type { get; set; }
        public bool is_blacklisted { get; set; }
    }
}