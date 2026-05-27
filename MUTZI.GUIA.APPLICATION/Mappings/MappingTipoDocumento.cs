using MUTZI.GUIA.APPLICATION.DTOs.TipoDocumento;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingTipoDocumento : AutoMapper.Profile
    {
        public MappingTipoDocumento()
        {
            CreateMap<TipoDocumento, TipoDocumentoDTO>();
        }
    }
}
