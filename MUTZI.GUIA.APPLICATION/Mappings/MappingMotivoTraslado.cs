using MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingMotivoTraslado : AutoMapper.Profile
    {
        public MappingMotivoTraslado()
        {
            CreateMap<MotivoTraslado, MotivoTrasladoDTO>();
            CreateMap<MotivoTraslado, MotivoTrasladoToSelectDTO>();
        }
    }
}
