
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.VentaConfig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Documento;
using Microsoft.Extensions.Configuration;

namespace MUTZI.GUIA.APPLICATION.Services.Documento
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
        public string ObtenerTipoGuia(string tipoGuia)
        {
            switch (tipoGuia.ToUpper())
            {
                case "REMITENTE":
                    return "09";
                case "TRANSPORTISTA":
                    return "31";
                default:
                    return "00";
            }
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
        public async Task<(bool Exito, string Guid, EfactResponseDTO Estado, InternalErrorResponseDTO Error)> ObtenerGuidDesdeTicketAsync(string numeroGuia, string token)
        {
            var urlBase = _config["AppSettings:url_base_facturador"];
            var ticketNomenclatura = $"{numeroGuia}";
            var url = $"{urlBase}/api-efact-ose/v1/ticket/{ticketNomenclatura}";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var efactError = JsonConvert.DeserializeObject<EfactResponseDTO>(json);
                    return (false, null, efactError, new InternalErrorResponseDTO
                    {
                        Error = "Error al consultar ticket",
                        Detalle = $"{response.StatusCode} - {json}"
                    });
                }
                catch
                {
                    return (false, null, null, new InternalErrorResponseDTO
                    {
                        Error = "Error al consultar ticket",
                        Detalle = $"{response.StatusCode} - {json}"
                    });
                }
            }

            var result = JsonConvert.DeserializeObject<TicketResponseDTO>(json);
            var guid = result?.Tickets?.FirstOrDefault();

            if (string.IsNullOrEmpty(guid))
            {
                return (false, null, null, new InternalErrorResponseDTO
                {
                    Error = "No se encontró ningún GUID en la respuesta.",
                    Detalle = json
                });
            }

            return (true, guid, null, null);
        }
        public async Task<(byte[] Archivo, EfactResponseDTO Estado, string Error)> ObtenerDocumentoAsync(string tipo, string guid, string token)
        {
            var urlBase = _config["AppSettings:url_base_facturador"];
            var url = $"{urlBase}/api-efact-ose/v1/{tipo}/{guid}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            var contentType = response.Content.Headers.ContentType?.MediaType;

            if (response.IsSuccessStatusCode)
            {
                // Si el archivo está disponible, lo devolvemos
                if (contentType == "application/pdf" || contentType == "application/xml" || contentType == "application/cdr" || contentType == "application/octet-stream")
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    return (bytes, null, null);
                }
                // Si la respuesta es JSON, puede estar en proceso
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var estado = JsonConvert.DeserializeObject<EfactResponseDTO>(json);
                    if (estado != null && estado.Code == "-9998")
                    {
                        return (null, estado, null); // En proceso
                    }
                }
                catch { }
            }
            // Si hay error, devolvemos el mensaje
            var errorMsg = await response.Content.ReadAsStringAsync();
            return (null, null, errorMsg);
        }
        public async Task<(bool Exito, byte[] Archivo, EfactResponseDTO Estado, InternalErrorResponseDTO Error)> ObtenerDocumentoCompletoAsync(
    string tipo, string ruc, string tipoGuia, string numeroGuia)
        {
            var token = await _ventaConfigService.ObtenerTokenAsync(ruc);
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                return (false, null, null, new InternalErrorResponseDTO
                {
                    Error = "Token inválido",
                    Detalle = "No se pudo obtener un token válido para EFACT."
                });
            }

            var (exito, guid, estado, error) = await ObtenerGuidDesdeTicketAsync($"{ruc}-{ObtenerTipoGuia(tipoGuia)}-{numeroGuia}", token.AccessToken);

            if (!exito)
                return (false, null, estado, error);

            var (archivo, estadoDoc, errorDoc) = await ObtenerDocumentoAsync(tipo, guid, token.AccessToken);

            if (archivo != null)
                return (true, archivo, null, null);

            if (estadoDoc != null)
                return (false, null, estadoDoc, null);

            return (false, null, null, new InternalErrorResponseDTO
            {
                Error = $"Error al obtener {tipo.ToUpper()}",
                Detalle = errorDoc
            });
        }
    }
}

