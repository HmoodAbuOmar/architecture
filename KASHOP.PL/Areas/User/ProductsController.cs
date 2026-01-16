using KASHOP.BLL.Service;
using KASHOP.PL.Resourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResource> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> index([FromQuery] string lang = "en")
        {
            var response = await _productService.GetAllProductsForUser(lang);
            return Ok(new { message = _localizer["Success"].Value, response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> index([FromRoute] int id, [FromQuery] string lang = "en")
        {
            var response = await _productService.GetAllProductsDetailsForUser(id, lang);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
