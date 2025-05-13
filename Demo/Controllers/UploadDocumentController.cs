using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCoreHost;
[Route("api/[controller]")]
[ApiController]
public class UploadDocumentController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public UploadDocumentController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpPost("[action]")]
    public ActionResult UploadDocument(IFormFile myFile)
    {

        try
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Document");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (var fileStream = System.IO.File.Create(Path.Combine(path, myFile.FileName)))
            {
                myFile.CopyTo(fileStream);
            }
        }
        catch
        {
            return BadRequest();
        }
        return Ok();
    }

}