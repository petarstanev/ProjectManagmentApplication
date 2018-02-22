using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Comment")]
        public string Content { get; set; }

        [DisplayName("Created date")]
        public DateTime CreatedDate { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int UserId { get; set; }
        public User Author { get; set; }
    }
}