using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class Column
    {
        public int ColumnId { get; set; }

        [Required]
        [StringLength(15)]
        public string Title { get; set; }

        [DisplayName("Board")]
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public List<Task> Tasks { get; set; }
    }
}