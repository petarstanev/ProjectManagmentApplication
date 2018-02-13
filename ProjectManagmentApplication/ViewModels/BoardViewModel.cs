using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.ViewModels
{
    [NotMapped]
    public class BoardViewModel
    {
        public Board SelectedBoard { get; set; }
        public List<Board> AllBoards { get; set; }
    }
}