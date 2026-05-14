using System.ComponentModel;
namespace MUTZI.GUIA.DOMAIN.Enums
{
    public enum EnumGuiaRemisionEstado
    {
        [Description("registrada")]
        Registrada = 1,

        [Description("editada")]
        Editada = 2,

        [Description("confirmada")]
        Confirmada = 3,

        [Description("aprobada")]
        Aprobada = 4,

        [Description("emitida")]
        Emitida = 5,

        [Description("salida_confirmada")]
        SalidaConfirmada = 6,

        [Description("en_transito")]
        EnTransito = 7,

        [Description("recepcion_confirmada")]
        RecepcionConfirmada = 8,

        [Description("cerrada")]
        Cerrada = 9,

        [Description("anulada")]
        Anulada = 10,

        [Description("rechazada")]
        Rechazada = 11
    }
}
