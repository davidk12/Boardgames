﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boardgames_webproject.Models
{
    public class UsersVM
    {
        public List<User> user_list { get; set; }
        public User specific_user { get; set; }

        public int current_user_id = -1;
    }
}