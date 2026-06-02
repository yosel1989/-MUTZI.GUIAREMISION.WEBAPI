using MUTZI.GUIA.APPLICATION.DTOs.Persona;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingPersona : AutoMapper.Profile
    {
        public MappingPersona()
        {
            CreateMap<Persona, PersonaDTO>();
        }
    }
}
