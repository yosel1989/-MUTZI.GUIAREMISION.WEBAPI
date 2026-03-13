using MUTZI.GUIA.APPLICATION.DTOs.Ubigeo;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Ubigeo;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Ubigeo;

namespace MUTZI.GUIA.APPLICATION.Services.Ubigeo
{
    public class UbigeoService: IUbigeoService
    {
        private readonly IUbigeoRepository _repository;
        public UbigeoService(IUbigeoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UbigeoDistritoDto>> GetDistritoAsync(string ubigeoProvincia)
        {
            return await _repository.GetDistritoAsync(ubigeoProvincia);
        }
        public async Task<IEnumerable<UbigeoProvinciaDto>> GetProvinciaAsync(string ubigeoDepartamento)
        {
            return await _repository.GetProvinciaAsync(ubigeoDepartamento);
        }
        public async Task<IEnumerable<UbigeoDepartamentoDto>> GetDepartamentoAsync()
        {
            try
            {
                return await _repository.GetDepartamentoAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<UbigeoDto> FindByIdAsync(string ubigeo)
        {
            return await _repository.FindByIdAsync(ubigeo);
        }
        public IEnumerable<PaisesDTO> GetPaisesAsync()
        {
            var listPaises = new List<PaisesDTO>();

            var paises = new[]
            {
                new PaisesDTO { Pais = "Afganistán", Codigo = "AF" },
                new PaisesDTO { Pais = "Albania", Codigo = "AL" },
                new PaisesDTO { Pais = "Alemania", Codigo = "DE" },
                new PaisesDTO { Pais = "Andorra", Codigo = "AD" },
                new PaisesDTO { Pais = "Angola", Codigo = "AO" },
                new PaisesDTO { Pais = "Arabia Saudita", Codigo = "SA" },
                new PaisesDTO { Pais = "Argentina", Codigo = "AR" },
                new PaisesDTO { Pais = "Armenia", Codigo = "AM" },
                new PaisesDTO { Pais = "Australia", Codigo = "AU" },
                new PaisesDTO { Pais = "Austria", Codigo = "AT" },
                new PaisesDTO { Pais = "Bahamas", Codigo = "BS" },
                new PaisesDTO { Pais = "Bahréin", Codigo = "BH" },
                new PaisesDTO { Pais = "Bangladesh", Codigo = "BD" },
                new PaisesDTO { Pais = "Barbados", Codigo = "BB" },
                new PaisesDTO { Pais = "Bélgica", Codigo = "BE" },
                new PaisesDTO { Pais = "Belice", Codigo = "BZ" },
                new PaisesDTO { Pais = "Bolivia", Codigo = "BO" },
                new PaisesDTO { Pais = "Brasil", Codigo = "BR" },
                new PaisesDTO { Pais = "Bulgaria", Codigo = "BG" },
                new PaisesDTO { Pais = "Canadá", Codigo = "CA" },
                new PaisesDTO { Pais = "Chile", Codigo = "CL" },
                new PaisesDTO { Pais = "China", Codigo = "CN" },
                new PaisesDTO { Pais = "Colombia", Codigo = "CO" },
                new PaisesDTO { Pais = "Costa Rica", Codigo = "CR" },
                new PaisesDTO { Pais = "Cuba", Codigo = "CU" },
                new PaisesDTO { Pais = "Dinamarca", Codigo = "DK" },
                new PaisesDTO { Pais = "Ecuador", Codigo = "EC" },
                new PaisesDTO { Pais = "Egipto", Codigo = "EG" },
                new PaisesDTO { Pais = "El Salvador", Codigo = "SV" },
                new PaisesDTO { Pais = "España", Codigo = "ES" },
                new PaisesDTO { Pais = "Estados Unidos", Codigo = "US" },
                new PaisesDTO { Pais = "Estonia", Codigo = "EE" },
                new PaisesDTO { Pais = "Finlandia", Codigo = "FI" },
                new PaisesDTO { Pais = "Francia", Codigo = "FR" },
                new PaisesDTO { Pais = "Grecia", Codigo = "GR" },
                new PaisesDTO { Pais = "Guatemala", Codigo = "GT" },
                new PaisesDTO { Pais = "Honduras", Codigo = "HN" },
                new PaisesDTO { Pais = "Hungría", Codigo = "HU" },
                new PaisesDTO { Pais = "India", Codigo = "IN" },
                new PaisesDTO { Pais = "Indonesia", Codigo = "ID" },
                new PaisesDTO { Pais = "Irlanda", Codigo = "IE" },
                new PaisesDTO { Pais = "Islandia", Codigo = "IS" },
                new PaisesDTO { Pais = "Israel", Codigo = "IL" },
                new PaisesDTO { Pais = "Italia", Codigo = "IT" },
                new PaisesDTO { Pais = "Jamaica", Codigo = "JM" },
                new PaisesDTO { Pais = "Japón", Codigo = "JP" },
                new PaisesDTO { Pais = "México", Codigo = "MX" },
                new PaisesDTO { Pais = "Nicaragua", Codigo = "NI" },
                new PaisesDTO { Pais = "Noruega", Codigo = "NO" },
                new PaisesDTO { Pais = "Panamá", Codigo = "PA" },
                new PaisesDTO { Pais = "Paraguay", Codigo = "PY" },
                new PaisesDTO { Pais = "Perú", Codigo = "PE" },
                new PaisesDTO { Pais = "Polonia", Codigo = "PL" },
                new PaisesDTO { Pais = "Portugal", Codigo = "PT" },
                new PaisesDTO { Pais = "Puerto Rico", Codigo = "PR" },
                new PaisesDTO { Pais = "Reino Unido", Codigo = "GB" },
                new PaisesDTO { Pais = "República Dominicana", Codigo = "DO" },
                new PaisesDTO { Pais = "Rusia", Codigo = "RU" },
                new PaisesDTO { Pais = "Sudáfrica", Codigo = "ZA" },
                new PaisesDTO { Pais = "Suecia", Codigo = "SE" },
                new PaisesDTO { Pais = "Suiza", Codigo = "CH" },
                new PaisesDTO { Pais = "Turquía", Codigo = "TR" },
                new PaisesDTO { Pais = "Ucrania", Codigo = "UA" },
                new PaisesDTO { Pais = "Uruguay", Codigo = "UY" },
                new PaisesDTO { Pais = "Venezuela", Codigo = "VE" },
                new PaisesDTO { Pais = "Vietnam", Codigo = "VN" }
            };

            listPaises.AddRange(paises);

            return listPaises;
        }
    }
}
