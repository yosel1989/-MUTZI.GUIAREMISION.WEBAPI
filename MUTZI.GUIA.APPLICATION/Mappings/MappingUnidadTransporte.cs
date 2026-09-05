using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingUnidadTransporte : AutoMapper.Profile
    {
        public MappingUnidadTransporte()
        {
            CreateMap<TransportUnit, UnidadTransporteDTO>()
                .ForMember(dest => dest.EntidadReguladoraVehicularNombre, opt => opt.MapFrom(src => src.EntidadReguladoraVehicular.DescripcionCorta));
            
            CreateMap<TransportUnit, UnidadTransporteListDTO>();
            CreateMap<UnidadTransporteCreateDTO, TransportUnit>();
            CreateMap<UnidadTransporteUpdateDTO, TransportUnit>();
            CreateMap<TransportUnit, EstadoActualizadoResponseDTO>();
        }
    }
}
