using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace SORTEO.LAGRANDE.WEBAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/doc")]
    public class DocsController : Controller
    {
        [HttpGet()]
        public IActionResult Redoc()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "redoc.html");

                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "text/html";
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }


}