using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingUnidadTransporte : AutoMapper.Profile
    {
        public MappingUnidadTransporte()
        {
            CreateMap<TransportUnit, TransportUnitDTO>()
                .ForMember(dest => dest.EntidadReguladoraVehicularNombre, opt => opt.MapFrom(src => src.EntidadReguladoraVehicular.DescripcionCorta));
            
            CreateMap<TransportUnit, TransportUnitListDTO>();
            CreateMap<TransportUnitCreateDTO, TransportUnit>();
            CreateMap<TransportUnitUpdateDTO, TransportUnit>();
            CreateMap<TransportUnit, EstadoActualizadoResponseDTO>();
        }
    }
}
