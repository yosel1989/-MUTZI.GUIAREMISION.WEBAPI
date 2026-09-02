using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingEntityBranch : AutoMapper.Profile
    {
        public MappingEntityBranch()
        {
            CreateMap<EntityBranch, EntityBranchToSelectDTO>();
            CreateMap<EntityBranch, EntityBranchDTO>()
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RucNavigation.Empresa));

            CreateMap<EntityBranch, EntityBranchListToModalDTO>()
                .ForMember(dest => dest.Entidad, opt => opt.MapFrom(src => src.RucNavigation.Empresa));

            CreateMap<EntityBranch, EntityBranchListDTO>()
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito));

            CreateMap<EntityBranchCreateDTO, EntityBranch>();
            CreateMap<EntityBranchUpdateDTO, EntityBranch>();

            CreateMap<EntityBranch, EstablecimientoRemitenteDTO>()
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RucNavigation.Empresa));

            CreateMap<EntityBranch, EstadoActualizadoResponseDTO>();
        }
    }
}
