using GuiaRemision.Services.DocumentEfact;
using GuiaRemision.Services.GuiaRemision;
using GuiaRemision.Services.VentaConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GuiaRemision.Controllers
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
        //[Route("obtener-guia-pdf/{ruc}/{tipoGuia}/{numeroGuia}")]
        [Route("pdf/{ruc}/{tipoGuia}/{numeroGuia}")]
        public async Task<ActionResult> ObtenerPdf(string ruc, string tipoGuia, string numeroGuia)
        {
            var token = await _ventaConfigService.ObtenerTokenAsync(ruc);
            if (string.IsNullOrEmpty(token.AccessToken))
                return Unauthorized("No se pudo obtener un token válido para EFACT.");
            var guid = await _documentService.ObtenerGuidDesdeTicketAsync(ruc + "-" + tipoGuia + "-" + numeroGuia, token.AccessToken); 
            var pdfBytes = await _documentService.ObtenerPdfAsync(guid, token.AccessToken);
            return File(pdfBytes, "application/pdf", $"{numeroGuia}.pdf");
        }
        [HttpGet]
        [Route("xml/{ruc}/{tipoGuia}/{numeroGuia}")]
        public async Task<ActionResult> ObtenerXml(string ruc, string tipoGuia, string numeroGuia)
        {
            var token = await _ventaConfigService.ObtenerTokenAsync(ruc);
            if (string.IsNullOrEmpty(token.AccessToken))
            {
                return Unauthorized("No se pudo obtener un token válido para EFACT.");
            }
            var guid = await _documentService.ObtenerGuidDesdeTicketAsync(ruc + "-" + tipoGuia + "-" + numeroGuia, token.AccessToken);
            var xmlBytes = await _documentService.ObtenerXmlAsync(guid, token.AccessToken);
            return File(xmlBytes, "application/xml", $"{numeroGuia}.xml");
        }
        [HttpGet]
        [Route("cdr/{ruc}/{tipoGuia}/{numeroGuia}")]
        public async Task<ActionResult> ObtenerCdr(string ruc, string tipoGuia, string numeroGuia)
        {
            var token = await _ventaConfigService.ObtenerTokenAsync(ruc);
            if (string.IsNullOrEmpty(token.AccessToken))
            {
                return Unauthorized("No se pudo obtener un token válido para EFACT.");
            }
            var guid = await _documentService.ObtenerGuidDesdeTicketAsync(ruc + "-" + tipoGuia + "-" + numeroGuia, token.AccessToken);
            var cdrBytes = await _documentService.ObtenerCdrAsync(guid, token.AccessToken);
            return File(cdrBytes, "application/cdr", $"{numeroGuia}.cdr");
        }
        [HttpGet]
        [Route("pdftk/{ticket}")]
        public async Task<ActionResult> ObtenerPdfTicket(string ticket)
        {
            var guia = await _guiaRemitente.GetByTicketAsync(ticket);
            var token = await _ventaConfigService.ObtenerTokenAsync(guia.Ruc);
            if (string.IsNullOrEmpty(token.AccessToken))
                return Unauthorized("No se pudo obtener un token válido para EFACT.");
            var pdfBytes = await _documentService.ObtenerPdfAsync(ticket, token.AccessToken);
            return File(pdfBytes, "application/pdf", $"{guia.NumeroGuia}.pdf");
        }
    }
}
