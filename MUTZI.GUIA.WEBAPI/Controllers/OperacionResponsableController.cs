using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.OperacionResponsable;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("operacion-responsable")]
    [ApiController]
    [Authorize]
    public class OperacionResponsableController : ControllerBase
    {
        private readonly IOperacionResponsableService _service;
        public OperacionResponsableController(IOperacionResponsableService OperacionResponsableService)
        {
            _service = OperacionResponsableService;
        }

        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar las operaciones para los responsables",
            Description = "Lista las operaciones para responsables por página. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_operaciones_responsable"
        )]
        [ProducesResponseType(typeof(PagedResult<OperacionResponsableDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<OperacionResponsableDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var OperacionResponsablees = await _service.GetListAsync(pageNumber, pageSize, search);
                return Ok(OperacionResponsablees);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar las operaciones.",
                    Detalle = errorMsg
                });
            }
        }

        


        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
            Summary = "Buscar operación por ID",
            Description = "Buscar operación por Id",
            OperationId = "buscar_operacion_por_id"
        )]
        [ProducesResponseType(typeof(OperacionResponsableDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OperacionResponsableDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _service.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener la operación con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar la operacíon.",
                    Detalle = errorMsg
                });
            }
        }

       
        [HttpPost]
        [SwaggerOperation(
        Summary = "Registrar una operación",
        Description = "Registra un operación en la bbdd",
        OperationId = "registrar_operacion_responsable")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] OperacionResponsableCreateDTO createDTO)
        {
            try
            {
                var createdId = await _service.CreateAsync(createDTO);
                return StatusCode(201, new CreatedDTO { Id = createdId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar la operación.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar una operación",
            Description = "Actualiza operación en la bbdd",
            OperationId = "actualizar_operacion"
        )]
        [ProducesResponseType(typeof(OperacionResponsableDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OperacionResponsableDTO>> Update(int id, [FromBody] OperacionResponsableUpdateDTO updateDTO)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateDTO);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar la operación.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet]
        [Route("listar-para-select")]
        [SwaggerOperation(
            Summary = "Listar operaciones para seleccionar",
            Description = "Lista los operaciones activas para seleccionar",
            OperationId = "listar_operaciones_para_select"
        )]
        [ProducesResponseType(typeof(PagedResult<OperacionResponsableDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<OperacionResponsableToSelectDTO>>> GetAllToSelect()
        {
            try
            {
                var OperacionResponsablees = await _service.ListToSelectAsync();
                return Ok(OperacionResponsablees);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar las operaciones.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
