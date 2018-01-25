using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagmentApplication.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int UserId { get; set; }
        public User Author { get; set; }
    }
}