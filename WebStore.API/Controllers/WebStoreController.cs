using Microsoft.AspNetCore.Mvc;
using WebStore.Application.Dtos.WebStore;
using WebStore.Application.Services.WebStore;

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
            var createdWebStore = await webStoreService.CreateWebStoreAsync(webStoreCreateDto);
            if (createdWebStore is null)
            {
                return BadRequest("Failed to create web store.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdWebStore.Id }, createdWebStore);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WebStoreUpdateDto webStoreCreateDto)
        {            
            var updatedWebStore = await webStoreService.UpdateWebStoreAsync(webStoreCreateDto);
            if (updatedWebStore is null)
            {
                return BadRequest("Failed to update web store.");
            }
            return Ok(updatedWebStore);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var webStore = await webStoreService.GetWebStoreByIdAsync(id);
            if (webStore is null)
            {
                return NotFound();
            }

            await webStoreService.DeleteWebStoreAsync(id);
            return Ok();
        }
    }
}
