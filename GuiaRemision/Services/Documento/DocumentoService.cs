
using GuiaRemision.DTOs.Efact;
using GuiaRemision.Services.VentaConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;

namespace GuiaRemision.Services.DocumentEfact
{
    public class DocumentoService : IDocumentoService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IVentaConfigService _ventaConfigService;
        private readonly string _urlBaseEfact;
        public DocumentoService(HttpClient httpClient, IConfiguration config, IVentaConfigService ventaConfigService)
        {
            _httpClient = httpClient;
            _config = config;
            _ventaConfigService = ventaConfigService;
            _urlBaseEfact = _config["AppSettings:url_base_facturador"];
        }
        public async Task<byte[]> ObtenerCdrAsync(string guid, string token)
        {
            var urlBase = _config["AppSettings:url_base_facturador"];
            var url = $"{urlBase}/api-efact-ose/v1/cdr/{guid}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener CDR: {response.StatusCode}-{await
                    response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> ObtenerPdfAsync(string guid, string token)
        {
            var urlBase = _config["AppSettings:url_base_facturador"];
            var url = $"{urlBase}/api-efact-ose/v1/pdf/{guid}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener PDF: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> ObtenerXmlAsync(string guid, string token)
        {
            var urlBase = _config["AppSettings:url_base_facturador"];
            var url = $"{urlBase}/api-efact-ose/v1/xml/{guid}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", token);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener XML: {response.StatusCode}-{await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<string> ObtenerGuidDesdeTicketAsync(string numeroGuia, string token)
        {
            var urlBase = _config["AppSettings:url_base_facturador"];
            var ticketNomenclatura = $"{numeroGuia}";
            var url = $"{urlBase}/api-efact-ose/v1/ticket/{ticketNomenclatura}";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al consultar ticket: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TicketResponseDTO>(json);

            return result?.Tickets?.FirstOrDefault()
                   ?? throw new Exception("No se encontró ningún GUID en la respuesta.");
        }

    }
}

