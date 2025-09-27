using GuiaRemision.DTOs.GuiaRemitente.Destino;
using System.ComponentModel.DataAnnotations;

namespace GuiaRemision.DTOs.GuiaRemitente.Destinatario
{
    public class DestinatarioDTO
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string UbigeoId { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Pais { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
