using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GuiaRemisionController : ControllerBase
    {
        private readonly IGuiaRemitenteService _guiasRemisionBDService;
        private readonly IGuiaTransportistaService _guiasTransportistaBDService;
        private readonly IEnvioService _envioService;
        public GuiaRemisionController(IGuiaRemitenteService guiaRemisionBDService, IGuiaTransportistaService guiaTransportistaService, IEnvioService envioService)
        {
            _guiasRemisionBDService = guiaRemisionBDService;
            _guiasTransportistaBDService = guiaTransportistaService;
            _envioService = envioService;
        }

        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar guías de remisión y transportista",
            Description = "Lista las guías de remisión y transportista. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_guia_remision"
        )]
        [Permission("listar_guia_remision")]
        [ProducesResponseType(typeof(PagedResult<GuiaRemisionListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<PagedResult<GuiaRemisionListDTO>>> GetAllGuias(int pageNumber, int pageSize)
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
            OperationId = "exportar_lista_guia_remision"
        )]
        [Permission("exportar_lista_guia_remision")]
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
            OperationId = "buscar_guia_remision_por_id"
        )]
        [Permission("buscar_guia_remision_por_id")]
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
        [Route("registrar-remitente/{ruc}")]
        [SwaggerOperation(
            Summary = "Registrar guía remitente",
            Description = "Registra guía de remisión remitente y lo guarda en la bbdd.",
            OperationId = "registrar_guia_remision"
        )]
        [Permission("registrar_guia_remision")]
        [ProducesResponseType(typeof(GuiaCreatedResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarGuiaRemision(string ruc, [FromBody] GuiaRemitenteCreateDTO doc)
        {
            if (doc == null)
            {
                return BadRequest(new BadRequestResponseDTO
                {
                    Error = "Error en datos enviados",
                    Detalle = "La guía de remisión remitente no puede ser nula."
                });
            }

            //var resultado = await _envioService.ProcesarGuiaRemisionAsync(ruc, doc);
            var resultado = await _guiasRemisionBDService.CreateAsync(ruc, doc);
            return resultado switch
            {
                GuiaCreatedResponseDTO ok => Created(string.Empty, ok),
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
        [Route("registrar-transportista/{ruc}")]
        [SwaggerOperation(
            Summary = "Registrar guía transportista",
            Description = "Registra guía de remisión transportista en la bbdd.",
            OperationId = "registrar_guia_transportista"
        )]
        [ProducesResponseType(typeof(GuiaCreatedResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RegistrarGuiaTransportista(string ruc, [FromBody] GuiaTransportistaCreateDTO doc)
        {
            if (doc == null)
            {
                return BadRequest(new BadRequestResponseDTO
                {
                    Error = "Error en datos enviados",
                    Detalle = "La guía de remisión transportista no puede ser nula."
                });
            }
            var resultado = await _guiasTransportistaBDService.CreateAsync(ruc, doc);
            return resultado switch
            {
                GuiaCreatedResponseDTO ok => Created(string.Empty, ok),
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
        [Route("actualizar-remitente{id}")]
        [SwaggerOperation(
            Summary = "Actualizar guía remitente",
            Description = "Actualiza guía de remisión remitente por ID.",
            OperationId = "editar_guia_remision"
        )]
        [Permission("editar_guia_remision")]
        [ProducesResponseType(typeof(GuiaRemitenteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GuiaRemitenteDTO>> UpdateGuiaRemitente(int id, [FromBody] GuiaRemitenteUpdateDTO guiaDTO)
        {
            try
            {
                var updated = await _guiasRemisionBDService.UpdateAsync(id, guiaDTO);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar guía de remisión remitente.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("actualizar-transportista{id}")]
        [SwaggerOperation(
        Summary = "Actualizar guía transportista",
        Description = "Actualiza guía de remisión transportista por ID.",
        OperationId = "actualizar_guia_transportista")]
        [ProducesResponseType(typeof(GuiaTransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GuiaTransportistaDTO>> UpdateGuiaTransportista(int id, [FromBody] GuiaTransportistaUpdateDTO guiaDTO)
        {
            try
            {
                var updated = await _guiasTransportistaBDService.UpdateAsync(id, guiaDTO);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar guía de remisión transportista.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPost]
        [Route("{ruc}/enviar/{guiaId}")]
        [SwaggerOperation(
            Summary = "Enviar guia remisión",
            Description = "Envía una guía existente(REMITENTE ó TRANSPORTISTA) al facturador electrónico.",
            OperationId = "aprobar_guia_remision"
        )]
        [Permission("aprobar_guia_remision")]
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

        [HttpPut]
        [Route("{id}/confirmar")]
        [SwaggerOperation(
            Summary = "Confirmar guia remisión",
            Description = "Confirma una guia de remisión y le pone el estado 'confirmado'.",
            OperationId = "confirmar_guia_remision"
        )]
        [Permission("confirmar_guia_remision")]
        //[ProducesResponseType(typeof(SendGuiaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmarGuiaRemision(int id)
        {
            var resultado = await _guiasRemisionBDService.ConfirmarAsync(id);

            return Ok(resultado);
        }
    }
}
