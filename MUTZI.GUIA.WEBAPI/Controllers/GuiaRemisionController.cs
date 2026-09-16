using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemision;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Documento;
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
        private readonly ICatalogoService _catalogoService;
        private readonly IGuiaTransportistaService _guiasTransportistaBDService;
        private readonly IEnvioService _envioService;
        private readonly IDocumentoService _documentService;
        public GuiaRemisionController(
            IGuiaRemitenteService guiaRemisionBDService, 
            IGuiaTransportistaService guiaTransportistaService,
            ICatalogoService catalogoService,
            IEnvioService envioService,
            IDocumentoService documentService
        )
        {
            _guiasRemisionBDService = guiaRemisionBDService;
            _guiasTransportistaBDService = guiaTransportistaService;
            _catalogoService = catalogoService;
            _envioService = envioService;
            _documentService = documentService;
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

        [HttpGet]
        [Route("buscar-por-uuid/{guiaRemisionUuid:Guid}")]
        [SwaggerOperation(
            Summary = "Buscar guía de remisión por su Uuid",
            Description = "Busca guía de remisión remitente o transportista por Uuid desde la bbdd",
            OperationId = "buscar_guia_remision_por_uuid"
        )]
        [Permission("buscar_guia_remision_por_uuid")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GetByUuid(Guid guiaRemisionUuid)
        {
            try
            {
                var guia = await _guiasRemisionBDService.GetByUuidAsync(guiaRemisionUuid);
                if (guia == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener guía de remisión con el ID: {guiaRemisionUuid}. Verifique si el ID existe en la bbdd."
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
            OperationId = "actualizar_guia_transportista"
        )]
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
            var resultado = await _guiasRemisionBDService.AprobarEnviarAsync(guiaId);

            //var resultado = await _envioService.ReenviarGuiaRemisionAsync(ruc, guiaId);

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
        [Route("{id}/aprobar")]
        [SwaggerOperation(
            Summary = "Aprobar y enviar guia remisión",
            Description = "Aprueba y envía una guía existente(REMITENTE ó TRANSPORTISTA) al facturador electrónico.",
            OperationId = "aprobar_guia_remision"
        )]
        [Permission("aprobar_guia_remision")]
        [ProducesResponseType(typeof(SendGuiaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AprobarGuiaRemision(int id)
        {

            var resultado = await _guiasRemisionBDService.AprobarEnviarAsync(id);

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
            try
            {
                var resultado = await _guiasRemisionBDService.ConfirmarAsync(id);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                var errorResponse = new BadRequestResponseDTO
                {
                    Error = "Operación invalida",
                    Detalle = ex.Message
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPut]
        [Route("{id}/anular")]
        [SwaggerOperation(
            Summary = "Anular guia remisión",
            Description = "Anular una guia de remisión y le pone el estado 'anulada'. Solo si la guía no fue emitida",
            OperationId = "anular_guia_remision"
        )]
        [Permission("anular_guia_remision")]
        //[ProducesResponseType(typeof(SendGuiaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AnularGuiaRemision(int id, [FromBody] GuiaRemisionCambioEstadoDTO request)
        {
            try
            {
                var resultado = await _guiasRemisionBDService.AnularAsync(request.GuiaRemisionId, request.Descripcion);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                var errorResponse = new BadRequestResponseDTO
                {
                    Error = "Operación invalida",
                    Detalle = ex.Message
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPut]
        [Route("{id}/rechazar")]
        [SwaggerOperation(
            Summary = "Rechazar guia remisión",
            Description = "Rechazar una guia de remisión y le pone el estado 'rechazada'. Solo si la guía no fue emitida",
            OperationId = "rechazar_guia_remision"
        )]
        [Permission("rechazar_guia_remision")]
        //[ProducesResponseType(typeof(SendGuiaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RechazarGuiaRemision(int id, [FromBody] GuiaRemisionCambioEstadoDTO request)
        {
            try
            {
                var resultado = await _guiasRemisionBDService.RechazarAsync(request.GuiaRemisionId, request.Descripcion);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                var errorResponse = new BadRequestResponseDTO
                {
                    Error = "Operación invalida",
                    Detalle = ex.Message
                };
                return BadRequest(errorResponse);
            }
        }




        [HttpGet("pdf/{guiaRemisionId}/{entityId}")]
        [SwaggerOperation(
            Summary = "Obtener guía en PDF",
            Description = "Obtiene un archivo PDF de la guía emitida. (En tipoGuia solo acepta valores de: 'REMITENTE' ó 'TRANSPORTISTA')",
            OperationId = "guia_remision_get_pdf"
        )]
        [ProducesResponseType(typeof(EfactResponseDTO), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPdf(int guiaRemisionId, int entityId)
        {
            byte[] archivo = await _guiasRemisionBDService.GetDocumentAsync(guiaRemisionId, entityId);

            return File(archivo, "application/pdf", $"aaa.pdf");

            /*if (exito && archivo != null)
                return File(archivo, "application/pdf", $"{numeroGuia}.pdf");

            if (estado != null && estado.Code == "-9998")
                return StatusCode(StatusCodes.Status202Accepted, estado);

            if (estado != null)
                return StatusCode(StatusCodes.Status400BadRequest, estado);

            return StatusCode(StatusCodes.Status500InternalServerError, error);*/
        }




        [HttpGet("pdf-internal/{guiaRemisionId}/{entityId}")]
        [SwaggerOperation(
            Summary = "Obtener guía en PDF formato interno",
            Description = "Obtiene un archivo PDF de la guía emitida con un formato interno. (En tipoGuia solo acepta valores de: 'REMITENTE' ó 'TRANSPORTISTA')",
            OperationId = "guia_remision_get_pdf"
        )]
        [ProducesResponseType(typeof(EfactResponseDTO), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPdfInternal(int guiaRemisionId, int entityId)
        {
            try
            {
                byte[] archivo = await _guiasRemisionBDService.GetDocumentInternalAsync(guiaRemisionId, entityId);
                return File(archivo, "application/pdf", $"aaa.pdf");
            }
            catch (Exception e)
            {
                throw;
            }
            

            /*if (exito && archivo != null)
                return File(archivo, "application/pdf", $"{numeroGuia}.pdf");

            if (estado != null && estado.Code == "-9998")
                return StatusCode(StatusCodes.Status202Accepted, estado);

            if (estado != null)
                return StatusCode(StatusCodes.Status400BadRequest, estado);

            return StatusCode(StatusCodes.Status500InternalServerError, error);*/
        }
    }
}
