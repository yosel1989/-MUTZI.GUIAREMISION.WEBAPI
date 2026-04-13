using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.VentaConfig;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaTransportista
{
    public class GuiaTransportistaService : IGuiaTransportistaService
    {
        private readonly IGuiaTransportistaRepository _repository;
        private readonly IVentaConfigRepository _ventaConfigRepository;
        private readonly IMapper _mapper;
        public GuiaTransportistaService(IGuiaTransportistaRepository repository, IMapper mapper, IVentaConfigRepository ventaConfigRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _ventaConfigRepository = ventaConfigRepository;
        }
        public async Task<object> CreateAsync(string ruc, GuiaTransportistaCreateDTO dto)
        {
            try
            {
                var (esValido, error) = ValidarDatosGuiaTransportista(dto, ruc);
                if (!esValido)
                {
                    return new BadRequestResponseDTO
                    {
                        Error = "Error en datos enviados",
                        Detalle = error
                    };
                }
                // Crear la guía en la BBDD
                GuiaTransportistaDTO guiaCreada;
                try
                {
                    // Mapear todos los objetos
                    var remitente = _mapper.Map<DOMAIN.Entities.Remitente>(dto.Remitente);
                    var remiteTransportista = _mapper.Map<DOMAIN.Entities.RemiteTransportistum>(dto.RemiteTransportista);
                    var destinatario = _mapper.Map<DOMAIN.Entities.Destinatario>(dto.Destinatario);
                    var conductor = dto.DatosEnvio.ConductorId.Select(p => _mapper.Map<DOMAIN.Entities.ConductorGuid>(p)).ToList();
                    var transporte = dto.DatosEnvio.TransporteId.Select(p => _mapper.Map<DOMAIN.Entities.TransporteGuid>(p)).ToList();
                    var guiaRemision = _mapper.Map<DOMAIN.Entities.GuiaRemision>(dto);
                    var datosEnvio = _mapper.Map<DOMAIN.Entities.DatosEnvio>(dto.DatosEnvio);
                    var origen = _mapper.Map<DOMAIN.Entities.Origen>(dto.Origen);
                    var destino = dto.Destino.Select(p => _mapper.Map<DOMAIN.Entities.Destino>(p)).ToList();
                    var detalles = dto.Productos.Select(p => _mapper.Map<DOMAIN.Entities.GuiaDetalle>(p)).ToList();
                    var docRelacionados = dto.DocRelacionado?.Select(p => _mapper.Map<DOMAIN.Entities.DocRelacionado>(p)).ToList();
                    var emailsDestinatarioGuia = dto.Destinatario?.EmailDestinatarioGuia?.Select(p => _mapper.Map<DOMAIN.Entities.EmailDestinatarioGuid>(p)).ToList();
                    var emailsRemiteTransportistaGuia = dto.RemiteTransportista?.EmailRemiteTransportista?.Select(p => _mapper.Map<DOMAIN.Entities.EmailRemiteTransportistaGuid>(p)).ToList();

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
                throw new KeyNotFoundException("Guia de remisión transportista no encontrada");
            }
            _mapper.Map(guiaTransportistaDto, existingGuiaRemision);
            await _repository.UpdateAsync(existingGuiaRemision);
            return _mapper.Map<GuiaTransportistaDTO>(existingGuiaRemision);
        }

        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO guia, string ruc)
        {
            try
            {
                var ventaConfig = _ventaConfigRepository.GetByRucAsync(ruc).Result;
                if (ventaConfig == null || string.IsNullOrEmpty(ventaConfig.Ruc))
                {
                    return (false, $"El RUC principal ingresado {ruc} no tiene acceso al facturador.");
                }
                if (guia.RemiteTransportista.NumeroDocumento== guia.Remitente.Ruc)
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
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaDTO guia, string ruc)
        {
            try
            {
                var ventaConfig = _ventaConfigRepository.GetByRucAsync(ruc).Result;
                if (ventaConfig == null || string.IsNullOrEmpty(ventaConfig.Ruc))
                {
                    return (false, $"El RUC principal ingresado {ruc} no tiene acceso al facturador.");
                }
                if (guia.RemiteTransportista.NumeroDocumento == guia.Remitente.Ruc)
                {
                    return (false, $"'El 'Número de documento de identidad del remitente', no debe ser igual al número de RUC del transportista.'.");
                }
                _ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento);
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
