using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionHistorial;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class GuiaRemisionHistorialService: IGuiaRemisionHistorialService
    {
        private readonly IGuiaRemisionHistorialRepository _guiaRemisionHistorialRepository;
        private readonly IMapper _mapper;
        public GuiaRemisionHistorialService(
            IGuiaRemisionHistorialRepository guiaRemisionHistorialRepository,
            IMapper mapper
        ) { 
            _guiaRemisionHistorialRepository = guiaRemisionHistorialRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GuiaRemisionHistorialListDTO>> GetAllByGuiaAsync(int guiaRemisionId)
        {
            var data = await _guiaRemisionHistorialRepository.ObtenerListadoByGuiaAsync(guiaRemisionId);

            return _mapper.Map<IEnumerable<GuiaRemisionHistorialListDTO>>(data);
        }
    }
}
