using KASHOP.DAL.Data;
using KASHOP.PL.Resourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult index()
        {
            //return Ok(_localizer["Success"].Value);
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}
