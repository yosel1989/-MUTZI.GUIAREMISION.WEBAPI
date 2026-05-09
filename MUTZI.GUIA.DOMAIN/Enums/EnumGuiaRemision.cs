using System.ComponentModel;
namespace MUTZI.GUIA.DOMAIN.Enums
{
    public enum EnumGuiaRemisionEstado
    {
        [Description("registrada")]
        Registrada = 0,

        [Description("editada")]
        Editada = 1,

        [Description("confirmada")]
        Confirmada = 2,

        [Description("aprobada")]
        Aprobada = 3,

        [Description("emitida")]
        Emitida = 4,

        [Description("confirmada_salida_despacho")]
        ConfirmadaSalidaDespacho = 5,

        [Description("en_transito")]
        EnTransito = 6,

        [Description("registrada")]
        ConfirmadaEntregaRecepcion = 7,

        [Description("confirmada_recepcion")]
        ConfirmadaRecepcion = 8,

        [Description("productos_validados_recepcion")]
        ProductosValidadosRecepcion = 9,

        [Description("guia_cerrada")]
        GuiaCerrada = 10,

        [Description("anulada")]
        Anulada = 11,

        [Description("rechazada")]
        Rechazada = 12
    }
}
