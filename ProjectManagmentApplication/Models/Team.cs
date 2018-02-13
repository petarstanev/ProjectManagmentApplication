using System.Collections.Generic;

namespace ProjectManagementApplication.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public List<User> Members { get; set; }
        public List<Board> Boards { get; set; }
    }
}