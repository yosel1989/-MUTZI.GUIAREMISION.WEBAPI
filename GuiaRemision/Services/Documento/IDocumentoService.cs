using GuiaRemision.DTOs.Efact;
using Microsoft.AspNetCore.Mvc;

namespace GuiaRemision.Services.DocumentEfact
{
    public interface IDocumentoService
    {
        Task<byte[]> ObtenerPdfAsync(string guid, string token);
        Task<byte[]> ObtenerXmlAsync(string guid, string token);
        Task<byte[]> ObtenerCdrAsync(string guid, string token);
        //Task<string> ObtenerGuidDesdeTicketAsync(string numeroGuia, string token);
        Task <(bool Exito, string Guid, EfactResponseDTO Estado, InternalErrorResponseDTO Error)> ObtenerGuidDesdeTicketAsync(string numeroGuia, string token);
        Task<(byte[] Archivo, EfactResponseDTO Estado, string Error)> ObtenerDocumentoAsync(string tipo, string guid, string token);
        Task<(bool Exito, byte[] Archivo, EfactResponseDTO Estado, InternalErrorResponseDTO Error)> ObtenerDocumentoCompletoAsync(
    string tipo, string ruc, string tipoGuia, string numeroGuia);
    }
}
