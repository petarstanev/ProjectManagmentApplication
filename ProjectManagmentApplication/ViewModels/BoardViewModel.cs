using System.Collections.Generic;
using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.ViewModels
{
    public class BoardViewModel : Board
    {
        public Board SelectedBoard { get; set; }
        public List<Board> AllBoards { get; set; }
    }
}