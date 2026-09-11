using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionTransportUnit;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingGuiaRemision : AutoMapper.Profile
    {
        public MappingGuiaRemision()
        {

            // Unidades de transporte de la guía de remisión
            CreateMap<GuiaRemisionTransportUnitCreateDTO, GuiaRemisionTransportUnit>();
            CreateMap<GuiaRemisionTransportUnit, GuiaRemisionTransportUnitDTO>();
        }
    }
}
