using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
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
                    var docRelacionados = dto.DocRelacionado?.Select(p => _mapper.Map<DOMAIN.Entities.DocRelacionado>(p)).ToList();
                    var emailsDestinatarioGuia = dto.Destinatario.EmailDestinatarioGuia.Select(p => _mapper.Map<DOMAIN.Entities.EmailDestinatarioGuid>(p)).ToList();
                    var emailsRemiteTransportistaGuia = dto.RemiteTransportista.EmailRemiteTransportista.Select(p => _mapper.Map<DOMAIN.Entities.EmailRemiteTransportistaGuid>(p)).ToList();

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
                        Error = "Error al guardar la guía transportista",
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

        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaCreateDTO guia, string ruc)
        {
            try
            {
                var ventaConfig = _ventaConfigRepository.GetByRucAsync(ruc).Result;
                if (ventaConfig == null || string.IsNullOrEmpty(ventaConfig.Ruc))
                {
                    return (false, $"El RUC principal ingresado {ruc} no tiene acceso al facturador.");
                }
                _ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento);
                if (guia.DocRelacionado?.Count > 1)
                {
                    return (false, "Solo se permite un documento relacionado como referencia en la guía de remisión transportista.");
                }
                //if (string.IsNullOrWhiteSpace(guia.Observacion))
                //    return (false, "La observación no puede estar vacía.");
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool EsValido, string Error) ValidarDatosGuiaTransportista(GuiaTransportistaDTO guia, string ruc)
        {
            //try
            //{
            //    if (guia == null)
            //        return (false, "La guía proporcionada es nula.");

            //    GuiaTransportistaCreateDTO guiaCreate;
            //    try
            //    {
            //        guiaCreate = _mapper.Map<GuiaTransportistaCreateDTO>(guia);
            //    }
            //    catch (Exception ex)
            //    {
            //        return (false, $"Error al convertir GuiaTransportistaDTO a GuiaTransportistaCreateDTO: {ex.Message}");
            //    }

            //    return ValidarDatosGuiaTransportista(guiaCreate, ruc);
            //}
            //catch (Exception ex)
            //{
            //    return (false, ex.Message);
            //}
            try
            {
                var ventaConfig = _ventaConfigRepository.GetByRucAsync(ruc).Result;
                if (ventaConfig == null || string.IsNullOrEmpty(ventaConfig.Ruc))
                {
                    return (false, $"El RUC principal ingresado {ruc} no tiene acceso al facturador.");
                }
                _ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento);
                if (guia.DocRelacionados?.Count > 1)
                {
                    return (false, "Solo se permite un documento relacionado como referencia en la guía de remisión transportista.");
                }
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
