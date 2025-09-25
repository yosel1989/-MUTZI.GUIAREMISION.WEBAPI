using AutoMapper;
using GuiaRemision.DTOs.GuiaTransportista;
using GuiaRemision.Repositories.GuiaTransportista;

namespace GuiaRemision.Services.GuiaTransportista
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
            var remitente = _mapper.Map<Models.DBMS.Remitente>(dto.Remitente);
            var remiteTransportista = _mapper.Map<Models.DBMS.RemiteTransportista>(dto.RemiteTransportista);
            var destinatario = _mapper.Map<Models.DBMS.Destinatario>(dto.Destinatario);
            var conductor = _mapper.Map<Models.DBMS.Conductor>(dto.DatosEnvio.Conductor);
            var transporte = _mapper.Map<Models.DBMS.Transporte>(dto.DatosEnvio.UnidadTransporte);
            var guiaRemision = _mapper.Map<Models.DBMS.GuiaRemision>(dto);
            var datosEnvio = _mapper.Map<Models.DBMS.DatosEnvio>(dto.DatosEnvio);
            var origen = _mapper.Map<Models.DBMS.Origen>(dto.Origen);
            //var destino = _mapper.Map<Models.DBMS.Destino>(dto.Destino);
            var destino = dto.Destino.Select(p => _mapper.Map<Models.DBMS.Destino>(p)).ToList();
            var detalles = dto.Productos.Select(p => _mapper.Map<Models.DBMS.GuiaDetalle>(p)).ToList();

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
                if (string.IsNullOrWhiteSpace(guia.Observacion))
                    return (false, "La observación no puede estar vacía.");
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
