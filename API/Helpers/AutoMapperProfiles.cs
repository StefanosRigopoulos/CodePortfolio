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
                .ForMember(dest => dest.age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.MainPhotoURL, opt => opt.MapFrom(src => src.ProjectPhotos.FirstOrDefault().URL));
            CreateMap<ProjectPhoto, ProjectPhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
        }
    }
}