using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.VentaConfig;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuiaRemitenteController : ControllerBase
    {
        private readonly IVentaConfigService _ventaConfigService;
        private readonly IGuiaRemitenteUBLService _guiaRemisionUBLService;
        private readonly IGuiaRemitenteService _guiasRemisionBDService;
        private readonly IEnvioService _envioService;
        public GuiaRemitenteController(IVentaConfigService ventaConfigService, IGuiaRemitenteUBLService guiaRemisionUBLService, IGuiaRemitenteService guiaRemisionBDService, IEnvioService envioService)
        {
            _ventaConfigService = ventaConfigService;
            _guiaRemisionUBLService = guiaRemisionUBLService;
            _guiasRemisionBDService = guiaRemisionBDService;
            _envioService = envioService;
        }
        [HttpGet]
        [Route("obtener-token/{ruc}")]
        public async Task<ActionResult<TokenResponseDTO>> ObtenerToken(string ruc)
        {
            var token = await _ventaConfigService.ObtenerTokenAsync(ruc);
            return Ok(token);
        }
        [HttpPost]
        [Route("{ruc}")]
        [SwaggerOperation(
        Summary = "Registrar una guía de remisión remitente",
        Description = "Registra una nueva guía de remisión y la envía a EFACT.",
        OperationId = "enviar_guia_remitente")]
        [ProducesResponseType(typeof(SendGuiaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EnviarGuiaRemision(string ruc, [FromBody] GuiaRemitenteCreateDTO doc)
        {
            if (doc == null)
            {
                return BadRequest(new BadRequestResponseDTO
                {
                    Error = "Error en datos enviados",
                    Detalle = "La guía de remisión no puede ser nula."
                });
            }

            var resultado = await _envioService.ProcesarGuiaRemisionAsync(ruc, doc);

            return resultado switch
            {
                SendGuiaResponseDTO ok => Created(string.Empty, ok),
                BadRequestResponseDTO bad => BadRequest(bad),
                InternalErrorResponseDTO error => StatusCode(500, error),
                _ => StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error desconocido",
                    Detalle = "Ocurrió un error desconocido al procesar la solicitud."
                })
            };
        }
        //Listar guias de remision desde BBDD
        [HttpGet]
        [Route("listar")]
        [SwaggerOperation(
        Summary = "Listar guías de remisión remitente",
        Description = "Lista guías de remisión remitente desde la bbdd",
        OperationId = "listar_guia_remitente")]
        [ProducesResponseType(typeof(GuiaTransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GuiaRemitenteDTO>>> GetAll()
        {
            try
            {
                var result = await _guiasRemisionBDService.GetAllAsync();
                if (result == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = "No se pudo obtener la lista de guías de remisión."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar guías de remisión.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar guía de remisión remitente por ID",
        Description = "Busca guía de remisión remitente por ID desde la bbdd",
        OperationId = "buscar_guia_remitente")]
        [ProducesResponseType(typeof(GuiaTransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GuiaRemitenteDTO>> GetById(int id)
        {
            try
            {
                var guia = await _guiasRemisionBDService.GetByIdAsync(id);
                if (guia == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener guía de remisión con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(guia);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar guía de remisión remitente.",
                    Detalle = errorMsg
                });
            }
        }
    }

}
