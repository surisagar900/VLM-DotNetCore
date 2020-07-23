using AutoMapper;
using VLM.Core.Entities;
using VLM.Core.Models;

namespace VLM.Core
{
    public class VLMProfile : Profile
    {
        public VLMProfile()
        {
            
            CreateMap<Users, UserDTO>().ReverseMap();

            CreateMap<Records, RecordsDTO>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName))
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => $"{s.User.FirstName.Trim()} {s.User.LastName.Trim()}"))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.User.Phone))
                .ForMember(d => d.EmailAddress, opt => opt.MapFrom(s => s.User.Email));

            //CreateMap<Records, UserRecordDTO>().ReverseMap()
            //    .ForMember(t => t.Movies, opt => opt.Ignore())
            //    .ForMember(t => t.User, opt => opt.Ignore());

            CreateMap<Records, UserRecordDTO>()
                .ForMember(t => t.MovieTitle, opt => opt.MapFrom(r => r.Movies.Title));
        }
    }
}
