using System.ComponentModel;
namespace MUTZI.GUIA.DOMAIN.Enums
{
    public enum EnumGuiaRemisionEstado
    {
        [Description("Registrada")]
        Registrada = 1,

        [Description("Editada")]
        Editada = 2,

        [Description("Confirmada")]
        Confirmada = 3,

        [Description("Aprobada")]
        Aprobada = 4,

        [Description("Emitida")]
        Emitida = 5,

        [Description("Salida Confirmada")]
        SalidaConfirmada = 6,

        [Description("En Transito")]
        EnTransito = 7,

        [Description("Recepción Confirmada")]
        RecepcionConfirmada = 8,

        [Description("Cerrada")]
        Cerrada = 9,

        [Description("Anulada")]
        Anulada = 10,

        [Description("Rechazada")]
        Rechazada = 11
    }
}
