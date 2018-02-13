using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [DisplayName("Created by")]
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public User CreatedByUser { get; set; }

        [DisplayName("Assigned to")]
        [ForeignKey("AssignedToUser")]
        public int? AssignedTo { get; set; }
        public User AssignedToUser { get; set; }

        [DisplayName("Column")]
        public int ColumnId { get; set; }
        public Column Column { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Image> Images { get; set; }
        //public Label Label { get; set; }
    }
}