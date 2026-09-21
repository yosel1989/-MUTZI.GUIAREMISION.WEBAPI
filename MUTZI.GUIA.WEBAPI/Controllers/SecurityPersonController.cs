using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("security-personal")]
    [ApiController]
    [Authorize]
    public class SecurityPersonController : ControllerBase
    {
        private readonly ISecurityPersonService _service;
        public SecurityPersonController(ISecurityPersonService establecimientoService)
        {
            _service = establecimientoService;
        }

        [HttpGet]
        [Route("{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar personales de seguridad",
            Description = "Lista los personales de seguridad. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_establecimiento"
        )]
        [Permission("listar_establecimiento")]
        [ProducesResponseType(typeof(PagedResult<SecurityPersonDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<SecurityPersonDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var remitentes = await _service.GetListAsync(pageNumber, pageSize, search);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar personales de seguridad.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Buscar personal de seguridad por ID",
            Description = "Buscar objeto por ID",
            OperationId = "security_person_get_by_id"
        )]
        [Permission("security_person_get_by_id")]
        [ProducesResponseType(typeof(SecurityPersonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SecurityPersonDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _service.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener el personal de seguridad con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar personal de seguridad.",
                    Detalle = errorMsg
                });
            }
        }

        

        /*[HttpPut]
        [Route("{id}/toggle-active")]
        [SwaggerOperation(
            Summary = "Actualizar estado de un personal de seguridad",
            Description = "Actualizar el estado de un personal de seguridad en la bbdd. 1='activo', 0='inactivo'.",
            OperationId = "security_person_toggle_active"
        )]
        [Permission("security_person_toggle_active")]
        [ProducesResponseType(typeof(ResponseDTO<ToggleActiveResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _service.(id, infoRequestDto);
                return Ok(new ResponseDTO<ToggleActiveResponseDTO>
                {
                    Data = result,
                    Detalle = $"Se {(result.Active ? "activó" : "desactivó")} el establecimiento COD-{id.ToString("D4")} con éxito."
                });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar el estado de un establecimiento.",
                    Detalle = errorMsg
                });
            }
        }*/

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar un personal de seguridad",
            Description = "Registra un personal de seguridad en la bbdd",
            OperationId = "security_person_create"
        )]
        //[Permission("security_person_create")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] SecurityPersonCreateDTO createDTO)
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
                    Error = "Error al registrar personal de seguridad.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar un personal de seguridad",
            Description = "Actualiza un personal de seguridad en la bbdd",
            OperationId = "security_person_update"
        )]
        [Permission("security_person_update")]
        [ProducesResponseType(typeof(SecurityPersonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SecurityPersonDTO>> Update(int id, [FromBody] SecurityPersonUpdateDTO updateDTO)
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
                    Error = "Error al actualizar al personal de seguridad.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
