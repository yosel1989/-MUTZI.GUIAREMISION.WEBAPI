using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingConductor : AutoMapper.Profile
    {
        public MappingConductor()
        {
            CreateMap<ConductorGuiaUpdateDTO, GuiaRemisionConductor>();
            CreateMap<Conductor, ConductorDTO>()
                .ForMember(dest => dest.TipoDocumentoNombre, opt => opt.MapFrom(src => src.TipoDocumento.DescripcionCorta));
            CreateMap<Conductor, ConductorListDTO>();
            CreateMap<ConductorCreateDTO, Conductor>()
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres.ToUpper()))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos.ToUpper()))
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Cargo.ToUpper()));
            CreateMap<ConductorUpdateDTO, Conductor>();
            CreateMap<Conductor, EstadoActualizadoResponseDTO>();


            //ConductorGuia
            CreateMap<GuiaRemisionConductor, ConductorGuiaDTO>()
                .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.Conductor.TipoDocumento.DescripcionCorta))
                .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.Conductor.NumeroDocumento))
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Conductor.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Conductor.Apellidos))
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Conductor.Cargo))
                .ForMember(dest => dest.Licencia, opt => opt.MapFrom(src => src.Conductor.Licencia))
                ;
            CreateMap<ConductorCreateGuiaDTO, GuiaRemisionConductor>();
            CreateMap<int, GuiaRemisionConductor>()
            .ForMember(dest => dest.ConductorId, opt => opt.MapFrom(src => src));
        }
    }
}
