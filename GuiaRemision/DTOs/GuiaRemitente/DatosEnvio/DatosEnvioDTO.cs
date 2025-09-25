using GuiaRemision.DTOs.GuiaRemitente.Conductor;
using GuiaRemision.DTOs.GuiaRemitente.Origen;
using GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte;

namespace GuiaRemision.DTOs.GuiaRemitente.DatosEnvio
{
    public class DatosEnvioDTO
    {
        public string MotivoEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public string PesoBruto { get; set; }
        public string CodigoUM { get; set; } = "KGM";
        public string? RucEmpresaCurrier { get; set; }
        public string? RazonSocialCurrier { get; set; }
        public string? RegistroMtcCurrier { get; set; }
        public ConductorDTO? Conductor { get; set; }
        public UnidadTransporteDTO? UnidadTransporte { get; set; }
    }
}
