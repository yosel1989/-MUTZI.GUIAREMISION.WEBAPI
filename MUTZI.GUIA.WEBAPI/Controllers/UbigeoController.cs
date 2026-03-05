using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Ubigeo;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Ubigeo;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("ubigeo")]
    [ApiController]
    public class UbigeoController : ControllerBase
    {
        private readonly IUbigeoService _service;
        private readonly IMemoryCache _cache;
        public UbigeoController(IUbigeoService service, IMemoryCache memoryCache)
        {
            _service = service;
            _cache = memoryCache;
        }
        [HttpGet]
        [Route("departamentos")]
        [SwaggerOperation(
        Summary = "Listar departamentos",
        Description = "Lista los departamentos",
        OperationId = "listar_departamentos")]
        [ProducesResponseType(typeof(IEnumerable<UbigeoDepartamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UbigeoDepartamentoDto>>> GetAll()
        {
            try
            {
                var departamentos = await _cache.GetOrCreateAsync("departamentos", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                    return await _service.GetDepartamentoAsync();
                });

                if (departamentos == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = "No se pudo obtener la lista de departamentos."
                    });
                }

                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar departamentos.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("provincias-by-departamento/{idDepartamento}")]
        [SwaggerOperation(
        Summary = "Buscar provincias por departamento",
        Description = "Buscar provincias por departamento",
        OperationId = "buscar_provincias_por_departamento")]
        [ProducesResponseType(typeof(IEnumerable<UbigeoProvinciaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UbigeoProvinciaDto>>> GetByDepartamento(string idDepartamento)
        {
            try
            {
                var provincias = await _cache.GetOrCreateAsync("provincias", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return await _service.GetProvinciaAsync(idDepartamento);
                });
                if (provincias==null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener Ubigeo con el inicial del código: {idDepartamento}. Verifique si el inicial del código existe en la bbdd."
                    });
                }
                return Ok(provincias);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar provincias.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet]
        [Route("distritos-by-provincia/{idProvincia}")]
        [SwaggerOperation(
        Summary = "Buscar distritos por provincia",
        Description = "Buscar distritos por provincia",
        OperationId = "buscar_distritos_por_provincia")]
        [ProducesResponseType(typeof(IEnumerable<UbigeoDistritoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UbigeoDistritoDto>>> GetByProvincia(string idProvincia)
        {
            try
            {
                var distritos = await _cache.GetOrCreateAsync("distritos", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return await _service.GetDistritoAsync(idProvincia);
                });
                if (distritos == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo listar los distritos con el inicial del código: {idProvincia}. Verifique si el inicial del código existe en la bbdd."
                    });
                }

                return Ok(distritos);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar distritos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("{idUbigeo}")]
        [SwaggerOperation(
        Summary = "Buscar ubigeo por ID",
        Description = "Buscar ubigeo por ID",
        OperationId = "buscar_ubigeo_por_id")]
        [ProducesResponseType(typeof(UbigeoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UbigeoDto>> GetById(string idUbigeo)
        {
            try
            {
                var result = await _service.FindByIdAsync(idUbigeo);
                if (result == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraro registro",
                        Detalle = $"No se pudo obtener el ubigeo con el código: {idUbigeo}. Verifique si el código existe en la bbdd."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al obtener el ubigeo.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
