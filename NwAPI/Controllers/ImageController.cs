using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NwAPI.Services;

namespace NwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly BlobService _blobService;

        public ImageController(BlobService blobService)
        {
            _blobService = blobService;
        }

        // POST api/Image/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Ingen fil vald.");

            var url = await _blobService.UploadAsync(file);
            return Ok(new { url });
        }
    }
}
