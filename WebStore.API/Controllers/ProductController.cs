using Microsoft.AspNetCore.Mvc;
using WebStore.Application.Dtos.Product;
using WebStore.Application.Services.Product;

namespace WebStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService produtService) : Controller
    {
        [HttpGet("{webStoreId}/products")]
        public async Task<IActionResult> GetProducts([FromRoute] Guid webStoreId)
        {
            var products = await produtService.GetAllProductsFromWebStoreAsync(webStoreId);
            return Ok(products);
        }

        [HttpGet("{webStoreId}/products/{productId}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            var product = await produtService.GetProductByIdAsync(productId);
            if (product is null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost("{webStoreId}/products")]
        public async Task<IActionResult> CreateProduct([FromRoute] Guid webStoreId, [FromBody] ProductCreateDto productCreateDto)
        {
            var createdProduct = await produtService.CreateProductAsync(webStoreId, productCreateDto);
            if (createdProduct is null)
                return BadRequest("Failed to create product.");
            return CreatedAtAction(nameof(GetProductById), new { webStoreId, productId = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{webStoreId}/products/{productId}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] ProductUpdateDto productUpdateDto)
        {
            if (productId != productUpdateDto.Id)
                return BadRequest("Product ID mismatch.");

            var updatedProduct = await produtService.UpdateProductAsync(productUpdateDto);
            if (updatedProduct is null)
                return BadRequest("Failed to update product.");
            return Ok(updatedProduct);
        }

        [HttpDelete("{webStoreId}/products/{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var product = await produtService.GetProductByIdAsync(productId);
            if (product is null)
            {
                return NotFound();
            }                
            await produtService.DeleteProductAsync(productId);
            return Ok();
        }
    }
}
