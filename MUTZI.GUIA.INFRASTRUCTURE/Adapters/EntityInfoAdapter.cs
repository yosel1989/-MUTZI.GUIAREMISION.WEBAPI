using Microsoft.Extensions.Configuration;
using MUTZI.GUIA.APPLICATION.Interfaces.Ports;
using MUTZI.GUIA.DOMAIN.Models;
using System.Text.Json;

namespace MUTZI.GUIA.INFRASTRUCTURE.Adapters
{
    public class EntityInfoAdapter: IEntityInfoPort
    {
        public readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EntityInfoAdapter(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<PersonInfo> GetPersonInfoAsync(string documentNumber)
        {
            return await GetPersonInfoPeruDevsAsync(documentNumber);
        }

        public async Task<CompanyInfo> GetCompanyInfoAsync(string documentNumber)
        {
            return await GetCompanyInfoPeruDevsAsync(documentNumber);
        }

        private async Task<PersonInfo?> GetPersonInfoPeruDevsAsync(string documentNumber)
        {
            var url = $"{_configuration["EntityInfoProviders:PeruDevs:Url"]}/dni/complete?document={documentNumber}&key={_configuration["EntityInfoProviders:PeruDevs:ApiKey"]}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al consultar info de persona desde perudevs");
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            if (data["estado"].GetBoolean())
            {
                var resultado = data["resultado"];
                var resultadoDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(resultado.GetRawText());

                return new PersonInfo
                {
                    DocumentNumber = resultadoDict["id"].GetString(),
                    FirstName = resultadoDict["nombres"].GetString(),
                    LastName = $"{resultadoDict["apellido_paterno"].GetString()} {resultadoDict["apellido_materno"].GetString()}"
                };
            }


            return null;
        }

        private async Task<CompanyInfo> GetCompanyInfoPeruDevsAsync(string documentNumber)
        {
            var url = $"{_configuration["EntityInfoProviders:PeruDevs:Url"]}/ruc?document={documentNumber}&key={_configuration["EntityInfoProviders:PeruDevs:ApiKey"]}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al consultar info de empresa desde perudevs");
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            if (data["estado"].GetBoolean())
            {
                var resultado = data["resultado"];
                var resultadoDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(resultado.GetRawText());

                return new CompanyInfo
                {
                    DocumentNumber = resultadoDict["id"].GetString(),
                    Name = resultadoDict["razon_social"].GetString(),
                    Address = resultadoDict["direccion"].GetString()
                };
            }


            return null;
        }
    }
}
