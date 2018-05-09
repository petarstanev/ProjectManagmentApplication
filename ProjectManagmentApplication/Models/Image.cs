using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string Url { get; set; }
        public byte[] Content { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}