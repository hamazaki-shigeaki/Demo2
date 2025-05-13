using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCoreHost;
[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public UploadController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpPost("[action]")]

    public ActionResult Upload(IFormFile myFile)
    {

        try
        {
            string suitNo = Request.Form["SuitNo"];

            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Upload" + @"\" + suitNo);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (var fileStream = System.IO.File.Create(Path.Combine(path, myFile.FileName )))
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