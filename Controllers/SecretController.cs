using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        private readonly string _blobSasToken;

        public SecretController(IConfiguration configuration)
        {
            // Use null-coalescing operator to ensure a non-null value
            _blobSasToken = configuration["BlobSasToken"] ?? "secret not found";
        }

        [HttpGet]
        public IActionResult GetSecret()
        {
            return Ok(new { Secret = _blobSasToken });
        }
    }
}
