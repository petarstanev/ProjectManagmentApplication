using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.ViewModels
{
    public class ImageViewModel
    {
        public int TaskId { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}