using Genexs.AWS.Library;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Remitente;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RemitenteController : ControllerBase
    {
        private readonly IRemitenteService _remitenteService;
        public RemitenteController(IRemitenteService remitenteService)
        {
            _remitenteService = remitenteService;
        }
        [HttpGet]
        [Route("listar")]
        [SwaggerOperation(
        Summary = "Listar remitentes",
        Description = "Lista los remitentes",
        OperationId = "listar_remitente")]
        [ProducesResponseType(typeof(RemitenteListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RemitenteListDTO>>> GetAll()
        {
            try
            {
                var remitentes = await _remitenteService.GetAllAsync();
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
        [ProducesResponseType(typeof(RemitenteListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RemitenteListDTO>>> GetBySuggestion(string numeroDoc)
        {
            try
            {
                var proveedores = await _remitenteService.GetListBySuggestion(numeroDoc);
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
        [Route("listar-por-pagina/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
        Summary = "Listar remitentes por página",
        Description = "Lista los remitentes por página. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
        OperationId = "listar_remitente_por_pagina")]
        [ProducesResponseType(typeof(PagedResult<RemitenteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<RemitenteDTO>>> GetAllForPages(int pageNumber, int pageSize)
        {
            try
            {
                var remitentes = await _remitenteService.GetListAsync(pageNumber, pageSize);
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
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar remitente por ID",
        Description = "Buscar objeto por remitenteID",
        OperationId = "buscar_remitente_por_id")]
        [ProducesResponseType(typeof(RemitenteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RemitenteDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _remitenteService.GetByIdAsync(id);
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
        [ProducesResponseType(typeof(RemitenteForGuiaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<RemitenteForGuiaDTO>> GetForGuiaById(int id, string tipoGuia)
        {
            try
            {
                var remitente = await _remitenteService.GetByIdForGuiaAsync(id, tipoGuia);
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
        public async Task<ActionResult<int>> Create([FromBody] RemitenteCreateDTO createDTO)
        {
            try
            {
                var createdId = await _remitenteService.CreateAsync(createDTO);
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
        [ProducesResponseType(typeof(RemitenteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RemitenteDTO>> Update(int id, [FromBody] RemitenteUpdateDTO updateDTO)
        {
            try
            {
                var updated = await _remitenteService.UpdateAsync(id, updateDTO);
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
                await _remitenteService.DeleteAsync(id);
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
        public async Task<IActionResult> Cancelar(int id, [FromBody] InfoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _remitenteService.CancelAsync(id, infoRequestDto);
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
        }
    }
}
