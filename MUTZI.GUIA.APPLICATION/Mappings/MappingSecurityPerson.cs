using MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranch;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie;
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
                .ForMember(dest => dest.PersonDocumentNumber, opt => opt.MapFrom(src => src.Persona.NumeroDocumento))
                .ForMember(dest => dest.PersonRole, opt => opt.MapFrom(src => src.Persona.Cargo))
                .ForMember(
                    dest => dest.Series,
                    opt => opt.MapFrom(src =>
                        src.SecurityPersonEntityBranchSeries.Select(x => new SecurityPersonSeriesDTO {
                            Serie = x.EntityBranchSerie.Serie,
                            EntityName = x.EntityBranchSerie.EntityBranch.Entity.Name ?? "",
                            EntityDocumentNumber = x.EntityBranchSerie.EntityBranch.Entity.DocumentNumber
                        })
                    )
                )
                .ForMember(
                    dest => dest.EntityBranches,
                    opt => opt.MapFrom(src =>
                        src.SecurityPersonEntityBranchs.Select(x => new SecurityPersonEntityBranchsDTO
                        {
                            Alias = x.EntityBranch.Description,
                            EntityName = x.EntityBranch.Entity.Name ?? "",
                            EntityDocumentNumber = x.EntityBranch.Entity.DocumentNumber
                        })
                    )
                )
                ;
            

            CreateMap<SecurityPersonEntityBranchCreateDTO, SecurityPersonEntityBranch>();
            CreateMap<SecurityPersonEntityBranchDeleteDTO, SecurityPersonEntityBranch>();
            CreateMap<SecurityPersonEntityBranch, SecurityPersonEntityBranchDTO>()
                .ForMember(dest => dest.EntityBranchAddress, opt => opt.MapFrom(src => src.EntityBranch.Address))
                .ForMember(dest => dest.EntityBranchAlias, opt => opt.MapFrom(src => src.EntityBranch.Description))
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityBranch.Entity.Name))
                .ForMember(dest => dest.EntityDocumentNumber, opt => opt.MapFrom(src => src.EntityBranch.Entity.DocumentNumber))
                ;

            CreateMap<SecurityPersonEntityBranchSerieCreateDTO, SecurityPersonEntityBranchSerie>();
            CreateMap<SecurityPersonEntityBranchSerieDeleteDTO, SecurityPersonEntityBranchSerie>();
            CreateMap<SecurityPersonEntityBranchSerie, SecurityPersonEntityBranchSerieDTO>()
                .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.EntityBranchSerie.Serie))
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityBranchSerie.EntityBranch.Entity.Name))
                .ForMember(dest => dest.EntityDocumentNumber, opt => opt.MapFrom(src => src.EntityBranchSerie.EntityBranch.Entity.DocumentNumber))
                .ForMember(dest => dest.EntityBranchAlias, opt => opt.MapFrom(src => src.EntityBranchSerie.EntityBranch.Description))
                .ForMember(dest => dest.EntityBranchAddress, opt => opt.MapFrom(src => src.EntityBranchSerie.EntityBranch.Address))
                ;
        }
    }
}
