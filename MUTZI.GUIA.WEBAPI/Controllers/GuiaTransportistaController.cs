using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
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
        [ProducesResponseType(typeof(SendGuiaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EnviarGuiaTransportista(string ruc, [FromBody] GuiaTransportistaCreateDTO doc)
        {
            if (doc == null)
            {
                return BadRequest(new BadRequestResponseDTO
                {
                    Error = "La guía transportista no puede ser nula.",
                    Detalle = "El objeto enviado en el cuerpo de la solicitud es nulo."
                });
            }
            var resultado=await _envioService.ProcesarGuiaTransportistaAsync(ruc, doc);
            return resultado switch
            {
                SendGuiaResponseDTO ok=> Created(string.Empty,ok),
                BadRequestResponseDTO bad=>BadRequest(bad),
                InternalErrorResponseDTO error=>StatusCode(500, error),
                _ => StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error desconocido",
                    Detalle = "Ocurrió un error desconocido al procesar la solicitud."
                })
            };
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar guía de remisión transportista por ID",
        Description = "Busca guía de remisión transportista por ID desde la bbdd",
        OperationId = "buscar_guia_transportista")]
        [ProducesResponseType(typeof(GuiaTransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GuiaTransportistaDTO>> GetById(int id)
        {
            try
            {
                var result = await _guiaTransportistaBDService.GetByIdAsync(id);
                if (result == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = $"No se pudo obtener guia transportista con el ID: {id}.",
                        Detalle = "No se encontraron registros, verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar guía de remisión transportista.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
