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
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            //qui specifico da dove a dove voglio mappare
            //in questo caso voglio partire da AppUser e finire in MembersDto
            //src => src.Photos.FirstOrDefault(x => x.IsMain).Url qui gli sto dicento
            //in AppUser alla prprietà Photos (che è una lista di foto) cerca la foto che ha
            //il valore IsMain settato a true e prendi l'url
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
        
        }
    }
}