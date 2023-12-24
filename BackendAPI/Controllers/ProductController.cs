using Application.Catalog.Products;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Catalog.Products;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IPrivateProductService _privateProductService;
        public  ProductController(IPublicProductService publicProductService, IPrivateProductService privateProductService)
        {
            _publicProductService = publicProductService;
            _privateProductService = privateProductService;
        }

        //localhost:post/product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId)
        {
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
        }

        //localhost:post/product/public-paging
        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> Get([FromQuery]GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request);
            return Ok(products);
        }

        //localhost:post/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var products = await _privateProductService.GetById(productId, languageId);
            if (products == null)
                return BadRequest("Cannot find product");
            return Ok(products);
        }

        //localhost:post/product/
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            var ProductId = await _privateProductService.Create(request);
            if (ProductId == 0)
                return BadRequest();

            var product = await _privateProductService.GetById(ProductId,request.LanguageId);

            return CreatedAtAction(nameof(GetById), new {id = ProductId} ,product);
        }

        //localhost:post/product/
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var affectedResult = await _privateProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        //localhost:post/product/
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _privateProductService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        //localhost:post/product/
        [HttpPut("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id,decimal newPrice)
        {
            var isSuccessful = await _privateProductService.UpdatePrice(id,newPrice);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }

    }
}
