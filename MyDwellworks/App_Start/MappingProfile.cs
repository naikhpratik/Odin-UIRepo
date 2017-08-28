using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyDwellworks.Data.Core.Dtos;
using MyDwellworks.Data.Core.Models;

namespace MyDwellworks.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}