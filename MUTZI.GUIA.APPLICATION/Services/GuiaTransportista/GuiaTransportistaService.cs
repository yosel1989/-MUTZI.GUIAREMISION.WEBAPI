using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaTransportista
{
    public class GuiaTransportistaService : IGuiaTransportistaService
    {
        private readonly IGuiaTransportistaRepository _repository;
        private readonly IEstablecimientoRepository _establecimientoRepository;
        private readonly IIntegrationCredentialRepository _integrationCredentialRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IMapper _mapper;
        public GuiaTransportistaService(
            IGuiaTransportistaRepository repository,
            IEstablecimientoRepository establecimientoRepository,
            IIntegrationCredentialRepository integrationCredentialRepository,
            IEntityRepository entityRepository,
            IMapper mapper
        )
        {
            _repository = repository;
            _establecimientoRepository = establecimientoRepository;
            _integrationCredentialRepository = integrationCredentialRepository;
            _entityRepository = entityRepository;
            _mapper = mapper;
        }
        public async Task<object> CreateAsync(string ruc, GuiaTransportistaCreateDTO request)
        {
            try
            {
                
                // Crear la guía en la BBDD
                GuiaTransportistaDTO guiaCreada;
                try
                {
                    // Mapear todos los objetos
                    var remitente = await _establecimientoRepository.GetByIdAsync(request.RemitenteId);
                    var remiteTransportista = await _entityRepository.GetByIdAsync(request.RemiteTransportistaId ?? 0);
                    var destinatario = await _establecimientoRepository.GetByIdAsync(request.DestinatarioId);
                    var conductor = request.DatosEnvio.ConductorId.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionConductor>(p)).ToList();
                    var transporte = request.DatosEnvio.TransporteId.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionUnidadTransporte>(p)).ToList();
                    var guiaRemision = _mapper.Map<DOMAIN.Entities.GuiaRemision>(request);
                    var datosEnvio = _mapper.Map<DOMAIN.Entities.GuiaRemisionDatoEnvio>(request.DatosEnvio);
                    var origen = _mapper.Map<DOMAIN.Entities.GuiaRemisionOrigen>(request.Origen);
                    var destino = request.Destino.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionDestino>(p)).ToList();
                    var detalles = request.Productos.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionDetalle>(p)).ToList();
                    var docRelacionados = request.DocRelacionado?.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionDocumentoRelacionado>(p)).ToList();
                    var emailsDestinatarioGuia = request.DestinatarioCorreo.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionEmailDestinatario>(p)).ToList();
                    var emailsRemiteTransportistaGuia = request.RemiteTransportistaCorreo.Select(p => _mapper.Map<DOMAIN.Entities.EmailRemiteTransportistaGuid>(p)).ToList();
                    var ventaConfig = await _integrationCredentialRepository.GetByEntityId(request.EntityId, "transportista", strict: true);


                    if (remitente is null) throw new Exception("No existe este remitente.");
                    if (destinatario is null) throw new Exception("No existe este destinatario.");
                    

                    var (esValido, error) = ValidarDatosGuiaTransportista(request, ruc, remitente, destinatario, remiteTransportista, ventaConfig);
                    if (!esValido)
                    {
                        return new BadRequestResponseDTO
                        {
                            Error = "Error en datos enviados",
                            Detalle = error
                        };
                    }

                    // Llamar al repositorio
                    var (guiaID, numeroGuia) = await _repository.CreateGuiaTransportistaTransactionalAsync(
                        remitente, remiteTransportista, destinatario, conductor, transporte,
                        guiaRemision, datosEnvio, origen, destino, detalles, docRelacionados, emailsDestinatarioGuia, emailsRemiteTransportistaGuia
                    );
                    return new GuiaCreatedResponseDTO
                    {
                        GuiaID = guiaID,
                        NumeroGuia = numeroGuia,
                        TipoGuia = "TRANSPORTISTA"
                    };
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                    string errorBox = errorMsg.Contains("FOREIGN KEY") ? "Error de clave foránea."
                                    : errorMsg.Contains("UnitMeasurement") ? "Unidad de medida no válida."
                                    : errorMsg.Contains("String") ? "Error de longitud de campo."
                                    : "";
                    return new BadRequestResponseDTO
                    {
                        Error = "Error al guardar guía transportista",
                        Detalle = $"{errorBox} Más detalle: {errorMsg}"
                    };
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                return new InternalErrorResponseDTO
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }
            //SE obtiene la guia de remision creada
            //var guiaCreada = await _repository.GetByIdAsync(id);
            //return _mapper.Map<GuiaTransportistaDTO>(guiaCreada);
        }

        public async Task<GuiaTransportistaDTO> GetByIdAsync(int id)
        {
            var guia = await _repository.GetByIdAsync(id);
            return _mapper.Map<GuiaTransportistaDTO>(guia);
        }

        public async Task<GuiaTransportistaDTO> UpdateAsync(int id, GuiaTransportistaUpdateDTO guiaTransportistaDto)
        {
            var existingGuiaRemision = await _repository.GetByIdAsync(id);
            if (existingGuiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión transportista no encontrada");
            }
            _mapper.Map(guiaTransportistaDto, existingGuiaRemision);
            await _repository.UpdateAsync(existingGuiaRemision);
            return _mapper.Map<GuiaTransportistaDTO>(existingGuiaRemision);
        }

        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO guia, string ruc, EntityBranch remitente, EntityBranch destinatario, Entity remiteTransportista, IntegrationCredential integrationCredential)
        {
            try
            {
                if (remiteTransportista.EntityId == remitente.EntityId)
                {
                    return (false, $"La entidad remitente no debe ser igual a la entidad transportista");
                }
                if (guia.DocRelacionado?.Count > 1)
                {
                    return (false, "Solo se permite un documento relacionado como referencia en la guía de remisión transportista.");
                }
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaDTO guia, string ruc, EntityBranch remitente, EntityBranch destinatario, Entity remiteTransportista, IntegrationCredential ventaConfig)
        {
            try
            {
                if (guia.RemiteTransportista.NumeroDocumento == guia.Remitente.Ruc)
                {
                    return (false, $"'El 'Número de documento de identidad del remitente', no debe ser igual al número de RUC del transportista.'.");
                }
                //_ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento);
                _ = guia.getTipoCodigoDocumento("RUC");
                if (guia.DocRelacionados?.Count > 1)
                {
                    return (false, "Solo se permite un documento relacionado como referencia en la guía de remisión transportista.");
                }
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
