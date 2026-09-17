using MUTZI.GUIA.APPLICATION.DTOs.EntityBranchSerie;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingEntityBranchSerie : AutoMapper.Profile
    {
        public MappingEntityBranchSerie()
        {
            CreateMap<EntityBranchSerieCreateDTO, EntityBranchSerie>();
            CreateMap<EntityBranchSerie, EntityBranchSerieDTO>()
                .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.InvoiceType.Name))
                ;
        }
    }
}
