using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementApplication.Models
{
    public class Board
    {
        public int BoardId { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }
        public List<Column> Columns { get; set; }
        
        [Display(Name = "Administrator")]
        public int? UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "Board Type")]
        public BoardType BoardType { get; set; }
    }
}