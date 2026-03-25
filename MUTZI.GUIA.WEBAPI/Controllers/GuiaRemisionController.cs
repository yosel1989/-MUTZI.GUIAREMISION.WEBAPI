using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GuiaRemisionController : ControllerBase
    {
        private readonly IGuiaRemitenteService _guiasRemisionBDService;
        private readonly IEnvioService _envioService;
        public GuiaRemisionController(IGuiaRemitenteService guiaRemisionBDService, IEnvioService envioService)
        {
            _guiasRemisionBDService = guiaRemisionBDService;
            _envioService = envioService;
        }
        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
        Summary = "Listar guías de remisión y transportista",
        Description = "Lista las guías de remisión y transportista. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
        OperationId = "listar_guias")]
        [ProducesResponseType(typeof(PagedResult<GuiaListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<PagedResult<GuiaListDTO>>> GetAllGuias(int pageNumber, int pageSize)
        {
            try
            {
                var q = Request.Query;
                var result = await _guiasRemisionBDService.GetListAsync(pageNumber, pageSize, q);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;

                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar guías.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("exportar")]
        [SwaggerOperation(
        Summary = "Exportar guías de remisión y transportista",
        Description = "Exporta las guías de remisión y transportista.",
        OperationId = "exportar_guias")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ExportGuias()
        {
            try
            {
                var q = Request.Query;
                byte[] fileBytes = await _guiasRemisionBDService.ExportGuiasAsync(1, 10, q);
                return File(fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "reporte-guias-remision.xlsx");
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;

                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al exportar guías.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar guía de remisión por ID",
        Description = "Busca guía de remisión remitente o transportista por ID desde la bbdd",
        OperationId = "buscar_guia_remision")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GetById(int id)
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
                    Error = "Error al buscar guía de remisión.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPost]
        [Route("remitente/{ruc}")]
        [SwaggerOperation(
        Summary = "Registrar guía remitente",
        Description = "Registra una nueva guía de remisión remitente y la envía a EFACT.",
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
                    Detalle = "La guía de remisión remitente no puede ser nula."
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
        [HttpPost]
        [Route("transportista/{ruc}")]
        [SwaggerOperation(
            Summary = "Registrar guía transportista",
            Description = "Registra una nueva guía de remisión transportista y la envía a EFACT.",
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
                    Error = "Error en datos enviados",
                    Detalle = "La guía de remisión transportista no puede ser nula."
                });
            }
            var resultado = await _envioService.ProcesarGuiaTransportistaAsync(ruc, doc);
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
        [HttpPut]
        [Route("{ruc}/{guiaId}")]
        [SwaggerOperation(
        Summary = "Reenviar guia remisión",
        Description = "Reenvia una guía existente(REMITENTE ó TRANSPORTISTA) al facturador electrónico.",
        OperationId = "reenviar_guia_remision")]
        [ProducesResponseType(typeof(SendGuiaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReenviarGuiaRemision(string ruc, int guiaId)
        {
            var resultado = await _envioService.ReenviarGuiaRemisionAsync(ruc, guiaId);

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
    }
}
