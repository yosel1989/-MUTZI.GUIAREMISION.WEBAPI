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
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.Entity.Name ?? $"{src.Entity.FirstName} {src.Entity.LastName}"))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.Entity.DocumentNumber))
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Entity.Country.Code));

            CreateMap<EntityBranch, EntityBranchListToModalDTO>()
                .ForMember(dest => dest.Entidad, opt => opt.MapFrom(src => src.Entity.Name == null ? $"{src.Entity.FirstName} {src.Entity.LastName}" : src.Entity.Name));

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
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.Entity.Name ?? $"{src.Entity.FirstName} {src.Entity.LastName}"))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.Entity.DocumentNumber));

            CreateMap<EntityBranch, EstadoActualizadoResponseDTO>();
        }
    }
}
