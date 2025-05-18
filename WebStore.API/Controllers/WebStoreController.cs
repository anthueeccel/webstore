using Microsoft.AspNetCore.Mvc;
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
    }
}
