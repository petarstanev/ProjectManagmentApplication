using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class TeamBoard : Board
    {
        public List<User> Members { get; set; }
    }
}