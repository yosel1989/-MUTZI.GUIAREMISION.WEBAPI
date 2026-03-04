using AutoMapper;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.VentaConfig;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using System;
using System.Text.Json;

namespace MUTZI.GUIA.APPLICATION.Services.GuiaRemitente
{
    public class GuiaRemitenteService : IGuiaRemitenteService
    {
        private readonly IGuiaRemitenteRepository _repository;
        private readonly IVentaConfigRepository _ventaConfigRepository;
        private readonly IMapper _mapper;
        public GuiaRemitenteService(IGuiaRemitenteRepository repository, IVentaConfigRepository ventaConfigRepository, IMapper mapper)
        {
            _repository = repository;
            _ventaConfigRepository = ventaConfigRepository;
            _mapper = mapper;
        }
        public async Task<List<GuiaRemitenteDTO>> GetAllAsync()
        {
            var guiaRemisions = await _repository.GetAllAsync();
            return _mapper.Map<List<GuiaRemitenteDTO>>(guiaRemisions);
        }
        public async Task<GuiaRemitenteDTO> GetByIdAsync(int id)
        {
            var guiaRemision = await _repository.GetByIdAsync(id);
            return _mapper.Map<GuiaRemitenteDTO>(guiaRemision);
        }
        public async Task<GuiaPorTicketDTO> GetByTicketAsync(string ticket)
        {
            var guiaRemision = await _repository.GetByTicketAsync(ticket);
            return _mapper.Map<GuiaPorTicketDTO>(guiaRemision);
        }
        public async Task<GuiaRemitenteDTO> CreateAsync(GuiaRemitenteCreateDTO dto)
        {
            // Mapear todos los objetos
            var remitente = _mapper.Map<DOMAIN.Entities.Remitente>(dto.Remitente);
            var destinatario = _mapper.Map<DOMAIN.Entities.Destinatario>(dto.Destinatario);
            var proveedor = _mapper.Map<DOMAIN.Entities.Proveedor>(dto.Proveedor);
            //var conductor = _mapper.Map<DOMAIN.Entities.Conductor>(dto.DatosEnvio.Conductor);
            var conductor=dto.DatosEnvio.Conductor.Select(p => _mapper.Map<DOMAIN.Entities.Conductor>(p)).ToList();
            //var transporte = _mapper.Map<DOMAIN.Entities.Transporte>(dto.DatosEnvio.UnidadTransporte);
            var transporte=dto.DatosEnvio.UnidadTransporte.Select(p => _mapper.Map<DOMAIN.Entities.Transporte>(p)).ToList();
            var guiaRemision = _mapper.Map<DOMAIN.Entities.GuiaRemision>(dto);
            var datosEnvio = _mapper.Map<DOMAIN.Entities.DatosEnvio>(dto.DatosEnvio);
            var origen = _mapper.Map<DOMAIN.Entities.Origen>(dto.Origen);
            //var destino = _mapper.Map<DOMAIN.Entities.Destino>(dto.Destino);
            var destino = dto.Destino.Select(p => _mapper.Map<DOMAIN.Entities.Destino>(p)).ToList();
            var detalles = dto.Productos.Select(p => _mapper.Map<DOMAIN.Entities.GuiaDetalle>(p)).ToList();
            var docRelacionados = dto.DocRelacionado?.Select(p => _mapper.Map<DOMAIN.Entities.DocRelacionado>(p)).ToList();
            var emailDestinatarioGuia = dto.Destinatario.EmailDestinatarioGuia.Select(p => _mapper.Map<DOMAIN.Entities.EmailDestinatarioGuid>(p)).ToList();

            // Llamar al repositorio
            var id = await _repository.CreateGuiaRemisionTransactionalAsync(
                remitente, destinatario, proveedor, conductor, transporte,
                guiaRemision, datosEnvio, origen, destino, detalles, docRelacionados,emailDestinatarioGuia
            );
            //SE obtiene la guia de remision creada
            var guiaCreada = await _repository.GetByIdAsync(id);
            return _mapper.Map<GuiaRemitenteDTO>(guiaCreada);
        }

        public async Task<GuiaRemitenteUpdateDTO> UpdateAsync(int id, GuiaRemitenteUpdateDTO guiaRemisionDto)
        {
            var existingGuiaRemision = await _repository.GetByIdAsync(id);
            if (existingGuiaRemision == null)
            {
                throw new KeyNotFoundException("Guia de remisión no encontrada");
            }
            _mapper.Map(guiaRemisionDto, existingGuiaRemision);
            await _repository.UpdateAsync(existingGuiaRemision);
            return guiaRemisionDto;
        }
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado)
        {
            await _repository.UpdateRespuestaTicketAsync(guiaId, ticket, estado);
        }
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO guia, string ruc)
        {
            try
            {
                if (guia.DocRelacionado?.Count > 3)
                {
                    return (false, "Solo se permite hasta 3 documentos relacionados como referencia en la guía de remisión transportista.");
                }
                    var ventaConfig = _ventaConfigRepository.GetByRucAsync(ruc).Result;
                if (ventaConfig == null || string.IsNullOrEmpty(ventaConfig.Ruc))
                {
                    return (false, $"El RUC principal ingresado {ruc} no tiene acceso al facturador. Pruebe con el Número de RUC del emisor del documento que se está relacionando al comprobante Guía de remisión remitente.");
                }
                if (guia.Remitente.Ruc != ventaConfig.Ruc)
                    return (false, $"El RUC del remitente ({guia.Remitente.Ruc}) no coincide con el RUC de la empresa ({ventaConfig.Ruc}).");
                _ = guia.TipoTRansporteCode18();
                _ = guia.TipoTrasladoCode20();
                _ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento);
                //if (string.IsNullOrWhiteSpace(guia.Observacion))
                //    return (false, "La observación no puede estar vacía.");
                //Si tipo de transporte es PUBLICO, entonces DatosEnvio.rucEmpresaCurrier debe ser obligatorio
                if (guia.TipoTransporte=="PUBLICO")
                {
                    if (guia.DatosEnvio.RucEmpresaCurrier.Length != 11) { return (false, "Si tipoTransporte es PUBLICO, entonces debes completar los 11 dígitos de rucEmpresaCurrier."); }
                    if (string.IsNullOrWhiteSpace(guia.DatosEnvio.RazonSocialCurrier)) { return (false, "Si tipoTransporte es PUBLICO, La razonSocialCurrier no puede estar vacía"); }
                    if (string.IsNullOrWhiteSpace(guia.DatosEnvio.RegistroMtcCurrier)) { return (false, "Si tipoTransporte es PUBLICO, el registroMtcCurrier no puede estar vacío"); }
                }
                //if (guia.)
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
