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
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.Entity.Name ?? $"{src.Entity.FirstName} {src.Entity.LastName}"))
                .ForMember(dest => dest.EntityDocumentNumber, opt => opt.MapFrom(src => src.Entity.DocumentNumber))
                .ForMember(dest => dest.EntityAddress, opt => opt.MapFrom(src => src.Entity.Address))
                .ForMember(dest => dest.EntityDepartment, opt => opt.MapFrom(src => src.Entity.Ubigeo.Departamento))
                .ForMember(dest => dest.EntityProvince, opt => opt.MapFrom(src => src.Entity.Ubigeo.Provincia))
                .ForMember(dest => dest.EntityDistrict, opt => opt.MapFrom(src => src.Entity.Ubigeo.Distrito))
                .ForMember(dest => dest.EntityCountry, opt => opt.MapFrom(src => src.Entity.Country.Code));

            CreateMap<EntityBranch, EntityBranchListToModalDTO>()
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.Entity.Name == null ? $"{src.Entity.FirstName} {src.Entity.LastName}" : src.Entity.Name))
                .ForMember(dest => dest.EntityDocumentNumber, opt => opt.MapFrom(src => src.Entity.DocumentNumber));

            CreateMap<EntityBranch, EntityBranchListDTO>()
                .ForMember(dest => dest.EntityAddress, opt => opt.MapFrom(src => src.Entity.Address))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Ubigeo.Distrito));

            CreateMap<EntityBranchCreateDTO, EntityBranch>();
            CreateMap<EntityBranchUpdateDTO, EntityBranch>();

            CreateMap<EntityBranch, EstablecimientoRemitenteDTO>()
                .ForMember(dest => dest.EntityAddress, opt => opt.MapFrom(src => src.Entity.Address))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.Entity.Name ?? $"{src.Entity.FirstName} {src.Entity.LastName}"))
                .ForMember(dest => dest.EntityDocumentNumber, opt => opt.MapFrom(src => src.Entity.DocumentNumber))
                .ForMember(dest => dest.EntityCountry, opt => opt.MapFrom(src => src.Entity.Country.Code));

            CreateMap<EntityBranch, EstadoActualizadoResponseDTO>();
        }
    }
}
