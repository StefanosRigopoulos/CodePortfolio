using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.ProfilePhotoURL, opt => opt.MapFrom(src => src.Photo.URL));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.MainPhotoURL, opt => opt.MapFrom(src => src.ProjectPhotos.FirstOrDefault(x => x.IsMain).URL));
            CreateMap<ProjectPhoto, ProjectPhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));    // Exclude the fields that come in as null.
            CreateMap<ProjectUpdateDTO, Project>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.ProjectTitle.ToLower().Replace(" ","_")))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));    // Exclude the fields that come in as null.
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Like, LikeDTO>();
        }
    }
}