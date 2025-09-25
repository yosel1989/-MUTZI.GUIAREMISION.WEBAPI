namespace GuiaRemision.Services.DocumentEfact
{
    public interface IDocumentoService
    {
        Task<byte[]> ObtenerPdfAsync(string guid, string token);
        Task<byte[]> ObtenerXmlAsync(string guid, string token);
        Task<byte[]> ObtenerCdrAsync(string guid, string token);
        Task<string> ObtenerGuidDesdeTicketAsync(string numeroGuia, string token);
    }
}
