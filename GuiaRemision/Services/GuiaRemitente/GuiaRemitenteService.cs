using AutoMapper;
using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.Models.DBMS;
using GuiaRemision.Repositories.GuiaRemitente;
using System;
using System.Text.Json;

namespace GuiaRemision.Services.GuiaRemision
{
    public class GuiaRemitenteService : IGuiaRemitenteService
    {
        private readonly IGuiaRemitenteRepository _repository;
        private readonly IMapper _mapper;
        public GuiaRemitenteService(IGuiaRemitenteRepository repository, IMapper mapper)
        {
            _repository = repository;
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
            var remitente = _mapper.Map<Models.DBMS.Remitente>(dto.Remitente);
            var destinatario = _mapper.Map<Models.DBMS.Destinatario>(dto.Destinatario);
            var proveedor = _mapper.Map<Models.DBMS.Proveedor>(dto.Proveedor);
            var conductor = _mapper.Map<Models.DBMS.Conductor>(dto.DatosEnvio.Conductor);
            var transporte = _mapper.Map<Models.DBMS.Transporte>(dto.DatosEnvio.UnidadTransporte);
            var guiaRemision = _mapper.Map<Models.DBMS.GuiaRemision>(dto);
            var datosEnvio = _mapper.Map<Models.DBMS.DatosEnvio>(dto.DatosEnvio);
            var origen = _mapper.Map<Models.DBMS.Origen>(dto.Origen);
            //var destino = _mapper.Map<Models.DBMS.Destino>(dto.Destino);
            var destino = dto.Destino.Select(p => _mapper.Map<Models.DBMS.Destino>(p)).ToList();
            var detalles = dto.Productos.Select(p => _mapper.Map<Models.DBMS.GuiaDetalle>(p)).ToList();

            // Llamar al repositorio
            var id = await _repository.CreateGuiaRemisionTransactionalAsync(
                remitente, destinatario, proveedor, conductor, transporte,
                guiaRemision, datosEnvio, origen, destino, detalles
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
        public (bool EsValido, string Error) ValidarDatosGuiaRemitente(GuiaRemitenteCreateDTO guia)
        {
            try
            {
                _ = guia.TipoTRansporteCode18();
                _ = guia.TipoTrasladoCode20();
                _ = guia.DocumentoAdicinalTipo();
                _ = guia.getTipoCodigoDocumento(guia.Destinatario.TipoDocumento);
                if (string.IsNullOrWhiteSpace(guia.Observacion))
                    return (false, "La observación no puede estar vacía.");
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
