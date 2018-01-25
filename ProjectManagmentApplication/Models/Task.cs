using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagmentApplication.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }
        public bool Private { get; set; }

        public User CreatedBy { get; set; }
        public User AssignedTo { get; set; }
        public int ColumnId { get; set; }
        public Column Column { get; set; }
        public List<Comment> Comments { get; set; }
        //public List<Image> Images { get; set; }
        //public Label Label { get; set; }
    }
}