using MUTZI.GUIA.APPLICATION.DTOs.UnidadMedida;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingUnidadMedida : AutoMapper.Profile
    {
        public MappingUnidadMedida()
        {
            CreateMap<UnidadMedida, UnidadMedidaDTO>();
            CreateMap<UnidadMedida, UnidadMedidaToSelectDTO>();
        }
    }
}
