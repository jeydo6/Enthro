using AutoMapper;
using Enthro.Application.Models;
using Enthro.Domain.Dto;

namespace Enthro.Application.Mappers
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<LoginDto, LoginModel>()
                .ReverseMap();

            CreateMap<UserInfoDto, UserInfoModel>()
                .ReverseMap();
        }
    }
}
