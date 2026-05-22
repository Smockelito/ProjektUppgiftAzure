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
        private readonly IConfiguration _config;

        public ImageController(BlobService blobService, IConfiguration config)
        {
            _blobService = blobService;
            _config = config;
        }

        // GET api/Image/hero
        [AllowAnonymous]
        [HttpGet("hero")]
        public async Task<IActionResult> GetHero()
        {
            var blobName = _config["AzureStorage:HeroBlobName"]
                ?? throw new InvalidOperationException("AzureStorage:HeroBlobName is not configured.");
            var sasUrl = await _blobService.GetSasUrlAsync(blobName, TimeSpan.FromHours(1));
            return Ok(new { url = sasUrl });
        }

        // GET api/Image/home
        [HttpGet("home")]
        public async Task<IActionResult> GetHome()
        {
            var blobName = _config["AzureStorage:HomeBlobName"]
                ?? throw new InvalidOperationException("AzureStorage:HomeBlobName is not configured.");
            var sasUrl = await _blobService.GetSasUrlAsync(blobName, TimeSpan.FromHours(1));
            return Ok(new { url = sasUrl });
        }

        // GET api/Image/default-session
        [HttpGet("default-session")]
        public async Task<IActionResult> GetDefaultSession()
        {
            var blobName = _config["AzureStorage:DefaultSessionBlobName"]
                ?? throw new InvalidOperationException("AzureStorage:DefaultSessionBlobName is not configured.");
            var sasUrl = await _blobService.GetSasUrlAsync(blobName, TimeSpan.FromHours(1));
            return Ok(new { url = sasUrl });
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
