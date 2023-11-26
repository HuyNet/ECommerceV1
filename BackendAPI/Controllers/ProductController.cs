using Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _privateProductService;
        public  ProductController(IPublicProductService privateProductService)
        {
            _privateProductService = privateProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _privateProductService.GetAll();
            return Ok("test Ok");
        }
    }
}
