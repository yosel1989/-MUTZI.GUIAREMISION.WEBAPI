using MUTZI.GUIA.APPLICATION.DTOs.Entity;
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
            CreateMap<EntityBranchSerie, ToggleActiveResponseDTO>();
            CreateMap<EntityBranchSerie, EntityBranchSerieDTO>()
                .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.InvoiceType.Name))
                ;
            CreateMap<EntityBranchSerie, EntityBranchSerieToSelectDTO>()
                .ForMember(dest => dest.EntityDocumentNumber, opt => opt.MapFrom(src => src.EntityBranch.Entity.DocumentNumber))
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityBranch.Entity.Name))
                .ForMember(dest => dest.EntityBranchAddress, opt => opt.MapFrom(src => src.EntityBranch.Address))
                .ForMember(dest => dest.EntityBranchAlias, opt => opt.MapFrom(src => src.EntityBranch.Description))
                ;

            CreateMap<SecurityPersonEntityBranchSerie, EntityBySerieAssignedDTO>()
                .ForMember(d => d.Entity, o => o.MapFrom(s => s.EntityBranchSerie.EntityBranch.Entity))
                .ForMember(d => d.EntityBranch, o => o.MapFrom(s => s.EntityBranchSerie.EntityBranch))
                .ForMember(d => d.EntityBranchSerie, o => o.MapFrom(s => s.EntityBranchSerie));

            CreateMap<Entity, EntityBySerieAssigned_EntityDTO>()
                .ForMember(d => d.EntityId, o => o.MapFrom(s => s.EntityId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.DocumentNumber, o => o.MapFrom(s => s.DocumentNumber))
                .ForMember(d => d.UbigeoId, o => o.MapFrom(s => s.UbigeoId))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Department, o => o.MapFrom(s => s.Ubigeo.Departamento))
                .ForMember(d => d.Province, o => o.MapFrom(s => s.Ubigeo.Provincia))
                .ForMember(d => d.District, o => o.MapFrom(s => s.Ubigeo.Distrito))
                //.ForMember(d => d.CountryId, o => o.MapFrom(s => s.CountryId))
                .ForMember(d => d.MtcCode, o => o.MapFrom(s => s.MtcCode));

            CreateMap<EntityBranch, EntityBySerieAssigned_EntityBranchDTO>()
                .ForMember(d => d.EntityBranchId, o => o.MapFrom(s => s.EntityBranchId))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.CodeSunat, o => o.MapFrom(s => s.CodeSunat))
                .ForMember(d => d.UbigeoId, o => o.MapFrom(s => s.UbigeoId))
                .ForMember(d => d.Department, o => o.MapFrom(s => s.Ubigeo.Departamento))
                .ForMember(d => d.Province, o => o.MapFrom(s => s.Ubigeo.Provincia))
                .ForMember(d => d.District, o => o.MapFrom(s => s.Ubigeo.Distrito));

            CreateMap<EntityBranchSerie, EntityBySerieAssigned_EntityBranchSerieDTO>()
                .ForMember(d => d.EntityBranchSerieId, o => o.MapFrom(s => s.EntityBranchSerieId))
                .ForMember(d => d.Serie, o => o.MapFrom(s => s.Serie))
                .ForMember(d => d.Area, o => o.MapFrom(s => s.Area));
        }
    }
}
