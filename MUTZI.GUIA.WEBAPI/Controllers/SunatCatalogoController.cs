using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("sunat-catalogo")]
    [ApiController]
    [Authorize]
    public class MotivoTrasladoController : ControllerBase
    {
        private readonly IMotivoTrasladoService _service;
        private readonly IInvoiceTypeService _invoiceTypeService;
        public MotivoTrasladoController(
            IMotivoTrasladoService service,
            IInvoiceTypeService invoiceTypeService
        )
        {
            _service = service;
            _invoiceTypeService = invoiceTypeService;
        }


        [HttpGet]
        [Route("motivos-traslado")]
        [SwaggerOperation(
            Summary = "Listar los motivos de traslado",
            Description = "Lista los motivos de traslados activos para seleccionar",
            OperationId = "listar_motivos_traslado"
        )]
        [ProducesResponseType(typeof(List<MotivoTrasladoToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MotivoTrasladoToSelectDTO>>> GetAllToSelect()
        {
            try
            {
                var motivosTraslado = await _service.ListToSelectAsync();
                return Ok(motivosTraslado);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar los motivos de traslado.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet]
        [Route("reason-for-transfers-assigned")]
        [SwaggerOperation(
            Summary = "Listar los motivos de traslado asignados",
            Description = "Lista los motivos de traslados asignados para seleccionar",
            OperationId = "listar_motivos_traslados_asignados"
        )]
        [ProducesResponseType(typeof(List<MotivoTrasladoToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MotivoTrasladoToSelectDTO>>> GetAllAssignedToSelect()
        {
            try
            {
                var motivosTraslado = await _service.ListToSelectByAssignedAsync();
                return Ok(motivosTraslado);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar los motivos de traslado asignados.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("invoice-types")]
        [SwaggerOperation(
            Summary = "Listar tipos de documentos comerciales",
            Description = "Lista todos los documentos comerciales",
            OperationId = "invoice_type_to_select"
        )]
        [ProducesResponseType(typeof(List<InvoiceTypeToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<InvoiceTypeToSelectDTO>>> GetToSelect()
        {
            try
            {
                var result = await _invoiceTypeService.GetInvoiceTypeToSelect();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar los documentos comerciales del catalogo.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
