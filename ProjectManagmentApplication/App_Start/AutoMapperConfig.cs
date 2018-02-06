using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.ViewModels;

namespace ProjectManagementApplication.App_Start
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, EditUser>().ReverseMap();
        }
    }
}