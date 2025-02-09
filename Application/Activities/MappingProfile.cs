using AutoMapper;
using Domain;

namespace Application.Activities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName));
        }
    }
}