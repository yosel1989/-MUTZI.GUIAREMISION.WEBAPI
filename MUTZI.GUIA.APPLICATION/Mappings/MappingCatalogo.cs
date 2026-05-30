using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingCatalogo : AutoMapper.Profile
    {
        public MappingCatalogo()
        {
            CreateMap<EntidadReguladora, EntidadReguladoraDTO>();
            CreateMap<EntidadReguladora, EntidadReguladoraToSelectDTO>();
        }
    }
}
