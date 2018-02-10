using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        [Required]
        [StringLength(15)]
        public string Title { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }
        public bool Private { get; set; }

        [DisplayName("Created by")]
        public User CreatedBy { get; set; }

        [DisplayName("Assigned to")]
        public User AssignedTo { get; set; }

        [DisplayName("Column")]
        public int ColumnId { get; set; }
        public Column Column { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Image> Images { get; set; }
        //public Label Label { get; set; }
    }
}