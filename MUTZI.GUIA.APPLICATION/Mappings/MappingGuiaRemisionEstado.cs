using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingGuiaRemisionEstado : AutoMapper.Profile
    {
        public MappingGuiaRemisionEstado()
        {
            CreateMap<GuiaRemisionEstado, GuiaRemisionEstadoDTO>();
            CreateMap<GuiaRemisionEstado, GuiaRemisionEstadoWithPermisoDTO>()
                .ForMember(dest => dest.Permisos,
                    opt => opt.MapFrom(src =>
                        src.EstadosPermitidosDesde.Select(p => p.GuiaRemisionEstadoIdPermitido)));

        }
    }
}
