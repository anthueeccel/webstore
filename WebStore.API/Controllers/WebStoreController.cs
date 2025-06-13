using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Application.Dtos.WebStore;
using WebStore.Application.Helpers;
using WebStore.Application.WebStore.Services;

namespace WebStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebStoreController(IWebStoreService webStoreService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var webstores = await webStoreService.GetAllWebStoresAsync();
            return Ok(webstores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var webstore = await webStoreService.GetWebStoreByIdAsync(id);
            if (webstore is null)
            {
                return NotFound();
            }
            return Ok(webstore);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WebStoreCreateDto webStoreCreateDto)
        {
            var validationResult = WebStoreCreateDtoValidator.Validate(webStoreCreateDto);
            if (!validationResult.Item1)
            {
                return BadRequest($"Web store creation failed: {validationResult.Item2}");                
            }

            var createdWebStore = await webStoreService.CreateWebStoreAsync(webStoreCreateDto);
            if (createdWebStore is null)
            {
                return BadRequest("Failed to create web store.");
            }
            
            return CreatedAtAction(nameof(GetById), new { id = createdWebStore.Id }, createdWebStore);
        }
    }
}
