using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Models
{
    public class Board
    {
        public int BoardId { get; set; }

        [Required]
        [StringLength(15)]
        public string Title { get; set; }
        public List<Column> Columns { get; set; }
    }
}