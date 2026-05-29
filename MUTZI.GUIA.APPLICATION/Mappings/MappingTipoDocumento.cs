using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingTipoDocumento : AutoMapper.Profile
    {
        public MappingTipoDocumento()
        {
            CreateMap<TipoDocumento, TipoDocumentoDTO>();
            CreateMap<TipoDocumento, TipoDocumentoToSelectDTO>();
        }
    }
}
