using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;

namespace Lily.BusinessLogic.Mapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile() 
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserDetailsDTO>().ReverseMap();
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<UserTask, UserTaskDTO>().ReverseMap();
            CreateMap<Notification, NotificationDTO>().ReverseMap();
         
           
        }
    }
}
