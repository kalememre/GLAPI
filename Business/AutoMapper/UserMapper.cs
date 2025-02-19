using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Entity;

namespace Business.AutoMapper
{
    public class UserMapper : Profile
    {   
        public UserMapper()
        {
            CreateMap<AuthRequest, User>();
            CreateMap<User, AuthResponse>();
        }
    }
}