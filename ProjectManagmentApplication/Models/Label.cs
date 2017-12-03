using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagmentApplication.Models
{
    public class Label
    {
        public int LabelId { get; set; }
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
    }
}