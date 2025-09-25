using GuiaRemision.DTOs.Efact;
using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.DTOs.GuiaTransportista;
using GuiaRemision.Services.Envio;
using GuiaRemision.Services.GuiaRemision;
using GuiaRemision.Services.GuiaTransportista;
using GuiaRemision.Services.VentaConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace GuiaRemision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuiaTransportistaController : ControllerBase
    {
        private readonly IGuiaTransportistaService _guiaTransportistaBDService;
        private readonly IEnvioService _envioService;
        public GuiaTransportistaController(IGuiaTransportistaService guiaTransportistaBDService, IEnvioService envioService)
        {
            _guiaTransportistaBDService = guiaTransportistaBDService;
            _envioService = envioService;
        }
        [HttpPost]
        [Route("{ruc}")]
        [SwaggerOperation(
            Summary = "Envía una guía de remisión transportista a EFACT",
            Description = "Este endpoint permite enviar una guía de remisión transportista a EFACT.",
            OperationId = "enviar_guia_transportista"
            )]
        [ProducesResponseType(typeof(EnviarGuiaResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EnviarGuiaTransportista(string ruc, [FromBody] GuiaTransportistaCreateDTO doc)
        {
            if (doc == null)
            {
                return BadRequest(new BadRequestResponseDto
                {
                    Error = "La guía transportista no puede ser nula.",
                    Detalle = "El objeto enviado en el cuerpo de la solicitud es nulo."
                });
            }
            var resultado=await _envioService.ProcesarGuiaTransportistaAsync(ruc, doc);
            return resultado switch
            {
                EnviarGuiaResponseDto ok=> Created(string.Empty,ok),
                BadRequestResponseDto bad=>BadRequest(bad),
                InternalErrorResponseDto error=>StatusCode(500, error),
                _ => StatusCode(500, new InternalErrorResponseDto
                {
                    Error = "Error desconocido",
                    Detalle = "Ocurrió un error desconocido al procesar la solicitud."
                })
            };
        }
            [HttpGet]
        [Route("listar-por-id/{id}")]
        public async Task<ActionResult<GuiaTransportistaDTO>> GetById(int id)
        {
            var result = await _guiaTransportistaBDService.GetByIdAsync(id);
            if (result == null)
                return NotFound($"No se encontró la guía transportista con ID {id}.");
            return Ok(result);
        }
    }
}
