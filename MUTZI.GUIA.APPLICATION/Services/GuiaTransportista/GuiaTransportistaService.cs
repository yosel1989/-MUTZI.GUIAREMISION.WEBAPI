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
        private readonly ITransportistaRepository _transportistaRepository;
        private readonly IVentaConfigRepository _ventaConfigRepository;
        private readonly IMapper _mapper;
        public GuiaTransportistaService(
            IGuiaTransportistaRepository repository,
            IEstablecimientoRepository establecimientoRepository,
            ITransportistaRepository transportistaRepository,
            IVentaConfigRepository ventaConfigRepository,
            IMapper mapper
        )
        {
            _repository = repository;
            _establecimientoRepository = establecimientoRepository;
            _ventaConfigRepository = ventaConfigRepository;
            _transportistaRepository = transportistaRepository;
            _mapper = mapper;
        }
        public async Task<object> CreateAsync(string ruc, GuiaTransportistaCreateDTO dto)
        {
            try
            {
                
                // Crear la guía en la BBDD
                GuiaTransportistaDTO guiaCreada;
                try
                {
                    // Mapear todos los objetos
                    var remitente = await _establecimientoRepository.GetByIdAsync(dto.RemitenteId);
                    var remiteTransportista = await _transportistaRepository.GetByIdAsync(dto.RemiteTransportistaId ?? 0);
                    var destinatario = await _establecimientoRepository.GetByIdAsync(dto.DestinatarioId);
                    var conductor = dto.DatosEnvio.ConductorId.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionConductor>(p)).ToList();
                    var transporte = dto.DatosEnvio.TransporteId.Select(p => _mapper.Map<DOMAIN.Entities.UnidadTransporteGuid>(p)).ToList();
                    var guiaRemision = _mapper.Map<DOMAIN.Entities.GuiaRemision>(dto);
                    var datosEnvio = _mapper.Map<DOMAIN.Entities.GuiaRemisionDatoEnvio>(dto.DatosEnvio);
                    var origen = _mapper.Map<DOMAIN.Entities.GuiaRemisionOrigen>(dto.Origen);
                    var destino = dto.Destino.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionDestino>(p)).ToList();
                    var detalles = dto.Productos.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionDetalle>(p)).ToList();
                    var docRelacionados = dto.DocRelacionado?.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionDocumentoRelacionado>(p)).ToList();
                    var emailsDestinatarioGuia = dto.DestinatarioCorreo.Select(p => _mapper.Map<DOMAIN.Entities.GuiaRemisionEmailDestinatario>(p)).ToList();
                    var emailsRemiteTransportistaGuia = dto.RemiteTransportistaCorreo.Select(p => _mapper.Map<DOMAIN.Entities.EmailRemiteTransportistaGuid>(p)).ToList();
                    var ventaConfig = await _ventaConfigRepository.GetByRucAsync(ruc);


                    if (remitente is null) throw new Exception("No existe este remitente.");
                    if (destinatario is null) throw new Exception("No existe este destinatario.");
                    if (ventaConfig is null) throw new Exception($"El RUC principal ingresado {ruc} no tiene acceso al facturador. Pruebe con el Número de RUC del emisor del documento que se está relacionando al comprobante Guía de remisión remitente.");

                    var (esValido, error) = ValidarDatosGuiaTransportista(dto, ruc, remitente, destinatario, remiteTransportista, ventaConfig);
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
                                    : errorMsg.Contains("UnidadMedida") ? "Unidad de medida no válida."
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

        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO guia, string ruc, Establecimiento remitente, Establecimiento destinatario, Transportista remiteTransportista, VentaConfig ventaConfig)
        {
            try
            {
                if (remiteTransportista.NumeroDocumento == remitente.Ruc)
                {
                    return (false, $"'El 'Número de documento de identidad del remitente', no debe ser igual al número de RUC del transportista.'.");
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
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaDTO guia, string ruc, Establecimiento remitente, Establecimiento destinatario, Transportista remiteTransportista, VentaConfig ventaConfig)
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
