using MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingSecurityPerson : AutoMapper.Profile
    {
        public MappingSecurityPerson()
        {
            CreateMap<SecurityPersonCreateDTO, SecurityPerson>();
            CreateMap<SecurityPerson, ToggleActiveResponseDTO>();
            CreateMap<SecurityPerson, SecurityPersonDTO>()
                .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => $"{src.Persona.Nombre} {src.Persona.ApellidoPaterno} {src.Persona.ApellidoMaterno}"))
                ;
        }
    }
}
