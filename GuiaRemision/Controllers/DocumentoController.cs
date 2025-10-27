using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Documento;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.VentaConfig;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly IVentaConfigService _ventaConfigService;
        private readonly IDocumentoService _documentService;
        private readonly IGuiaRemitenteService _guiaRemitente;
        public DocumentoController(IVentaConfigService ventaConfigService,
            IDocumentoService documentEfactService, IGuiaRemitenteService guiaRemitenteService)
        {
            _ventaConfigService = ventaConfigService;
            _documentService = documentEfactService;
            _guiaRemitente = guiaRemitenteService;
        }
        [HttpGet]
        [Route("pdftk/{ticket}")]
        [SwaggerOperation(
        Summary = "Obtener PDF desde ticket",
        Description = "Obtiene un archivo PDF de la guía emitida.",
        OperationId = "guia_pdf_tk")]
        [ProducesResponseType(typeof(EfactResponseDTO), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObtenerPdfTicket(string ticket)
        {
            var guia = await _guiaRemitente.GetByTicketAsync(ticket);
            if (guia == null || string.IsNullOrEmpty(guia.Ruc))
                return NotFound(new BadRequestResponseDTO
                {
                    Error = "Guía no encontrada",
                    Detalle = $"No se encontró una guía asociada al ticket: {ticket}"
                });
            var token = await _ventaConfigService.ObtenerTokenAsync(guia.Ruc);
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Token inválido",
                    Detalle = "No se pudo obtener un token válido para EFACT."
                });
            }
            if (string.IsNullOrEmpty(token.AccessToken))
                return Unauthorized("No se pudo obtener un token válido para EFACT.");
            //var pdfBytes = await _documentService.ObtenerPdfAsync(ticket, token.AccessToken);
            var (pdfBytes, estadoDoc, errorDoc) = await _documentService.ObtenerDocumentoAsync("pdf", ticket, token.AccessToken);
            if (pdfBytes != null)
                return File(pdfBytes, "application/pdf", $"{guia.NumeroGuia}.pdf");
            if (estadoDoc != null)
                return StatusCode(StatusCodes.Status202Accepted, estadoDoc);
            return StatusCode(500, new InternalErrorResponseDTO 
            {
                Error = "Error al obtener PDF.",
                Detalle = errorDoc
            });
        }

        [HttpGet("pdf/{ruc}/{tipoGuia}/{numeroGuia}")]
        [SwaggerOperation(
        Summary = "Obtener guía en PDF",
        Description = "Obtiene un archivo PDF de la guía emitida. (En tipoGuia solo acepta valores de: 'REMITENTE' ó 'TRANSPORTISTA')",
        OperationId = "guia_pdf")]
        [ProducesResponseType(typeof(EfactResponseDTO), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObtenerPdf(string ruc, string tipoGuia, string numeroGuia)
        {
            var (exito, archivo, estado, error) = await _documentService.ObtenerDocumentoCompletoAsync("pdf", ruc, tipoGuia, numeroGuia);

            if (exito && archivo != null)
                return File(archivo, "application/pdf", $"{numeroGuia}.pdf");

            if (estado != null && estado.Code == "-9998")
                return StatusCode(StatusCodes.Status202Accepted, estado);

            if (estado != null)
                return StatusCode(StatusCodes.Status400BadRequest, estado);

            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }

        [HttpGet("xml/{ruc}/{tipoGuia}/{numeroGuia}")]
        [SwaggerOperation(
        Summary = "Obtener guía en XML",
        Description = "Obtiene un archivo XML de la guía emitida. (En tipoGuia solo acepta valores de: 'REMITENTE' ó 'TRANSPORTISTA')",
        OperationId = "guia_xml")]
        [ProducesResponseType(typeof(EfactResponseDTO), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObtenerXml(string ruc, string tipoGuia, string numeroGuia)
        {
            var (exito, archivo, estado, error) = await _documentService.ObtenerDocumentoCompletoAsync("xml", ruc, tipoGuia, numeroGuia);

            if (exito && archivo != null)
                return File(archivo, "application/xml", $"{numeroGuia}.xml");

            if (estado != null && estado.Code == "-9998")
                return StatusCode(StatusCodes.Status202Accepted, estado);

            if (estado != null)
                return StatusCode(StatusCodes.Status400BadRequest, estado);

            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }

        [HttpGet("cdr/{ruc}/{tipoGuia}/{numeroGuia}")]
        [SwaggerOperation(
        Summary = "Obtener guía en CDR",
        Description = "Obtiene un archivo CDR de la guía emitida. (En tipoGuia solo acepta valores de: 'REMITENTE' ó 'TRANSPORTISTA')",
        OperationId = "guia_cdr")]
        [ProducesResponseType(typeof(EfactResponseDTO), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObtenerCdr(string ruc, string tipoGuia, string numeroGuia)
        {
            var (exito, archivo, estado, error) = await _documentService.ObtenerDocumentoCompletoAsync("cdr", ruc, tipoGuia, numeroGuia);

            if (exito && archivo != null)
                return File(archivo, "application/cdr", $"{numeroGuia}.cdr");

            if (estado != null && estado.Code == "-9998")
                return StatusCode(StatusCodes.Status202Accepted, estado);

            if (estado != null)
                return StatusCode(StatusCodes.Status400BadRequest, estado);

            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}
