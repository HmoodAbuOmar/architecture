using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Models;
using KASHOP.PL.Resourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResource> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> index()
        {
            var response = await _categoryService.GetAllCategoriesForAdmin();
            return Ok(new { message = _localizer["Success"].Value, response });
        }



        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {
            var response = await _categoryService.CreateCategory(request);
            return Ok(new { message = _localizer["Success"].Value });
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryRequest request)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var result = await _categoryService.ToggleStatus(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
