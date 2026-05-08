
using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.VentaConfig;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using MUTZI.GUIA.APPLICATION.Helpers;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
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

        public async Task<List<EmpresaListDTO>> GetOnlyNameAsync()
        {
            var empresas = await _repository.GetOnlyNameAsync();
            return _mapper.Map<List<EmpresaListDTO>>(empresas);
        }

        public async Task<TokenResponseDTO> ObtenerTokenAsync(string ruc)
        {
            var config = await _repository.GetByRucAsync(ruc);

            if (config == null)
                return null;

            if (string.IsNullOrEmpty(Url_Base_Efact))
                return null;

            if (string.IsNullOrEmpty(config.EfactApi))
                return null;

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

            var url = $"{Url_Base_Efact}/api-efact-ose/oauth/token";

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("=== RESPONSE BODY ===");
            Console.WriteLine(responseContent);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var token = JsonConvert.DeserializeObject<TokenResponseDTO>(responseContent);

            return token;
        }
    }
}
