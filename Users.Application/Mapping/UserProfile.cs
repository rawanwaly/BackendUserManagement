using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using UserManagement.Domain.Core.Models;
using AutoMapper;
using UserManagement.Application.Dtos.UsersDtos;
namespace UserManagement.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserGetDto>()
                .ForMember(dest => dest.MaritalStatusName,
                           opt => opt.MapFrom(src => src.MaritalStatus.ToString()));

            CreateMap<UserPostDto, User>();
            CreateMap<UserPutDto, User>();
        }
    }
}
