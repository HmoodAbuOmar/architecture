using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService category, IStringLocalizer<SharedResource> localizer)
        {
            _category = category;
            _localizer = localizer;
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _category.CreateCategory(request);
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}
