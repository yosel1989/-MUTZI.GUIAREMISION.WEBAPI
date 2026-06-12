
using System.ComponentModel;

namespace MUTZI.GUIA.APPLICATION.Enums
{
    public class SunatCatalogoEnum
    {
        public enum MotivoTraslado
        {
            [Description("01")]
            VENTA = 1,

            [Description("02")]
            COMPRA = 2,

            [Description("04")]
            TRASLADO_ENTRE_ESTABLECIMIENTOS_DE_LA_MISMA_EMPRESA = 4,

            [Description("08")]
            IMPORTACION = 8,

            [Description("09")]
            EXPORTACION = 9,

            [Description("13")]
            OTROS = 13,

            [Description("14")]
            VENTA_SUJETA_A_CONFIRMACION_DEL_COMPRADOR = 14,

            [Description("18")]
            TRASLADO_EMISOR_ITINERANTE_DE_CP = 18,

            [Description("19")]
            TRASLADO_A_ZONA_PRIMARIA = 19
        }

    }
}
