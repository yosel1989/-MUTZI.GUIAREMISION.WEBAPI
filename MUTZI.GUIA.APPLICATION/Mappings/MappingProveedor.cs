using MUTZI.GUIA.APPLICATION.DTOs.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingProveedor : AutoMapper.Profile
    {
        public MappingProveedor()
        {
            CreateMap<Proveedor, ProveedorDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<Proveedor, ProveedorListDTO>();
            CreateMap<ProveedorCreateDTO, Proveedor>();
            CreateMap<ProveedorCreateGuiaDTO, Proveedor>();
            CreateMap<ProveedorUpdateDTO, Proveedor>();
            CreateMap<Proveedor, EstadoActualizadoResponseDTO>();
        }
    }
}
