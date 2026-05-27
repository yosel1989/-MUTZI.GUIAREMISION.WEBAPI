using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.Transportista;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingTransportista : AutoMapper.Profile
    {
        public MappingTransportista()
        {
            CreateMap<Transportista, TransportistaDTO>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToUpper()))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.EmailRemiteTransportista, opt => opt.MapFrom(src => src.EmailRemiteTransportistaGuids))
                ;
            CreateMap<TransportistaCreateDTO, Transportista>();
            CreateMap<TransportistaCreateGuiaDTO, Transportista>();
            CreateMap<TransportistaUpdateDTO, Transportista>();

            CreateMap<Transportista, EstadoActualizadoResponseDTO>();
        }
    }
}
