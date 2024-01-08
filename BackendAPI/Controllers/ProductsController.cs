using Application.Catalog.Products;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Catalog.Products;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]//Authorize : authentication is required to use A
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IPrivateProductService _privateProductService;
        public ProductsController(IPublicProductService publicProductService, IPrivateProductService privateProductService)
        {
            _publicProductService = publicProductService;
            _privateProductService = privateProductService;
        }


        //remove GetAll
        ////localhost:post/product
        //[HttpGet("{languageId}")]
        //public async Task<IActionResult> Get(string languageId)
        //{
        //    var products = await _publicProductService.GetAll(languageId);
        //    return Ok(products);
        //}

        //localhost:post/product?pageIndex=1&size=10&categoryId=
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(languageId, request);
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
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid) 
            { 
                BadRequest(ModelState); 
            };
            var ProductId = await _privateProductService.Create(request);
            if (ProductId == 0)
                return BadRequest();

            var product = await _privateProductService.GetById(ProductId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = ProductId }, product);
        }

        //localhost:post/product/
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            };
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
        //httpPatch :update part of the record
        [HttpPatch("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            var isSuccessful = await _privateProductService.UpdatePrice(id, newPrice);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }

        //image
        //localhost:post/product/
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            };
            var imageId = await _privateProductService.AddImages(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _privateProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        //localhost:post/product/
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            };
            var Result = await _privateProductService.UpdateImages(imageId,request);
            if (Result == 0)
                return BadRequest();

            return Ok();
        }

        //localhost:post/product/
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeteleImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            };
            var Result = await _privateProductService.DeleteImages(imageId);
            if (Result == 0)
                return BadRequest();

            return Ok();
        }

        //localhost:post/product/1
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _privateProductService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find product");
            return Ok(image);
        }
    }
}
