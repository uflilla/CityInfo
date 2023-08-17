using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
  [Route("api/files")]
  [ApiController]
  public class FileController : ControllerBase
  {
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public FileController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
      _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
    }
    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
      if (!System.IO.File.Exists(fileId)) return NotFound();
      var bytes = System.IO.File.ReadAllBytes(fileId);
      if (!_fileExtensionContentTypeProvider.TryGetContentType(fileId, out var contentType))
      {
        contentType = "application/octet-stream";
      };
      return File(bytes, contentType, Path.GetFileName(fileId));

    }
  }
}
