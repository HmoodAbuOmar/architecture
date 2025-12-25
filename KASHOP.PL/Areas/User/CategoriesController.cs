using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService category, IStringLocalizer<SharedResource> localizer)
        {
            _category = category;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> index([FromQuery] string lang = "en")
        {
            var response = await _category.GetAllCategoriesForUser(lang);
            return Ok(new { message = _localizer["Success"].Value, response });
        }

    }
}
