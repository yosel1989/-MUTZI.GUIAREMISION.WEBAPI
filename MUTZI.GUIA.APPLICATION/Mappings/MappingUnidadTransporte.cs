using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingUnidadTransporte : AutoMapper.Profile
    {
        public MappingUnidadTransporte()
        {
            CreateMap<UnidadTransporte, UnidadTransporteDTO>()
                .ForMember(dest => dest.EntidadReguladoraVehicularNombre, opt => opt.MapFrom(src => src.EntidadReguladoraVehicular.DescripcionCorta));
            
            CreateMap<UnidadTransporte, UnidadTransporteListDTO>();
            CreateMap<UnidadTransporteCreateDTO, UnidadTransporte>();
            CreateMap<UnidadTransporteUpdateDTO, UnidadTransporte>();
            CreateMap<UnidadTransporte, EstadoActualizadoResponseDTO>();
        }
    }
}
