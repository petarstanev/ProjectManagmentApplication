using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementApplication.Models
{
    public abstract class Board
    {
        public int BoardId { get; set; }

        [Required]
        [StringLength(15)]
        public string Title { get; set; }
        public List<Column> Columns { get; set; }
        
        [ForeignKey("Administrator")]
        public int? UserId { get; set; }
        public User Administrator { get; set; }
    }
}