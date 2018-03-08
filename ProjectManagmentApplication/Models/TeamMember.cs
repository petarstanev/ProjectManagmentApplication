using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class TeamMember
    {
        public int TeamMemberId { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        [DisplayName("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}