using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.APPLICATION.DTOs.Persona;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("entities")]
    [ApiController]
 
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _service;
        public EntityController(IEntityService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar una entidad",
            Description = "Registrar una entidad con su tipo y roles",
            OperationId = "entity_create"
        )]
        [ProducesResponseType(typeof(IEnumerable<EntityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EntityDTO>>> Create([FromBody] EntityCreateDTO request)
        {
            try
            {
                var newEntity = await _service.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, newEntity);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar entidad.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Buscar personal por id",
            Description = "Buscar la información de un personal por su id",
            OperationId = "buscar_personal_por_id"
        )]
        [ProducesResponseType(typeof(PersonaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDTO>> GetById(int id)
        {
            try
            {
                var personal = await _service.GetByIdAsync(id);
                return Ok(personal);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar al personal",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("collection/{role}/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Obtener entidades",
            Description = "Listar entidades por rol. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize' y el rol con el cual tengan relación.",
            OperationId = "entity_collection"
        )]
        [ProducesResponseType(typeof(PagedResult<EntityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<EntityDTO>>> GetAll(string role, int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var q = Request.Query;
                var result = await _service.GetDataTableAsync(pageNumber, pageSize, search, role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar proveedores.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("list/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar entidades",
            Description = "Listar entidades. Para ello se debe ingresar como parámetro: el número de página 'pageNumber'.",
            OperationId = "entity_list"
        )]
        [ProducesResponseType(typeof(PagedResult<EntityListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<EntityListDTO>>> GetList( int pageNumber, int pageSize, [FromQuery] string? search )
        {
            try
            {
                var q = Request.Query;
                var result = await _service.GetListAsync(pageNumber, pageSize, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar proveedores.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
