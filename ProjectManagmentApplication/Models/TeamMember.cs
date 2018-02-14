using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class TeamMember
    {
        public int TeamMemberId { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}