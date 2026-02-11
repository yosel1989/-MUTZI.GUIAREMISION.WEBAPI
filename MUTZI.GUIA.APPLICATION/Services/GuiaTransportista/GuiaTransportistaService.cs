using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaTransportista
{
    public class GuiaTransportistaService : IGuiaTransportistaService
    {
        private readonly IGuiaTransportistaRepository _repository;
        private readonly IMapper _mapper;
        public GuiaTransportistaService(IGuiaTransportistaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GuiaTransportistaDTO> CreateAsync(GuiaTransportistaCreateDTO dto)
        {
            // Mapear todos los objetos
            var remitente = _mapper.Map<DOMAIN.Entities.Remitente>(dto.Remitente);
            var remiteTransportista = _mapper.Map<DOMAIN.Entities.RemiteTransportistum>(dto.RemiteTransportista);
            var destinatario = _mapper.Map<DOMAIN.Entities.Destinatario>(dto.Destinatario);
            //var conductor = _mapper.Map<DOMAIN.Entities.Conductor>(dto.DatosEnvio.Conductor);
            var conductor = dto.DatosEnvio.Conductor.Select(p => _mapper.Map<DOMAIN.Entities.Conductor>(p)).ToList();
            //var transporte = _mapper.Map<DOMAIN.Entities.Transporte>(dto.DatosEnvio.UnidadTransporte);
            var transporte = dto.DatosEnvio.UnidadTransporte.Select(p => _mapper.Map<DOMAIN.Entities.Transporte>(p)).ToList();
            var guiaRemision = _mapper.Map<DOMAIN.Entities.GuiaRemision>(dto);
            var datosEnvio = _mapper.Map<DOMAIN.Entities.DatosEnvio>(dto.DatosEnvio);
            var origen = _mapper.Map<DOMAIN.Entities.Origen>(dto.Origen);
            //var destino = _mapper.Map<DOMAIN.Entities.Destino>(dto.Destino);
            var destino = dto.Destino.Select(p => _mapper.Map<DOMAIN.Entities.Destino>(p)).ToList();
            var detalles = dto.Productos.Select(p => _mapper.Map<DOMAIN.Entities.GuiaDetalle>(p)).ToList();

            // Llamar al repositorio
            var id = await _repository.CreateGuiaTransportistaTransactionalAsync(
                remitente, remiteTransportista, destinatario, conductor, transporte,
                guiaRemision, datosEnvio, origen, destino, detalles
            );
            //SE obtiene la guia de remision creada
            var guiaCreada = await _repository.GetByIdAsync(id);
            return _mapper.Map<GuiaTransportistaDTO>(guiaCreada);
        }

        public async Task<GuiaTransportistaDTO> GetByIdAsync(int id)
        {
            var guia = await _repository.GetByIdAsync(id);
            return _mapper.Map<GuiaTransportistaDTO>(guia);
        }

        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO guia)
        {
            try
            {
                _ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento);
                _ = guia.DocumentoAdicinalTipo();
                //if (string.IsNullOrWhiteSpace(guia.Observacion))
                //    return (false, "La observación no puede estar vacía.");
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
