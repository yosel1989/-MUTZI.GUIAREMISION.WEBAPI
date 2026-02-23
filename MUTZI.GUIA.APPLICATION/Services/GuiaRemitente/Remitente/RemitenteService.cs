using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente.Remitente
{
    public class RemitenteService : IRemitenteService
    {
        private readonly IRemitenteRepository _repository;
        private readonly IMapper _mapper;
        public RemitenteService(IRemitenteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<RemitenteListDTO>> GetAllAsync()
        {
            var remitentes = await _repository.GetAllAsync();
            return _mapper.Map<List<RemitenteListDTO>>(remitentes);
        }
        public async Task<RemitenteDTO?> GetByIdAsync(int id)
        {
            var remitente = await _repository.GetByIdAsync(id);
            if (remitente == null) return null;
            return _mapper.Map<RemitenteDTO>(remitente);
        }
        public async Task<RemitenteForGuiaDTO?> GetByIdForGuiaAsync(int id, string tipoGuia)
        {
            var remitente = await _repository.GetByIdAsync(id);
            if (remitente == null) return null;

            var dto = _mapper.Map<RemitenteForGuiaDTO>(remitente);

            // Calcular serie completa según tipo de guía
            var prefix = string.Equals(tipoGuia, "TRANSPORTISTA", StringComparison.OrdinalIgnoreCase) ? "V" : "T";
            var serieBase = remitente.Serie?.Trim();
            if (!string.IsNullOrEmpty(serieBase))
            {
                var serieCompleta = prefix + serieBase;

                // Obtener la última guía registrada para esa serie
                var lastGuia = await _repository.GetLastBySerieAsync(serieCompleta);

                // Calcular el siguiente correlativo (si no existe, iniciar en 1)
                int nextCorrelativo = 1;
                if (lastGuia != null && !string.IsNullOrWhiteSpace(lastGuia.Correlativo))
                {
                    if (int.TryParse(lastGuia.Correlativo, out int lastCorr))
                        nextCorrelativo = lastCorr + 1;
                }

                // Formatear correlativo a 8 dígitos y construir número de guía
                string correlativoStr = nextCorrelativo.ToString("D8");

                dto.NuevaSerie = serieCompleta;
                dto.NuevoCorrelativo = nextCorrelativo;
                dto.NuevoNumeroGuia = $"{serieCompleta}-{correlativoStr}";
            }

            return dto;
        }
        public async Task<int> CreateAsync(RemitenteCreateDTO createDTO)
        {
            var remitente = _mapper.Map<DOMAIN.Entities.Remitente>(createDTO);
            await _repository.CreateAsync(remitente);
            return remitente.RemitenteId;
        }
        public async Task<RemitenteUpdateDTO> UpdateAsync(int id, RemitenteUpdateDTO updateDTO)
        {
            var existingRemitente = await _repository.GetByIdAsync(id);
            if (existingRemitente == null)
            {
                throw new KeyNotFoundException($"Remitente con ID {id} no encontrado.");
            }
            _mapper.Map(updateDTO, existingRemitente);
            await _repository.UpdateAsync(existingRemitente);
            return updateDTO;
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
