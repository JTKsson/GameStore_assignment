using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _blobSasToken;

        public SecretController(IConfiguration configuration)
        {
            _blobSasToken = configuration["BlobSasToken"];
        }

        [HttpGet]
        public IActionResult GetSecret()
        {
            return Ok(new { Secret = _blobSasToken });
        }
    }
}
