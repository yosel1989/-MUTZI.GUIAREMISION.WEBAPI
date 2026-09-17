using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingCatalogo : AutoMapper.Profile
    {
        public MappingCatalogo()
        {
            // Entidad Reguladora
            CreateMap<EntidadReguladora, EntidadReguladoraDTO>();
            CreateMap<EntidadReguladora, EntidadReguladoraToSelectDTO>();

            // Unidad de Medida
            CreateMap<UnitMeasurement, UnidadMedidaDTO>();
            CreateMap<UnitMeasurement, UnidadMedidaToSelectDTO>();


            // Bien Normalizado
            CreateMap<BienNormalizado, BienNormalizadoDTO>();
            CreateMap<BienNormalizado, BienNormalizadoToSelectDTO>();


            // Documento Relacionado
            CreateMap<DocumentoRelacionado, DocumentoRelacionadoDTO>();
            CreateMap<DocumentoRelacionado, DocumentoRelacionadoToSelectDTO>();

            // Indicador de Traslado
            CreateMap<GuiaRemisionIndicadorTraslado, GuiaRemisionIndicadorTrasladoToSelectDTO>();

            // Paises
            CreateMap<Country, CountryDTO>();

            // Tipos de documentos comerciales
            CreateMap<InvoiceType, InvoiceTypeDTO>();
            CreateMap<InvoiceType, InvoiceTypeToSelectDTO>();

        }
    }
}
