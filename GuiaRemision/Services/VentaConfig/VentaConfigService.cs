
using AutoMapper;
using GuiaRemision.DTOs.Efact;
using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.DTOs.VentaConfig;
using GuiaRemision.Helpers;
using GuiaRemision.Repositories.VentaConfig;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GuiaRemision.Services.VentaConfig
{
    public class VentaConfigService : IVentaConfigService
    {
        private readonly IVentaConfigRepository _repository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string Url_Base_Efact = "";//"https://ose-gw1.efact.pe";
        public VentaConfigService(IVentaConfigRepository repository, IMapper mapper, HttpClient httpClient, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _httpClient = httpClient;
            _config = config;
            Url_Base_Efact = config["AppSettings:url_base_facturador"];
        }

        public async Task<VentaConfigAccessDTO> GetByRucAsync(string ruc)
        {
            return _mapper.Map<VentaConfigAccessDTO>(await _repository.GetByRucAsync(ruc));
        }
        public async Task<TokenResponseDTO> ObtenerTokenAsync(string ruc)
        {
            var config = await _repository.GetByRucAsync(ruc);

            if (config == null)
                throw new InvalidOperationException("No se encontró configuración para el RUC especificado.");

            if (string.IsNullOrEmpty(this.Url_Base_Efact))
                throw new InvalidOperationException("La URL base de la API no está configurada.");

            if (string.IsNullOrEmpty(config.EfactApi))
                throw new InvalidOperationException("La contraseña de la API (EfactApi) no está configurada.");

            // credenciales fijas del proveedor
            string credentials = "client:secret";
            string base64Credentials = Helper.Base64Encode(credentials);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", base64Credentials);

            var parameters = new Dictionary<string, string>
            {
                { "username", config.Ruc },
                { "password", config.EfactApi }, // contraseña
                { "grant_type", "password" }
            };

            var url = $"{this.Url_Base_Efact}/api-efact-ose/oauth/token";

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("=== RESPONSE BODY ===");
            Console.WriteLine(responseContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Error al obtener token: {response.StatusCode} - {responseContent}");
            }

            var token = JsonConvert.DeserializeObject<TokenResponseDTO>(responseContent);

            return token ?? throw new ApplicationException("No se pudo deserializar la respuesta del token.");
        }
    }
}
