using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class Column
    {
        public int ColumnId { get; set; }
        public string Title { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public List<Task> Tasks { get; set; }
    }
}