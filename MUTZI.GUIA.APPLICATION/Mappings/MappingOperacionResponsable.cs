using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.OperacionResponsable;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingOperacionResponsable : AutoMapper.Profile
    {
        public MappingOperacionResponsable()
        {
            CreateMap<OperacionResponsable, OperacionResponsableDTO>();
            CreateMap<OperacionResponsable, OperacionResponsableToSelectDTO>();
            CreateMap<OperacionResponsableCreateDTO, OperacionResponsable>();
            CreateMap<OperacionResponsableUpdateDTO, OperacionResponsable>();
        }
    }
}
