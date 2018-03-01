using System.Collections.Generic;
using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.ViewModels
{
    public class HomePageBoards
    {
        public List<Board> Favourite { get; set; }
        public List<Board> PrivateBoards { get; set; }
        public List<Board> TeamBoards { get; set; }
        public List<Board> PublicBoards { get; set; }
    }
}