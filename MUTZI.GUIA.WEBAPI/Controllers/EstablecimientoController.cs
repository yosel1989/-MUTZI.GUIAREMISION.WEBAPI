using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("establecimientos")]
    [ApiController]
    public class EstablecimientoController : ControllerBase
    {
        private readonly IEstablecimientoService _establecimientoService;
        public EstablecimientoController(IEstablecimientoService establecimientoService)
        {
            _establecimientoService = establecimientoService;
        }

        /*[HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
        Summary = "Listar remitentes",
        Description = "Lista los remitentes por página. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
        OperationId = "listar_remitente")]
        [ProducesResponseType(typeof(PagedResult<EstablecimientoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<EstablecimientoDTO>>> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var q = Request.Query;
                var remitentes = await _establecimientoService.GetListAsync(pageNumber, pageSize, q);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar remitentes.",
                    Detalle = errorMsg
                });
            }
        }*/

        [HttpGet]
        [Route("listar-select/por-ruc/{ruc}")]
        [SwaggerOperation(
            Summary = "Listar establecimientos para select",
            Description = "Lista los establecimientos para select",
            OperationId = "listar_establecimiento_select")
        ]
        [ProducesResponseType(typeof(EstablecimientoListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListDTO>>> GetToSelect(string ruc)
        {
            try
            {
                var remitentes = await _establecimientoService.GetAllByRucToSelectAsync(ruc);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("listar-sugerido/{ruc}")]
        [SwaggerOperation(
        Summary = "Listar establecimientos sugeridos",
        Description = "Lista los establecimientos sugeridos por número de documento o razón social, de una empresa especifica",
        OperationId = "listar_establecimento_sugerido")]
        [ProducesResponseType(typeof(EstablecimientoListToModalDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListToModalDTO>>> GetBySuggestion(string ruc, [FromQuery] string? search)
        {
            try
            {
                var data = await _establecimientoService.GetListBySuggestionAsync(ruc, search);
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar establecimiento por ID",
        Description = "Buscar objeto por establecimientoID",
        OperationId = "buscar_establecimiento_por_id")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _establecimientoService.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener el establecimiento con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar establecimiento.",
                    Detalle = errorMsg
                });
            }
        }



        /*
        [HttpGet]
        [Route("listar-nombres")]
        [SwaggerOperation(
        Summary = "Listar remitentes para filtro",
        Description = "Lista los remitentes para select específico de filtro.",
        OperationId = "listar_remitente_nombres")]
        [ProducesResponseType(typeof(EstablecimientoNameListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoNameListDTO>>> GetOnlyName()
        {
            try
            {
                var remitentes = await _establecimientoService.GetOnlynameAsync();
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar remitentes.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("listar-sugerido/{numeroDoc}")]
        [SwaggerOperation(
        Summary = "Listar remitentes sugeridos",
        Description = "Lista los remitentes sugeridos por número de documento o razón social",
        OperationId = "listar_remitente_sugerido")]
        [ProducesResponseType(typeof(EstablecimientoListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListDTO>>> GetBySuggestion(string numeroDoc)
        {
            try
            {
                var proveedores = await _establecimientoService.GetListBySuggestion(numeroDoc);
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar remitentes.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar remitente por ID",
        Description = "Buscar objeto por remitenteID",
        OperationId = "buscar_remitente_por_id")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _establecimientoService.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener remitente con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id-para-guia/{id}/{tipoGuia}")]
        [SwaggerOperation(
        Summary = "Buscar remitente por ID y por tipo de guía",
        Description = "Buscar objeto por ID y tipo de guía. En 'tipoGuia', colocar: 'REMITENTE' ó 'TRANSPORTISTA'.",
        OperationId = "buscar_remitente_por_id_guia")]
        [ProducesResponseType(typeof(EstablecimientoForGuiaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoForGuiaDTO>> GetForGuiaById(int id, string tipoGuia)
        {
            try
            {
                var remitente = await _establecimientoService.GetByIdForGuiaAsync(id, tipoGuia);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener remitente con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPost]
        [SwaggerOperation(
        Summary = "Registrar un remitente",
        Description = "Registra un remitente en la bbdd",
        OperationId = "registrar_remitente")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] EstablecimientoCreateDTO createDTO)
        {
            try
            {
                var createdId = await _establecimientoService.CreateAsync(createDTO);
                return StatusCode(201, new CreatedDTO { Id = createdId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Actualizar un remitente",
        Description = "Actualiza remitente en la bbdd",
        OperationId = "actualizar_remitente")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> Update(int id, [FromBody] EstablecimientoUpdateDTO updateDTO)
        {
            try
            {
                var updated = await _establecimientoService.UpdateAsync(id, updateDTO);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Eliminar remitente",
        Description = "Elimina un remitente de la bbdd",
        OperationId = "eliminar_remitente")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _establecimientoService.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [Route("{id}/actualizar-estado")]
        [SwaggerOperation(
        Summary = "Actualizar estado de un remitente",
        Description = "Actualizar el estado de un remitente en la bbdd. 1='activo', 0='inactivo'.",
        OperationId = "actualizar_estado_remitente")]
        [ProducesResponseType(typeof(UpdatedStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] InfoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _establecimientoService.CambiarEstadoAsync(id, infoRequestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar el estado de un remitente.",
                    Detalle = errorMsg
                });
            }
        }*/
    }
}
