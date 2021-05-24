using AutoMapper;
using NeHauar.DTO;
using NeHauar.Models;

namespace NeHauar.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<ChatDto, Chat>();
        }
    }
}