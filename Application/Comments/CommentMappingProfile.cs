using AutoMapper;
using Domain;

namespace Application.Comments
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(c => c.UserName, opt => opt.MapFrom(s => s.Author.UserName))
                .ForMember(c => c.DisplayName, opt => opt.MapFrom(s => s.Author.DisplayName));

        }
    }
}