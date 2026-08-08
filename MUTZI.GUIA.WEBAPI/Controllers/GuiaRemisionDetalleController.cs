using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("guia-remision-detalle")]
    [ApiController]
    [Authorize]
    public class GuiaRemisionDetalleController : ControllerBase
    {

        public GuiaRemisionDetalleController(

        )
        {

        }


        /*[AllowAnonymous]
        [HttpGet]
        [Route("download-template")]
        [SwaggerOperation(
            Summary = "Descargar la plantilla EXCEL",
            Description = "Descargar la plantilla EXCEL para subir los productos al detalle de una guía",
            OperationId = "download_template"
        )]
        [Permission("download_template")]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownloadTemplateDetail()
        {
            try
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Plantilla");
                var wsCatalogo = wb.Worksheets.Add("Catalogos");


                var unidadesMedida = await _catalogoService.GetUnidadesMedida(null);
                int row = 1;
                foreach (var medida in unidadesMedida)
                {
                    wsCatalogo.Cell(row, 1).Value = medida.UnidadMedidaId;
                    wsCatalogo.Cell(row, 2).Value = medida.Descripcion;
                    wsCatalogo.Cell(row, 3).Value = medida.DescripcionCorta;
                    row++;
                }

                // Encabezados
                ws.Cell(1, 1).Value = "Código";
                ws.Cell(1, 2).Value = "Nombre";
                ws.Cell(1, 3).Value = "Tipo de Medida";
                ws.Cell(1, 4).Value = "Cantidad";


                // Validación en la columna C
                var rangoValidacion = ws.Range("C2:C100");

                // Crear un nuevo objeto de validación
                var dv = rangoValidacion.CreateDataValidation();

                // Asignar lista inline (valores separados por comas, entre comillas)
                dv.List($"Catalogos!$B$1:$B${unidadesMedida.Count()}");
                rangoValidacion.Style.Protection.SetLocked(true);
                wsCatalogo.Protect("claveSegura");
                // Exportar a memoria
                using var stream = new MemoryStream();
                wb.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                // Devolver como archivo descargable
                return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "Plantilla.xlsx");
            }
            catch (InvalidOperationException ex)
            {
                var errorResponse = new BadRequestResponseDTO
                {
                    Error = "Operación inválida",
                    Detalle = ex.Message
                };
                return BadRequest(errorResponse);
            }
        }*/


        [AllowAnonymous]
        [HttpPost]
        [Route("import-data")]
        [SwaggerOperation(
            Summary = "Subir la plantilla EXCEL",
            Description = "Recibe la plantilla EXCEL con los productos al detalle de una guía",
            OperationId = "import_data"
        )]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadTemplateDetail(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new BadRequestResponseDTO
                {
                    Error = "Archivo inválido",
                    Detalle = "No se recibió ningún archivo"
                });
            }

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);

                using var wb = new XLWorkbook(stream);
                var ws = wb.Worksheet("Plantilla");

                var resultados = new List<GuiaRemisionDetalleDTO>();

                // Leer filas desde la 2 (porque la 1 son encabezados)
                foreach (var row in ws.RowsUsed().Skip(1))
                {
                    var codigo = row.Cell(1).GetString();
                    var nombre = row.Cell(2).GetString();
                    var tipoMedida = row.Cell(3).GetString();
                    var cantidad = row.Cell(4).GetDouble();

                    // Mapear descripción a Id usando la hoja Catalogos
                    var wsCatalogo = wb.Worksheet("Catalogos");
                    var match = wsCatalogo.RowsUsed()
                        .Select(r => new { Id = r.Cell(1).GetString(), Desc = r.Cell(2).GetString() })
                        .FirstOrDefault(m => m.Desc == tipoMedida);

                    resultados.Add(new GuiaRemisionDetalleDTO
                    {
                        Codigo = codigo,
                        Descripcion = nombre,
                        UnidadMedidaId = (match?.Id != null) ? Convert.ToInt32(match?.Id) : 0,
                        Cantidad = cantidad.ToString()
                    });
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new InternalErrorResponseDTO
                {
                    Error = "Error al procesar el archivo",
                    Detalle = ex.Message
                });
            }
        }

    }
}

