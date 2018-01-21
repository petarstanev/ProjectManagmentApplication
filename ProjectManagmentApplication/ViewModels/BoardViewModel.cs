using System.Collections.Generic;
using ProjectManagmentApplication.Models;

namespace ProjectManagmentApplication.ViewModels
{
    public class BoardViewModel : Board
    {
        public Board SelectedBoard { get; set; }
        public List<Board> AllBoards { get; set; }
    }
}